using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageCommandHandler : Handler,
                                                IRequestHandler<SendMessageCommand, ICommandResult>
    {
        private readonly IRepository<Message> _repository;
        private readonly string _genericErrorText;
        private readonly string _genericSuccessText;

        public MessageCommandHandler(IRepository<Message> repository)
        {
            _repository = repository;
            _genericErrorText = "Ops, parece que os dados da mensagem estão errados!";
            _genericSuccessText = "Mensagem salva com sucesso!";
        }

        public MessageCommandHandler(IRepository<Message> repository, string genericErrorText, string genericSuccessText) : this(repository)
        {
            _genericErrorText = genericErrorText;
            _genericSuccessText = genericSuccessText;
        }

        public async Task<ICommandResult> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            //validando comando preenchido
            if (command == null)
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, null));

            //avalia comando valido
            command.Validate();
            if (command.Invalid)
                return await Task.FromResult<ICommandResult>(new CommandResult(false, _genericErrorText, command.Notifications));

            //cria o objeto
            //var message = new Message("Nova mensagem","Messages-Microservice");
            var message = new Message(command.Body,command.ServiceId);

            // Salva no banco
            await _repository.Create(message);

            //invalida o cache
            INVALIDATE_ONE_CACHE = true;
            INVALIDATE_ALL_CACHE = true;

            // Retorna o resultado
            return await Task.FromResult<ICommandResult>(new CommandResult(true, _genericSuccessText, message));
        }

        //public ICommandResult Handle(UpdateClientCommand command)
        //{
        //    if (command == null)
        //        return new CommandResult(false, _genericErrorText, null);

        //    command.Validate();
        //    if (command.Invalid)
        //        return new CommandResult(false, _genericErrorText, command.Notifications);

        //    // Recupera 
        //    var client = _repository.GetById(command.Id);

        //    // modificacoes
        //    client.UpdateName(command.Name);

        //    // salva no banco
        //    _repository.Update(client);

        //    INVALIDATE_ONE_CACHE = true;
        //    INVALIDATE_ALL_CACHE = true;

        //    // Retorna o resultado
        //    return new CommandResult(true, _genericSuccessText, client);
        //}

        //public ICommandResult Handle(DeleteClientCommand command)
        //{
        //    if (command == null)
        //        return new CommandResult(false, _genericErrorText, null);

        //    command.Validate();
        //    if (command.Invalid)
        //        return new CommandResult(false, _genericErrorText, command.Notifications);

        //    // Recupera 
        //    var client = _repository.GetById(command.Id);

        //    // apaga no banco
        //    _repository.Delete(client);

        //    INVALIDATE_ONE_CACHE = true;
        //    INVALIDATE_ALL_CACHE = true;

        //    // Retorna o resultado
        //    return new CommandResult(true, "Cliente apagado com sucesso!", client);
        //}

        //public ICommandResult Handle(AddProductsClientCommand command)
        //{
        //    if (command == null)
        //        return new CommandResult(false, _genericErrorText, null);

        //    command.Validate();
        //    if (command.Invalid)
        //        return new CommandResult(false, _genericErrorText, command.Notifications);

        //    // Recupera 
        //    var client = _repository.GetById(command.ClientId);

        //    // modificacoes
        //    client.AddProducts(command.Products);

        //    // salva no banco
        //    _repository.Update(client);

        //    INVALIDATE_ONE_CACHE = true;
        //    INVALIDATE_ALL_CACHE = true;

        //    // Retorna o resultado
        //    return new CommandResult(true, "Produtos adicionados com sucesso para este cliente!", client);
        //}

    }
}