using System;
using System.Collections.Generic;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Bari.Test.Job.Tests.Repositories
{
    public class EntityFakeRepository : IRepository<Entity>
    {
        public Task Bind<Y>(Y entities, string named = null)
        {
            Y value = entities;
            return Task.FromResult<Y>(entities);
        }

        public Task<Entity> Create(Entity item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Entity> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<R> GetBy<K, R>(K key)
        {
            R value = default(R);
            return Task.FromResult<R>(value);
        }

        public Task Update(Entity item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entity>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}