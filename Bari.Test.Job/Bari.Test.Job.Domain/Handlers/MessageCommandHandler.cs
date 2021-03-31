using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageCommandHandler : Handler,
                                                IRequestHandler<SendMessageCommand, ICommandResult>
    {
        private readonly IRepository<Message> _repository;
        private readonly IRepository<Message> _cacheRepository;
        private readonly string _genericErrorText;
        private readonly string _genericSuccessText;

        public MessageCommandHandler(IEnumerable<IRepository<Message>> repositories, IRepository<Entity> entityRepository) : base(entityRepository)
        {
            _repository = repositories.First();
            _cacheRepository = repositories.ElementAt(1);
            _genericErrorText = "Ops, parece que os dados da mensagem estão errados!";
            _genericSuccessText = "Mensagem salva com sucesso!";
        }

        public MessageCommandHandler(IEnumerable<IRepository<Message>> repositories, IRepository<Entity> entityRepository, string genericErrorText, string genericSuccessText) : this(repositories, entityRepository)
        {
            _genericErrorText = genericErrorText;
            _genericSuccessText = genericSuccessText;
        }

        public async Task<ICommandResult> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command == null)
                    return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, null));

                command.Validate();
                if (command.Invalid)
                    return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, command.Notifications));

                var message = new Message(command.Body, command.ServiceId);

                message.Validate();
                if (message.Invalid)
                    return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, "Regra de negócio inválida"));

                await _repository.Create(message);
                await _cacheRepository.Create(message);

                List<Message> messagesCached = (List<Message>)await _cacheRepository.GetAll();

                messagesCached = InitializingCollection<Message>(message, messagesCached);

                await _cacheRepository.Bind<IEnumerable<Message>>(messagesCached, "Messages");

                INVALIDATE_ONE_CACHE = false;
                INVALIDATE_ALL_CACHE = false;

                return await Task.FromResult<ICommandResult>(new CommandResult(true, _genericSuccessText, message));
            }
            catch (Exception ex)
            {
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText + "|" + ex.Message + "|" + ex.StackTrace, null));
            }
        }
    }
}