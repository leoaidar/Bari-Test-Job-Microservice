using System;
using System.Collections.Generic;
using System.Text;

namespace Bari.Test.Job.Domain.Events.Bus.Events
{
    public abstract class Event
    {
        public DateTime Timestamp { get; protected set; }

        //protected Event()
        //{
        //    Timestamp = DateTime.Now;
        //}

        protected Event() => Timestamp = DateTime.Now;
    }
}
