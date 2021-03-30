using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bari.Test.Job.Domain.Commands;
using System;

namespace Bari.Test.Job.Tests.Commands
{
    [TestClass]
    public class SendMessageCommandTests
    {
        private readonly SendMessageCommand _validCommand;
        private readonly SendMessageCommand _invalidCommand;

        public SendMessageCommandTests()
        {
            _validCommand = new SendMessageCommand("Leonardo", "SendMessageCommandTests");
            _invalidCommand = new SendMessageCommand("", "");
        }

        [TestMethod]
        public void Dado_um_comando_invalido()
        {
            _invalidCommand.Validate();
            Assert.AreEqual(_invalidCommand.Valid, false);
        }
        
        [TestMethod]
        public void Dado_um_comando_valido()
        {
            _validCommand.Validate();
            Assert.AreEqual(_validCommand.Valid, true);
        }        
   
    }
}
