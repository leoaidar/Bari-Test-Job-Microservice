using Bari.Test.Job.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bari.Test.Job.Tests.EntitiesTests
{
    [TestClass]
    public class MessageTest
    {
        private Message _validMessage;

        public MessageTest()
        {
            _validMessage = new Message { Body = "Hello World!" };
        }

        [TestMethod]
        public void Test_Message_should_have_a_creation_date()
        {
            var message = new Message { Body = "Hello World!" };
            message.Validate();
            Assert.IsFalse(message.Invalid);
            Assert.AreEqual(message.CreateDate == null, false);
            Assert.IsTrue(message.CreateDate.HasValue);
        }

        [TestMethod]
        public void Test_Message_should_have_same_value_when_create_date_creation_and_last_update_date()
        {
            var message = new Message { Body = "Hello World!" };
            message.Validate();
            Assert.IsFalse(message.Invalid);
            Assert.AreEqual(message.CreateDate, message.LastUpdateDate);
        }

        [TestMethod]
        public void Test_Message_should_have_a_valid_timestamp_value()
        {
            var message = new Message { Body = "Hello World!" };

            TimeSpan epoch = ((TimeSpan)(message.CreateDate - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()));
            //return the total seconds (which is a UNIX timestamp)
            var timestamp = (double)epoch.TotalSeconds;

            message.Validate();
            Assert.IsFalse(message.Invalid);
            Assert.AreEqual(message.Timestamp, timestamp);
        }

        [TestMethod]
        public void Test_Message_must_be_invalid_when_body_is_empty()
        {
            var message = new Message { Body = string.Empty };
            message.Validate();
            Assert.IsTrue(message.Body == string.Empty);
            Assert.IsTrue(message.Invalid);
        }

        [TestMethod]
        public void Test_Message_must_be_invalid_when_text_body_is_empty()
        {
            var message = new Message { Body = "" };
            message.Validate();
            Assert.IsTrue(message.Body.Equals(""));
            Assert.IsTrue(message.Invalid);
        }
        
        [TestMethod]
        public void Test_Message_must_be_invalid_when_text_body_is_only_space()
        {
            var message = new Message { Body = " " };
            message.Validate();
            Assert.IsTrue(message.Body.Equals(" "));
            Assert.IsTrue(message.Invalid);
        }

        [TestMethod]
        public void Test_Message_must_be_invalid_when_body_is_null()
        {
            var message = new Message { Body = null };
            message.Validate();
            Assert.IsTrue(message.Body == null);
            Assert.IsTrue(message.Invalid);
        }

        [TestMethod]
        public void Test_Message_must_be_valid_when_body_has_unless_one_char()
        {
            var message = new Message { Body = "a" };
            message.Validate();
            Assert.IsTrue(message.Body.Equals("a"));
            Assert.IsFalse(message.Invalid);
        }
    }
}
