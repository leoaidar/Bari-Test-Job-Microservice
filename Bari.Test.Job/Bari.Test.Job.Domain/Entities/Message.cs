using System;
using System.Collections.Generic;
using System.Text;

namespace Bari.Test.Job.Domain.Entities
{
    public class Message : Entity
    {
        public string Body { get; set; }

        public double Timestamp { get { return (double)((TimeSpan)(CreateDate - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime())).TotalSeconds; } }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Body?.Trim()))
                Invalid = true;
        }

        
    }
}
