using AutoMapper;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bari.Test.Job.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMediator _mediator;
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

            return collection;
        }
        
        public async Task<CommandResult> SendMessage(SendMessageCommand command, CancellationToken cancellationToken)
        {

            var handler = (CommandResult)await (_mediator.Send(command, cancellationToken));

            var messageEvent = _mapper.Map<MessageCreatedEvent>(handler.Data);

            var viewModel = _mapper.Map<MessageViewModel>(handler.Data);

            handler.Data = viewModel;

            _bus.Publish(messageEvent);

            return handler;
        }
    }

}
