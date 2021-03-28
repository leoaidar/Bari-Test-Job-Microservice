using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Infra.Data.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pasquali.Sisprods.Infra.Data.Repositories
{
    public class EntityCacheRepository : IRepository<Entity>
    {
        private RedisCacheContext _ctx;

        public EntityCacheRepository(RedisCacheContext ctx)
        {
            _ctx = ctx;
        }


        public async Task<IEnumerable<Entity>> GetAll()
        {
            var value = await _ctx._connection.GetAsync<IEnumerable<Entity>> ("Entities");
            return ((IEnumerable<Entity>)(value.HasValue ? JsonConvert.DeserializeObject<IEnumerable<Entity>>(value.ToString()) : default));
        }

        public async Task<Entity> Get(Guid id)
        {
            var value = await _ctx._connection.GetAsync<Entity>($"{id}");
            return (Entity)(value.HasValue ? JsonConvert.DeserializeObject<Entity>(value.ToString()) : default);
        }

        public async Task<Entity> Create(Entity entity)
        {
            await _ctx._connection.SetAsync<string>($"{entity.Id}", JsonConvert.SerializeObject(entity, Formatting.Indented,
                                             new JsonSerializerSettings()
                                             {
                                                 ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                             }
                                         ), TimeSpan.FromDays(100));
            return await _ctx._connection.ExistsAsync($"{entity.Id}") ? entity : null;

            //await _ctx._connection.SetAsync<Entity>($"Entity_{entity.Id}", entity, TimeSpan.FromDays(100));
        }

        public async Task Update(Entity entity)
        {
            await _ctx._connection.SetAsync<Entity>($"{entity.Id}", entity, TimeSpan.FromDays(100));
        }

        public async Task Delete(Guid id)
        {
            await _ctx._connection.RemoveAsync($"{id}");
        }

        public async Task Bind<Y>(Y entities, string named = null)
        {
            await _ctx._connection.SetAsync<Y>(named ?? typeof(Y).Name, entities, TimeSpan.FromDays(100));
            //return await _ctx._connection.ExistsAsync(named ?? typeof(Y).Name) ? entities : null;
        }


        public async Task<R> GetBy<K, R>(K key)
        {
            var value = await _ctx._connection.GetAsync<K>(key.ToString());
            return ((R)(value.HasValue ? JsonConvert.DeserializeObject<R>(value.ToString()) : default));
        }

    }
}
