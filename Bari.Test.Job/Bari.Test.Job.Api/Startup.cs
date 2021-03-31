using Bari.Test.Job.Api.Jobs;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Infra.IoC;
using Hangfire;
using Hangfire.MemoryStorage;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;

namespace Bari.Test.Job.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<MessagesDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("MessagesDbConnection"));
            //});


            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            var connRabbit = Configuration.GetConnectionString("RabbitMQConnection");

            var factoryRabbit = new ConnectionFactory()
            {
                Uri = new Uri(connRabbit),
                AutomaticRecoveryEnabled = true
            };


            //services.AddHealthChecksUI()
            //        .AddInMemoryStorage();

            //services
            //    .AddHealthChecks()
            //    .AddRedis(Configuration.GetConnectionString("RedisCacheConnection"), failureStatus: HealthStatus.Degraded)
            //    .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQConnection"), name: "rabbitmq", failureStatus: HealthStatus.Degraded);
            //.AddSqlServer(Configuration.GetConnectionString("GatewayTIMDbConnection"), failureStatus: HealthStatus.Degraded)
            //

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice Message OnLine Documentation", Version = "v1" });
            });


            services.AddMediatR(typeof(Startup));

            services.AddAutoMapper(typeof(Startup));

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

            // Add the processing server as IHostedService
            services.AddHangfireServer();            

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {

            DependencyContainer.RegisterServices(services);

            //Jobs
            services.AddSingleton<IMessageJob, MessageJob>();
            //MQ Subscriptions
            services.AddTransient<MessageEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Messages Microservice V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            //app.UseHealthChecks("/hc", new HealthCheckOptions
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

            //nuget: AspNetCore.HealthChecks.UI
            //app.UseHealthChecksUI(options =>
            //{
            //    options.UIPath = "/hc-ui";
            //    options.ApiPath = "/hc-ui-api";
            //});

            //app.UseAuthorization();

            // Change `Back to site` link URL
            var options = new DashboardOptions { AppPath = "https://localhost:5001" };
            app.UseHangfireDashboard("/hangfire", options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecks("/hc");
                endpoints.MapHangfireDashboard("/hangfire", options);
            });
            recurringJobManager.AddOrUpdate("Run every 5 minutes", () => serviceProvider.GetService<IMessageJob>().SendMessage(), "*/5 * * * *");

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<MessageCreatedEvent, MessageEventHandler>();
        }
    }
}
