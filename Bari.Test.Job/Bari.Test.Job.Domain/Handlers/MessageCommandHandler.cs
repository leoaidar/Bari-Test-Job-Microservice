using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageCommandHandler : Handler,
                                                IRequestHandler<SendMessageCommand, ICommandResult>
    {
        private readonly IEnumerable<IRepository<Message>> _repositories;
        private readonly IRepository<Entity> _entityRepository;
        private readonly string _genericErrorText;
        private readonly string _genericSuccessText;

        public MessageCommandHandler(IEnumerable<IRepository<Message>> repositories, IRepository<Entity> entityRepository) : base(entityRepository)
        {
            _repositories = repositories;
            _entityRepository = entityRepository;
            _genericErrorText = "Ops, parece que os dados da mensagem est�o errados!";
            _genericSuccessText = "Mensagem salva com sucesso!";
        }

        public MessageCommandHandler(IEnumerable<IRepository<Message>> repositories, IRepository<Entity> entityRepository, string genericErrorText, string genericSuccessText) : this(repositories, entityRepository)
        {
            _genericErrorText = genericErrorText;
            _genericSuccessText = genericSuccessText;
        }

        public async Task<ICommandResult> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            //test command NULL
            if (command == null)
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, null));

            //test a valid command
            command.Validate();
            if (command.Invalid)
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, command.Notifications));

            //create object
            var message = new Message(command.Body,command.ServiceId);

            //test business rules
            message.Validate();
            if (message.Invalid)
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, "Regra de neg�cio inv�lida"));

            //save in databases
            await _repositories.First().Create(message);
            await _repositories.ElementAt(1).Create(message);

            List<Message> messagesCached = (List<Message>)await _repositories.ElementAt(1).GetAll();

            if (messagesCached == null || messagesCached.Count() == 0) messagesCached = new List<Message>() { message };
            else messagesCached.Add(message);

            await _repositories.ElementAt(1).Bind<IEnumerable<Message>>(messagesCached, "Messages");


            //working with Redis as main storage at the moment
            INVALIDATE_ONE_CACHE = false;
            INVALIDATE_ALL_CACHE = false;
            ////invalid cache to force update
            //INVALIDATE_ONE_CACHE = true;
            //INVALIDATE_ALL_CACHE = true;

            //return generic result
            return await Task.FromResult<ICommandResult>(new CommandResult(true, _genericSuccessText, message));
        }
    }
}