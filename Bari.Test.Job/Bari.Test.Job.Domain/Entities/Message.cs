using System;

namespace Bari.Test.Job.Domain.Entities
{
    public class Message : Entity
    {
        public Message()
        {

        }
        public Message(string body, string serviceId) : base()
        {
            Body = body;
            ServiceId = serviceId;
            LastUpdateDate = DateTime.Now;
            TimestampCalc();
        }

        public string Body { get; set; }

        public double Timestamp { get; set; }

        public string ServiceId { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Body?.Trim()))
                Invalid = true;
        }

        public void TimestampCalc()
        {
            Timestamp = (double)((TimeSpan)(CreateDate - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime())).TotalSeconds;
        }

        
    }
}
