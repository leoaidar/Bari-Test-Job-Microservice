using Bari.Test.Job.Domain.Commands.Contracts;

namespace Bari.Test.Job.Domain.Handlers.Contracts
{
    public interface IHandlerCommand<C> : IHandler<C,ICommandResult>  
                        where C : ICommand  
    {
        ICommandResult Handle(C command);
    }
}