using Bari.Test.Job.Domain.Commands;
using Bari.Test.Job.Domain.Entities;
using Bari.Test.Job.Domain.Handlers;
using Bari.Test.Job.Domain.Repositories;
using Bari.Test.Job.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bari.Test.Job.Tests.HandlerTests
{
    [TestClass]
    public class SendMessageHandlerTests
    {
        private readonly SendMessageCommand _validCommand;
        private readonly SendMessageCommand _invalidCommand;
        private MessageCommandHandler _handler;

        private CommandResult _result;

        public SendMessageHandlerTests()
        {
            _validCommand = new SendMessageCommand("Leonardo", "SendMessageHandlerTests"); ;
            _invalidCommand = new SendMessageCommand("", "");
            IRepository<Entity> repo = new EntityFakeRepository();
            var repositories = new List<IRepository<Message>>
            {
                new FakeMessageRepository(),
                new FakeMessageRepository()
            };
            _handler = new MessageCommandHandler(repositories, repo);
            _result = new CommandResult();
        }


        [TestMethod]
        public void Dado_um_comando_invalido_deve_interromper_a_execucao()
        {
            var cmd = _handler.Handle(_invalidCommand, new System.Threading.CancellationToken());
            _result = (CommandResult)cmd.Result;
            Assert.AreEqual(_result.Success, false);
        }
        
        [TestMethod]
        public void Dado_um_comando_valido_deve_criar_a_mensagem()
        {
            var cmd = _handler.Handle(_validCommand, new System.Threading.CancellationToken());
            _result = (CommandResult)cmd.Result;
            Assert.AreEqual(_result.Success, true);
        }

    }
}