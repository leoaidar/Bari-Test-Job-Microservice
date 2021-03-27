using Bari.Test.Job.Domain.Events.Bus.Events;
using System;

namespace Bari.Test.Job.Domain.Events
{
    public class MessageCreatedEvent : Event
    {
        public Guid Id { get; private set; }

        public string Body { get; set; }

        public double Timestamp { get; set; }

        public string ServiceId { get; set; }
    }
}
