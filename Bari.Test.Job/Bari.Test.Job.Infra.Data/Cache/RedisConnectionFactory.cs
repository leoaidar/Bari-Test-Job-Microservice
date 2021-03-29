using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;

namespace Bari.Test.Job.Infra.Data.Cache
{
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;        
        private readonly IOptions<ConfigurationOptions> _redisConfigOptions;
        private readonly IOptions<RedisConfiguration> redis;
        private readonly AppConfiguration _appConfiguration = new AppConfiguration();

        
        public RedisConnectionFactory()
        {
            
            var connectionString = _appConfiguration.AppSettings.GetConnectionString("RedisCacheConnection");
            var connectionString2 = _appConfiguration.ConnectionString("RedisCacheConnection");

            this._connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
                   
        }
        public ConnectionMultiplexer Connection()
        {
            return this._connection.Value;
        }

    }

}
