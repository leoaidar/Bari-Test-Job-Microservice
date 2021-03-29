
using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Bari.Test.Job.Infra.Data.Contexts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Domain.Queries;
using System.Threading.Tasks;

namespace Bari.Test.Job.Infra.Data.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        public static Dictionary<int, Message> messages = new Dictionary<int, Message>();

        public MessageRepository()
        {
            messages.Add(messages.Count() + 1, new Message("Léo" + messages.Count() + 1, "M"));
            messages.Add(messages.Count() + 1, new Message("Jamiles" + messages.Count() + 1, "F"));
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return await Task.Run(() => messages.Values.ToList());
        }

        public async Task<Message> Get(Guid id)
        {
            return await Task.Run(() => messages.FirstOrDefault(x => x.Value.Id == id).Value);
        }

        public async Task<Message> Create(Message msg)
        {
            return await Task.Run(() => {

                messages.Add(messages.Count() + 1, msg);
                return msg;
            });
        }

        public async Task Update(Message msg)
        {
            await Task.Run(() =>
            {
                messages.Remove(messages.FirstOrDefault(x => x.Value.Id == msg.Id).Key);
                messages.Add(messages.Count() + 1, msg);
            });
        }

        public async Task Delete(Guid id)
        {
            await Task.Run(() => messages.Remove(messages.FirstOrDefault(x => x.Value.Id == id).Key));
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
