using AutoMapper;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;

namespace Bari.Test.Job.Application.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Message, MessageViewModel>();            
            CreateMap<MessageViewModel, Message>();
            //CreateMap<TestCommandMessage, Contact>();
            //CreateMap<Contact, TestCommandMessage>();
            CreateMap<MessageCreatedEvent, Message>();
            CreateMap<Message, MessageCreatedEvent>();
        }
    }
}
