﻿using AutoMapper;
using MediatR;
using Bari.Test.Job.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;
//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.Mappers;
using Bari.Test.Job.Application.Services;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Domain.Queries;
using Bari.Test.Job.Domain.Queries.Contracts;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Infra.Data.Repositories;
using Bari.Test.Job.Infra.Data.Cache;
using EasyCaching.Core;
//using Bari.Test.Job.Infra.Data.Contexts;
//using Bari.Test.Job.Infra.Data.Repositories;

namespace Bari.Test.Job.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {

            //Repositories
            //services.AddTransient<MessagesDbContext>();
            //services.AddTransient<IEasyCachingProvider, IDatabaseCache>();
            services.AddSingleton<RedisCacheContext>();
            //Repositories Data
            services.AddSingleton<IRepository<Message>, MessageRepository>();
            //Repositories cache
            services.AddSingleton<IRepository<Message>, MessageCacheRepository>();

            //Mappers
            services.AddAutoMapper(typeof(AutoMapping));

            //Domain Bus
            //Before Refactor adding nuget Microsoft.DependencyInjection, IServiceScopeFactory in RabbitMQBus;
            services.AddTransient<IEventBus, RabbitMQBus>();
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);

            });

            services.AddSingleton<IMediator, Mediator>();
            services.AddSingleton<IMapper, Mapper>();
            //services.AddScoped(typeof(IMediator), typeof(MessageQueryHandler));

            services.AddTransient<IRequestHandler<MessageGetAllQuery, IQueryResult>, MessageQueryHandler>();
            //services.AddScoped<IRequestHandler<CreateCartCommand, ICommandResult>, CartCommandHandler>();

            //Redis Cache
            ////services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            ////services.AddSingleton<IDatabase>(c => new RedisConnectionFactory().Connection().GetDatabase());
            
            //Handlers
            //Handlers commands
            services.AddTransient<MessageCommandHandler, MessageCommandHandler>();
            //Handlers queries
            services.AddTransient<MessageQueryHandler, MessageQueryHandler>();
            

            services.AddScoped(typeof(IMediator), typeof(MessageQueryHandler));
            var repositories = new List<IRepository<Message>>
            {
                new MessageRepository(),
                new MessageCacheRepository()
            };
            services.AddTransient(c => new MessageQueryHandler(repositories));

            //////Application Services
            services.AddTransient<IMessageService, MessageService>();



        }
    }
}
