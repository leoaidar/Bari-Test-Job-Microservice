using System;

namespace Bari.Test.Job.Domain.Events.Bus.MQ.Events
{
    public abstract class Event
    {
        public DateTime Timestamp { get; protected set; }

        protected Event() => Timestamp = DateTime.Now;
    }
}
