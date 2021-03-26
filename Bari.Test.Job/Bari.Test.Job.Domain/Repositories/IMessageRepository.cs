using Bari.Test.Job.Domain.Entities;
using System.Collections.Generic;

namespace Bari.Test.Job.Domain.Repositories
{
    public interface IMessageRepository
    {
        void Create(Message Message);
        void Update(Message Message);
        Message GetById(int id);
        IEnumerable<Message> GetAll();
        void Delete(Message Message);
        void Bind<T>(T entities, string named = null);
    }

}