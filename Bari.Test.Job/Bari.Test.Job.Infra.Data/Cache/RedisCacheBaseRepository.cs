using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Bari.Test.Job.Infra.Data.Cache
{
    public class RedisCacheBaseRepository
    {

        protected async Task<bool> SetObjectAsync<T>(IDatabase cache, string key, T value)
        {
            bool v = await cache.StringSetAsync(key, JsonConvert.SerializeObject(value, Formatting.Indented,
                                             new JsonSerializerSettings()
                                             {
                                                 ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                             }
                                         ));
            return v;
        }

        protected async Task<T> GetObjectAsync<T>(IDatabase cache, string key)
        {
            var value = await cache.StringGetAsync(key);
            return ((T) (value.HasValue ? JsonConvert.DeserializeObject<T>(value.ToString()) : default));
        }

        protected async Task DelObjectAsync(IDatabase cache, string key)
        {
            await cache.KeyDeleteAsync(key);
        }

        protected async Task<bool> ExistObjectAsync<T>(IDatabase cache, string key)
        {
            var value = await cache.StringGetAsync(key);
            return value.HasValue;
        }



    }
}
