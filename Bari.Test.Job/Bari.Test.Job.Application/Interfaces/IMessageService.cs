using Bari.Test.Job.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<IEnumerable<MessageViewModel>> GetAll();
    }
}
