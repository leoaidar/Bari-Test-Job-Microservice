using System;
using Bari.Test.Job.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Bari.Test.Job.Tests.QueryTests
{
    [TestClass]
    public class MessageQueryTests
    {
        private FakeMessageRepository messageRepo;
        public MessageQueryTests()
        {
            messageRepo = new FakeMessageRepository();
        }
        
        [TestMethod]
        public void Dada_a_consulta_deve_retornar_os_contatos()
        {
            Assert.IsTrue(messageRepo.GetAll().Result.Count() > 0);
        }
    }
    
}