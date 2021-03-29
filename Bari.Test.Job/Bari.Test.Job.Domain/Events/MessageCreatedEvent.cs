using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using MediatR;
using System;

namespace Bari.Test.Job.Domain.Events
{
    public class MessageCreatedEvent : Event, IRequest<IEventResult>
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public double Timestamp { get; set; }

        public string ServiceId { get; set; }
    }
}
