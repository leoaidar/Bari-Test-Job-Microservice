using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Tests.Repositories
{
    public class FakeMessageRepository : IRepository<Message>
    {
        public Task Bind<Y>(Y entities, string named = null)
        {
            throw new NotImplementedException();
        }

        public Task<Message> Create(Message item)
        {
            return Task.FromResult<Message>(item);
        }

        public Task Delete(Guid id)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<Message> Get(Guid id)
        {
            return Task.FromResult<Message>(new Message("GetMethod", "Task<Message> Get(Guid id)"));
        }

        public Task<R> GetBy<K, R>(K key)
        {
            throw new NotImplementedException();
        }

        public Task Update(Message item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAll()
        {
            var list =  new List<Message>{
                new Message("Leonardo","FakeMessageRepository"),
                new Message("Arthur","FakeMessageRepository"),
                new Message("Aidenir","FakeMessageRepository"),
                new Message("Aldo","FakeMessageRepository")
            };

            return Task.FromResult<IEnumerable<Message>>(list);
        }
    }
}