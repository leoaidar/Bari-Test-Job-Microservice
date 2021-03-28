using Bari.Test.Job.Domain.Events.Bus.MQ.Commands;
using System;

namespace Bari.Test.Job.Domain.Events
{
    public class MessageSentCommand : Command
    {
        public Guid Id { get; private set; }

        public string Body { get; set; }

        public double Timestamp { get; set; }

        public string ServiceId { get; set; }
    }
}
