using AutoMapper;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Domain.Queries;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Message> _messageRepository;
        private readonly IEventBus _bus;
        private readonly IMapper _mapper;


        public MessageService(IEventBus bus, IMediator mediator, IMapper mapper)
        {            
            _bus = bus;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var handler = (QueryResult<IEnumerable<Message>>)await (_mediator.Send(new MessageGetAllQuery(), cancellationToken));

            var collection = _mapper.Map<IEnumerable<MessageViewModel>>(handler.Entity);

            var messageEvent = _mapper.Map<MessageCreatedEvent>(handler.Entity.First());

             //////_bus.Publish(messageEvent);

            return collection;
        }
        
        public async Task<CommandResult> SendMessage(SendMessageCommand command, CancellationToken cancellationToken)
        {

            var handler = (CommandResult)await (_mediator.Send(command, cancellationToken));

            var messageCommand = _mapper.Map<MessageSentCommand>(handler.Data);

            var viewModel = _mapper.Map<MessageViewModel>(handler.Data);

            handler.Data = viewModel;

            //////await _bus.SendCommand(messageCommand);

            return handler;
        }
    }

}
