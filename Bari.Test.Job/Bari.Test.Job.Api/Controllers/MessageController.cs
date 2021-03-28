using Bari.Test.Job.Application.Interfaces;
using Bari.Test.Job.Application.ViewModels;
using Bari.Test.Job.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace Bari.Test.Job.Controllers
{
    [ApiController]
    [Route("v1/message")]
    public class MessageController : ControllerBase
    {        
        private readonly IMediator _mediator;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMediator mediator, ILogger<MessageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<MessageViewModel>))]
        public async Task<IActionResult> Get([FromServices] IMessageService service)
        {
            var query = await service.GetAll(new System.Threading.CancellationToken());

            if (query == null)
                return NotFound();

            return Ok(query);
        }

        [HttpPost]
        [ResponseType(typeof(CommandResult))]
        public IActionResult Create(
            [FromBody] SendMessageCommand command,
            [FromServices] IMessageService service
        )
        {
            var cmd = service.SendMessage(command, new System.Threading.CancellationToken());

            if (cmd == null)
                return NotFound();

            return Ok(cmd.Result);
        }



    }
}