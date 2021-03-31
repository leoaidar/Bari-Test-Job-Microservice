using System;

namespace Bari.Test.Job.Application.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; private set; }

        public string Body { get; set; }

        public double Timestamp { get; set; }

        public string ServiceId { get; set; }
    }
}
