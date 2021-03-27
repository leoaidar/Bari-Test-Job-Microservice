using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bari.Test.Job.Infra.Data;
using EasyCaching.Core;
using Bari.Test.Job.Infra.Data.Cache;

namespace Pasquali.Sisprods.Infra.Data.Cache
{
    public abstract class RedisCacheContext 
    {
        public readonly IDatabaseCache _connection;
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly IEasyCachingProviderFactory _cachingProviderFactory;
        //private readonly AppConfiguration _appConfiguration = new AppConfiguration();

        public RedisCacheContext(IEasyCachingProviderFactory cachingProviderFactory)
        {
            var connectionStringCacheChannel = new AppConfiguration().AppSettings.GetConnectionString("RedisCacheChannel");
            _cachingProviderFactory = cachingProviderFactory;            
            _cachingProvider = _cachingProviderFactory.GetCachingProvider(connectionStringCacheChannel);
            _connection = (IDatabaseCache)_cachingProvider;
        }

    }

}
