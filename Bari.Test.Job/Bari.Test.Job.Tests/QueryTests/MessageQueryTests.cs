using Bari.Test.Job.Tests.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bari.Test.Job.Tests.QueryTests
{
    [TestClass]
    public class MessageQueryTests
    {
        private FakeMessageRepository _messageRepository;

        public MessageQueryTests()
        {
            _messageRepository = new FakeMessageRepository();
        }
        
        [TestMethod]
        public void Dada_a_consulta_deve_retornar_os_contatos()
        {
            Assert.IsTrue(_messageRepository.GetAll().Result.Count() > 0);
        }
    }
    
}
