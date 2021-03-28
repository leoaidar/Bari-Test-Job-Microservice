using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Domain.Entities;
using System.Threading.Tasks;
using Bari.Test.Job.Infra.Data.Cache;

namespace Bari.Test.Job.Infra.Data.Repositories
{
    public class EntityCacheRepository : RedisCacheBaseRepository, IRepository<Entity>
    {
        private readonly IDatabase _cacheDB;

        public EntityCacheRepository(IDatabase cacheDB)
        {
            _cacheDB = cacheDB;
        }

        public async Task<IEnumerable<Entity>> GetAll()
        {
            return await GetObjectAsync<IEnumerable<Entity>>(_cacheDB, "Entities");
        }

        public async Task<Entity> Get(Guid id)
        {
            return await GetObjectAsync<Entity>(_cacheDB, $"Entity_{id}");
        }

        public async Task<Entity> Create(Entity entity)
        {
            var recorded = await SetObjectAsync<Entity>(_cacheDB, $"Entity_{entity.Id}", entity);
            return recorded ? entity : null;
        }

        public async Task Update(Entity entity)
        {
            await SetObjectAsync<Entity>(_cacheDB, $"Entity_{entity.Id}", entity);
        }

        public async Task Delete(Guid id)
        {
            await DelObjectAsync(_cacheDB, $"Entity_{id}");
        }

        public async Task Bind<Y>(Y entities, string named = null)
        {
            await SetObjectAsync<Y>(_cacheDB, named ?? typeof(Y).Name, entities);
        }

        public async Task<R> GetBy<K, R>(K key)
        {
            return await GetObjectAsync<R>(_cacheDB, $"{key}");
        }
    }
}
