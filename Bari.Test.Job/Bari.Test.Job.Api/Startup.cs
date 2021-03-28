using EasyCaching.Redis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Bari.Test.Job.Infra.IoC;

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


            //services.AddDbContext<ContactsDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("ContactsDbConnection"));
            //});


            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            //configuration
            services.AddEasyCaching(options =>
            {
                //use memory cache that named default
                options.UseInMemory("default");

                // // use memory cache with your own configuration
                // options.UseInMemory(config => 
                // {
                //     config.DBConfig = new InMemoryCachingOptions
                //     {
                //         // scan time, default value is 60s
                //         ExpirationScanFrequency = 60, 
                //         // total count of cache items, default value is 10000
                //         SizeLimit = 100 
                //     };
                //     // the max random second will be added to cache's expiration, default value is 120
                //     config.MaxRdSecond = 120;
                //     // whether enable logging, default is false
                //     config.EnableLogging = false;
                //     // mutex key's alive time(ms), default is 5000
                //     config.LockMs = 5000;
                //     // when mutex key alive, it will sleep some time, default is 300
                //     config.SleepMs = 300;
                // }, "m2");

                //use redis cache that named redis1
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(Configuration.GetConnectionString("RedisCacheConnection"), 6379));
                }, "redis1")
                .WithMessagePack()//with messagepack serialization
                ;
            });



            //var connRabbit = Configuration.GetConnectionString("RabbitMQConnection");

            //var factoryRabbit = new ConnectionFactory()
            //{
            //    Uri = new Uri(connRabbit),
            //    AutomaticRecoveryEnabled = true
            //};

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();



            services
                .AddHealthChecks()
                .AddRedis(Configuration.GetConnectionString("RedisCacheConnection"), failureStatus: HealthStatus.Degraded);
                //.AddRabbitMQ(Configuration.GetConnectionString("RabbitMQConnection"), name: "rabbitmq", failureStatus: HealthStatus.Degraded);
            //.AddSqlServer(Configuration.GetConnectionString("GatewayTIMDbConnection"), failureStatus: HealthStatus.Degraded)
            //

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice Message OnLine Documentation", Version = "v1" });
            });


            services.AddMediatR(typeof(Startup));

            services.AddAutoMapper(typeof(Startup));

            RegisterServices(services);

        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/hc-ui";
                options.ApiPath = "/hc-ui-api";
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
