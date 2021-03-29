using AutoMapper;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.Mappers;
using Bari.Test.Job.Application.Services;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Domain.Queries;
using Bari.Test.Job.Domain.Queries.Contracts;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Infra.Bus;
using Bari.Test.Job.Infra.Data.Cache;
using Bari.Test.Job.Infra.Data.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Collections.Generic;
using Bari.Test.Job.Infra.Data.Contexts;

namespace Bari.Test.Job.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //SQL Server
            services.AddTransient<MessagesDbContext>();

            //Redis Cache
            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IDatabase>(c => new RedisConnectionFactory().Connection().GetDatabase());

            //////Application Services
            services.AddTransient<IMessageService, MessageService>();

            //Handlers
            //Handlers commands
            services.AddTransient<MessageCommandHandler, MessageCommandHandler>();
            services.AddTransient<IRequestHandler<SendMessageCommand, ICommandResult>, MessageCommandHandler>();
            //Handlers queries
            services.AddTransient<IRequestHandler<MessageGetAllQuery, IQueryResult>, MessageQueryHandler>();
            var repositories = new List<IRepository<Message>>
            {
                new MessageRepository(),
                new MessageCacheRepository(new RedisConnectionFactory().Connection().GetDatabase())
            };
            services.AddTransient(c => new MessageQueryHandler(repositories, new EntityCacheRepository(new RedisConnectionFactory().Connection().GetDatabase())));
            //Handlers events
            //services.AddTransient<MessageEventHandler, MessageEventHandler>();
            //services.AddTransient<IRequestHandler<MessageCreatedEvent, IEventResult>, MessageEventHandler>();

            //Repositories
            //Repositories Data
            services.AddTransient<MessageRepository>();
            services.AddSingleton<IRepository<Message>, MessageRepository>();
            //Repositories cache
            services.AddSingleton<IRepository<Message>, MessageCacheRepository>();
            services.AddSingleton<IRepository<Entity>, EntityCacheRepository>();

            //MediatR
            services.AddSingleton<IMediator, Mediator>();

            //Mappers            
            services.AddSingleton<IMapper, Mapper>();
            services.AddAutoMapper(typeof(AutoMapping));

            //Domain Bus MQ
            //Before Refactor adding nuget Microsoft.DependencyInjection, IServiceScopeFactory in RabbitMQBus;
            services.AddTransient<IEventBus, RabbitMQBus>();
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);

            });

            //Jobs
            //services.AddSingleton<IMessageJob, MessageJob>();

        }
    }
}
