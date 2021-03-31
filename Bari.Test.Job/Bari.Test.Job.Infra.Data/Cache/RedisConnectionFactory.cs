using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace Bari.Test.Job.Infra.Data.Cache
{
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;        
        private readonly AppConfiguration _appConfiguration = new AppConfiguration();
        
        public RedisConnectionFactory()
        {            
            var connectionString = _appConfiguration.AppSettings.GetConnectionString("RedisCacheConnection");
            this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));                   
        }

        public ConnectionMultiplexer Connection()
        {
            return this._connection.Value;
        }

    }

}
