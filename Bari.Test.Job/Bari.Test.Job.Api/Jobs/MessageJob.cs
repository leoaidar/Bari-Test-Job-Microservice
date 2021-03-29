using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Bari.Test.Job.Api.Jobs
{
    public class MessageJob : IMessageJob
    {
        private readonly IMessageService _messageService;
        private readonly IConfiguration _config;
        private readonly IEventBus _bus;

        public MessageJob(IMessageService messageService, IEventBus bus, IConfiguration config)
        {
            _messageService = messageService;
            _bus = bus;
            _config = config;
        }


        public async Task SendMessage()
        {
            var serviceId = _config.GetValue<string>("MicroserviceId");
            var messageEvent = new MessageCreatedEvent
            {
                Body = "Hello World!",
                ServiceId = serviceId,
                Id = Guid.NewGuid(),
                Timestamp = (double)((TimeSpan)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime())).TotalSeconds
            };
            // publish event ot RabbitMQ
            _bus.Publish(messageEvent);
            Console.WriteLine($"MessageJob: Publish Message from Job! Time:{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.");
        }

        //public async Task SendMessage()
        //{
        //    try
        //    {
        //        Console.WriteLine("Hangfire job from class");
        //        var serviceId = _config.GetValue<string>("MicroserviceId");
        //        var command = new SendMessageCommand { Body = "Hello World! From Job", ServiceId = serviceId };
        //        var job = await _messageService.SendMessage(command, new System.Threading.CancellationToken());
        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //}



    }
}
