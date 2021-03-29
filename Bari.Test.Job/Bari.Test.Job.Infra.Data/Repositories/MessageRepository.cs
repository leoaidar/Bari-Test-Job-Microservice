
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bari.Test.Job.Infra.Data.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private MessagesDbContext _ctx;

        public MessageRepository(MessagesDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await _ctx.Messages.ToListAsync();
        }

        public async Task<Message> Get(Guid id)
        {
            return await _ctx
                .Messages
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Message> Create(Message msg)
        {
            await _ctx.Messages.AddAsync(msg);
            _ctx.SaveChanges();
            return msg;
        }

        public async Task Update(Message msg)
        {
            _ctx.Entry(msg).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var msg = await _ctx.Messages.FirstOrDefaultAsync(x => x.Id == id);
            _ctx.Messages.Remove(msg);
            await _ctx.SaveChangesAsync();
        }

        public Task Bind<Y>(Y entities, string named)
        {
            throw new NotImplementedException();
        }

        public Task<R> GetBy<K, R>(K key)
        {
            throw new NotImplementedException();
        }
    }
}
