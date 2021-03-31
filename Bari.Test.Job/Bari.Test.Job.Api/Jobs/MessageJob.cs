using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Domain.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Bari.Test.Job.Api.Jobs
{
    public class MessageJob : IMessageJob
    {
        private readonly IMessageService _messageService;
        private readonly IConfiguration _config;
        private readonly ILogger<MessageJob> _logger;

        public MessageJob(IMessageService messageService, IConfiguration config, ILogger<MessageJob> logger)
        {
            _messageService = messageService;
            _config = config;
            _logger = logger;
        }

        public async Task SendMessage()
        {
            try
            {
                var serviceId = _config.GetValue<string>("MicroserviceId");
                var command = new SendMessageCommand { Body = "Hello World!", ServiceId = serviceId };
                var job = await _messageService.SendMessage(command, new System.Threading.CancellationToken());

                _logger.LogInformation($"MessageJob: Publish Message from Job! Time:{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }

        }
    }
}
