using EasyCaching.Core;
using Microsoft.Extensions.Configuration;

namespace Bari.Test.Job.Infra.Data.Cache
{
    public abstract class RedisCacheContext 
    {
        public readonly IDatabaseCache _connection;
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly IEasyCachingProviderFactory _cachingProviderFactory;
        //private readonly AppConfiguration _appConfiguration = new AppConfiguration();

        public RedisCacheContext()
        {

        }

        public RedisCacheContext(IEasyCachingProviderFactory cachingProviderFactory) : base()
        {
            var connectionStringCacheChannel = new AppConfiguration().AppSettings.GetConnectionString("RedisCacheChannel");
            _cachingProviderFactory = cachingProviderFactory;            
            _cachingProvider = _cachingProviderFactory.GetCachingProvider(connectionStringCacheChannel);
            _connection = (IDatabaseCache)_cachingProvider;
        }

    }

}
