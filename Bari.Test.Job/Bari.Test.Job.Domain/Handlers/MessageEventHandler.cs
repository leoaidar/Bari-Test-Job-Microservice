using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Events;
using Bari.Test.Job.Domain.Events.Bus.MQ;
using Bari.Test.Job.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Bari.Test.Job.Domain.Handlers
{
    public class MessageEventHandler : IEventHandler<MessageCreatedEvent>
    {
        private readonly IRepository<Message> _repository;
        private readonly ILogger<Message> _logger;

        public MessageEventHandler(IRepository<Message> repository, ILogger<Message> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(MessageCreatedEvent @event)
        {
            _repository.Create(new MessageLog
            {
                Body = @event.Body,
                Timestamp = @event.Timestamp,
                ServiceId = @event.ServiceId,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now               
            });

            _logger.LogInformation($"MessageEventHandler: Consumed Message from Job! Time:{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.");

            return Task.CompletedTask;
        }
    }
}
