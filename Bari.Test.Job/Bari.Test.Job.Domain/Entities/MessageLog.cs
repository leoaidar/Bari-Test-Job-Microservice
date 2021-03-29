using System;
using System.Collections.Generic;
using System.Text;

namespace Bari.Test.Job.Domain.Entities
{
    public class MessageLog : Message 
    {       
        public double Timestamp { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
