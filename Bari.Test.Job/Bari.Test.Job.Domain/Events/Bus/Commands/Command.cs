using Bari.Test.Job.Domain.Events.Bus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bari.Test.Job.Domain.Events.Bus.Commands
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
