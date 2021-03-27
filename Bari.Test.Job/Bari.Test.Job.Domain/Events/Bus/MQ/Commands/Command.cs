using Bari.Test.Job.Domain.Events.Bus.MQ.Events;
using System;

namespace Bari.Test.Job.Domain.Events.Bus.MQ.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }
        public Command()
        {
            Timestamp = DateTime.Now;
        }

    }
}
