using AutoMapper;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.Mappers;
using Bari.Test.Job.Application.Services;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events.Bus.MQ;
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

namespace Bari.Test.Job.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Redis Cache
            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddSingleton<IDatabase>(c => new RedisConnectionFactory().Connection().GetDatabase());

            //Application Services
            services.AddTransient<IMessageService, MessageService>();

            //Handlers
            var repositories = new List<IRepository<Message>>
            {
                new MessageRepository(),
                new MessageCacheRepository(new RedisConnectionFactory().Connection().GetDatabase())
            };
            //Handlers queries            
            services.AddTransient(c => new MessageQueryHandler(repositories, new EntityCacheRepository(new RedisConnectionFactory().Connection().GetDatabase())));
            services.AddTransient<IRequestHandler<MessageGetAllQuery, IQueryResult>, MessageQueryHandler>();
            //Handlers commands
            services.AddTransient(c => new MessageCommandHandler(repositories, new EntityCacheRepository(new RedisConnectionFactory().Connection().GetDatabase())));
            services.AddTransient<IRequestHandler<SendMessageCommand, ICommandResult>, MessageCommandHandler>();
            //Handlers events
            services.AddTransient<MessageEventHandler>();

            //Repositories
            //Repositories Data
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
            services.AddTransient<IEventBus, RabbitMQBus>();
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });
        }
    }
}
