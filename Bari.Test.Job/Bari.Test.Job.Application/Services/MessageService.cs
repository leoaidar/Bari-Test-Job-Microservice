using AutoMapper;
using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus;
using Bari.Test.Job.Domain.Queries;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<MessageViewModel>> GetAll()
        {
            var query = (GenericQueryResult<IEnumerable<Message>>) await (_mediator.Send(new MessageGetAllQuery()));
            
            //var collection = new List<MessageViewModel>();

            var c = _mapper.Map<MessageViewModel>(query.Entity.First());

            var collection = _mapper.Map<IEnumerable<MessageViewModel>>(query.Entity);

            //var messageCommand = _mapper.Map<TestCommandMessage>(query.Entity.First());

            var messageEvent = _mapper.Map<MessageCreatedEvent>(query.Entity.First());

            //await _bus.SendCommand(messageCommand);

             _bus.Publish(messageEvent);

            //funcionando
            //query.Entity.ToList().ForEach(message => {
            //    collection.Add(_mapper.Map<MessageViewModel>(message));
            //});

            //query.Entity.All(e => {
            //    collection.Add(_mapper.Map<MessageViewModel>(e));
            //    return true;
            //});

            return collection;
        }



        //public void Transfer(MessageViewModel accountTransfer)
        //{
        //    var createTransferCommand = new CreateTransferCommand(
        //            accountTransfer.FromAccount,
        //            accountTransfer.ToAccount,
        //            accountTransfer.TransferAmount
        //        );

        //    _bus.SendCommand(createTransferCommand);
        //}
    }

    //short helper class to ignore some properties from serialization
    //public class IgnorePropertiesResolver : DefaultContractResolver
    //{
    //    private readonly HashSet<string> ignoreProps;
    //    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
    //    {
    //        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
    //    }

    //    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //    {
    //        JsonProperty property = base.CreateProperty(member, memberSerialization);
    //        if (this.ignoreProps.Contains(property.PropertyName))
    //        {
    //            property.ShouldSerialize = _ => false;
    //        }
    //        return property;
    //    }
    //}

    ////Usage
    //JsonConvert.SerializeObject(YourObject, new JsonSerializerSettings()
    //{ ContractResolver = new IgnorePropertiesResolver(new[] { "Prop1", "Prop2" }) };);
}
