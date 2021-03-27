using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Repositories
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();

        public Task<T> Get(Guid id);

        public Task<R> GetBy<K,R>(K key);

        public Task<T> Create(T item);

        public Task Update(T item);

        public Task Delete(Guid id);

        public Task Bind<Y>(Y entities, string named = null);
    }
}

