using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Commands.Contracts;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using System.Globalization;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageEventHandler : IEventHandler<MessageCreatedEvent>
    {
        private readonly IRepository<Message> _repository;

        public MessageEventHandler(IRepository<Message> repository)
        {
            _repository = repository;
        }

        public Task Handle(MessageCreatedEvent @event)
        {
            _repository.Create(new MessageLog
            {
                Body = @event.Body,
                Timestamp = @event.Timestamp,
                ServiceId = @event.ServiceId,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
               
            });
            Console.WriteLine($"MessageEventHandler: Get Message from Job! Time:{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture)}.");
            return Task.CompletedTask;
        }
    }
}
