using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Tests.Repositories
{
    public class FakeMessageRepository : IRepository<Message>
    {
        private List<Message> _list;

        public FakeMessageRepository()
        {
            _list = new List<Message>{
                new Message("Leonardo","FakeMessageRepository"),
                new Message("Arthur","FakeMessageRepository"),
                new Message("Aidenir","FakeMessageRepository"),
                new Message("Selma","FakeMessageRepository"),
                new Message("Aldo","FakeMessageRepository")
            };
        }
        public Task Bind<Y>(Y entities, string named = null)
        {
            if (named.Equals("Messages"))
                _list = entities as List<Message>;

            return Task.FromResult<bool>(true);
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
            return Task.FromResult<IEnumerable<Message>>(_list);
        }
    }
}