using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessagessssEventHandler.cs : Handler,
                                                IRequestHandler<MessageCreatedEvent, IEventResult>,
                                                IEventHandler<MessageCreatedEvent>
    {
        private readonly IRepository<Message> _repository;
        private readonly IRepository<Entity> _entityRepository;
        private readonly string _genericErrorText;
        private readonly string _genericSuccessText;

        public MessagessssEventHandler.cs(IRepository<Message> repository, IRepository<Entity> entityRepository) : base(entityRepository)
        {
            _repository = repository;
            _entityRepository = entityRepository;
            _genericErrorText = "Ops, parece que os dados da mensagem estão errados!";
            _genericSuccessText = "Mensagem salva com sucesso!";
        }

        public MessagessssEventHandler.cs(IRepository<Message> repository, IRepository<Entity> entityRepository, string genericErrorText, string genericSuccessText) : this(repository, entityRepository)
        {
            _genericErrorText = genericErrorText;
            _genericSuccessText = genericSuccessText;
        }

        public async Task<IEventResult> Handle(MessageCreatedEvent @event, CancellationToken cancellationToken)
        {
            //test @event NULL
            if (@event == null)
                return await Task.FromResult<IEventResult>(new EventResult());

            //create object
            var message = new MessageLog
            {
                Body = @event.Body,
                Timestamp = @event.Timestamp,
                ServiceId = @event.ServiceId,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            };

            //test business rules
            message.Validate();
            if (message.Invalid)
                return await Task.FromResult<IEventResult>(new EventResult());

            //save in database
            await _repository.Create(message);

            //invalid cache to force update
            INVALIDATE_ONE_CACHE = true;
            INVALIDATE_ALL_CACHE = true;

            //return generic result
            return await Task.FromResult<IEventResult>(new EventResult());
        }

        public Task Handle(MessageCreatedEvent @event)
        {
            Handle(@event, new CancellationToken());
            return Task.CompletedTask;
        }
    }
}