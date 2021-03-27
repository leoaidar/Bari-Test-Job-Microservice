using System;
using System.Collections.Generic;
using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Web.Http.Description;
using System.Threading.Tasks;
using Bari.Test.Job.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Bari.Test.Job.Application.ViewModels;
using MediatR;
using Bari.Test.Job.Domain.Queries;

namespace Bari.Test.Job.Controllers
{
    [ApiController]
    [Route("v1/messages")]
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
        public async Task<IActionResult> Get([FromServices] MessageQueryHandler handler)
        {

            var query = await handler.Handle(new MessageGetAllQuery(), new System.Threading.CancellationToken());

            if (query == null)
                return NotFound();
                //return await Task.FromResult<IActionResult>(NotFound());

            return Ok(query);
            //return await Task.FromResult<IActionResult>(Ok(query));
        }

        [HttpPost]
        public CommandResult Create(
            [FromBody] SendMessageCommand command,
            [FromServices] MessageCommandHandler handler
        )
        {
            return (CommandResult) handler.Handle(command, new System.Threading.CancellationToken());
        }


    }
}