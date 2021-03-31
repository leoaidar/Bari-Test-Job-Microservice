using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Infra.Data.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Infra.Data.Repositories
{
    public class MessageCacheRepository : RedisCacheBaseRepository, IRepository<Message>
    {
        private readonly IDatabase _cacheDB;

        public MessageCacheRepository(IDatabase cacheDB)
        {
            _cacheDB = cacheDB;
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await GetObjectAsync<IEnumerable<Message>>(_cacheDB, "Messages");
        }

        public async Task<Message> Get(Guid id)
        {
            return await GetObjectAsync<Message>(_cacheDB, $"Message_{id}");
        }

        public async Task<Message> Create(Message message)
        {
            var recorded = await SetObjectAsync<Message>(_cacheDB, $"Message_{message.Id}", message);
            return recorded ? message : null;
        }

        public async Task Update(Message message)
        {
            await SetObjectAsync<Message>(_cacheDB, $"Message_{message.Id}", message);
        }

        public async Task Delete(Guid id)
        {
            await DelObjectAsync(_cacheDB, $"Message_{id}");
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
