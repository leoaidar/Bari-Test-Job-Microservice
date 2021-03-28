using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Application.Interfaces
{
    public interface IMessageService
    {
        public Task<IEnumerable<MessageViewModel>> GetAll(CancellationToken cancellationToken);

        public Task<CommandResult> SendMessage(SendMessageCommand command, CancellationToken cancellationToken);

    }
}
