using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;
using Pasquali.Sisprods.Infra.Data.Cache;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Domain.Entities;
using System.Threading.Tasks;
using Bari.Test.Job.Infra.Data.Cache;

namespace Pasquali.Sisprods.Infra.Data.Repositories
{
    public class MessageCacheRepository : IRepository<Message>
    {
        private RedisCacheContext _ctx;

        public MessageCacheRepository(RedisCacheContext ctx)
        {
            _ctx = ctx;
        }


        public async Task<IEnumerable<Message>> GetAll()
        {
            var value = await _ctx._connection.GetAsync<IEnumerable<Message>> ("Messages");
            return ((IEnumerable<Message>)(value.HasValue ? JsonConvert.DeserializeObject<IEnumerable<Message>>(value.ToString()) : default));
        }

        public async Task<Message> Get(Guid id)
        {
            var value = await _ctx._connection.GetAsync<Message>($"Message_{id}");
            return (Message)(value.HasValue ? JsonConvert.DeserializeObject<Message>(value.ToString()) : default);
        }

        public async Task<Message> Create(Message message)
        {
            await _ctx._connection.SetAsync<string>($"Message_{message.Id}", JsonConvert.SerializeObject(message, Formatting.Indented,
                                             new JsonSerializerSettings()
                                             {
                                                 ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                             }
                                         ), TimeSpan.FromDays(100));
            return await _ctx._connection.ExistsAsync($"Message_{message.Id}") ? message : null;

            //await _ctx._connection.SetAsync<Message>($"Message_{message.Id}", message, TimeSpan.FromDays(100));
        }

        public async Task Update(Message message)
        {
            await _ctx._connection.SetAsync<Message>($"Message_{message.Id}", message, TimeSpan.FromDays(100));
        }

        public async Task Delete(Guid id)
        {
            await _ctx._connection.RemoveAsync($"Message_{id}");
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
