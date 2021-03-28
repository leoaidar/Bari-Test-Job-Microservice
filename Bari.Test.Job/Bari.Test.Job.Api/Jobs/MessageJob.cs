using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bari.Test.Job.Api.Jobs
{
    public class MessageJob : IMessageJob
    {
        public void SendMessage()
        {
            Console.WriteLine("Hangfire job");
        }
    }
}
