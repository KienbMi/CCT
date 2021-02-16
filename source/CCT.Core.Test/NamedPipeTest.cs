using Microsoft.VisualStudio.TestTools.UnitTesting;
using NamedPipe;
using System.Threading.Tasks;

namespace CCT.Core.Test
{
    [TestClass]
    public class NamedPipeTest
    {
        [TestMethod]
        public void T01_SendMessageFromClient_Ok()
        {
            // Arrange
            string pipeName = "T01pipe";
            var server = new PipeServer(pipeName: pipeName);
            var client = new PipeClient(pipeName: pipeName);
            string expectedMessage = "Hallo ich bin der Client";

            // Act
            client.SendMessage(expectedMessage);
            string actualMessage = server.ReceiceMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void T02_SendMessageFromClientWithDelay_Ok()
        {
            // Arrange
            string pipeName = "T02pipe";
            var server = new PipeServer(pipeName: pipeName);
            var client = new PipeClient(pipeName: pipeName);
            string expectedMessage = "Hallo ich bin der Client";

            // Act
            client.SendMessage(expectedMessage);
            Task.Delay(1000).Wait();
            string actualMessage = server.ReceiceMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void T03_SendMessageFromClientAndTwoReads_Ok()
        {
            // Arrange
            string pipeName = "T03pipe";
            var server = new PipeServer(pipeName: pipeName);
            var client = new PipeClient(pipeName: pipeName);
            string expectedMessage1 = "Hallo ich bin der Client";
            string expectedMessage2 = "";

            // Act
            client.SendMessage(expectedMessage1);
            string actualMessage1 = server.ReceiceMessage();
            string actualMessage2 = server.ReceiceMessage();

            // Assert
            Assert.AreEqual(expectedMessage1, actualMessage1);
            Assert.AreEqual(expectedMessage2, actualMessage2);
        }

        [TestMethod]
        public void T04_SendMessageFromServer_Ok()
        {
            // Arrange
            string pipeName = "T04pipe";
            var server = new PipeServer(pipeName: pipeName);
            var client = new PipeClient(pipeName: pipeName);
            string expectedMessage = "Hallo ich bin der Server";

            // Act
            server.SendMessage(expectedMessage);
            string actualMessage = client.ReceiceMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void T05_SendMessageFromServerWithDelay_Ok()
        {
            // Arrange
            string pipeName = "T05pipe";
            var server = new PipeServer(pipeName: pipeName);
            var client = new PipeClient(pipeName: pipeName);
            string expectedMessage = "Hallo ich bin der Server";

            // Act
            server.SendMessage(expectedMessage);
            Task.Delay(1000).Wait();
            string actualMessage = client.ReceiceMessage();

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        //[TestMethod]
        //public void T06_SendMessageFromServerAndTwoReads_Ok()
        //{
        //    // Arrange
        //    string pipeName = "T06pipe";
        //    var server = new PipeServer(pipeName: pipeName);
        //    var client = new PipeClient(pipeName: pipeName);
        //    string expectedMessage1 = "Hallo ich bin der Server";
        //    string expectedMessage2 = "";

        //    // Act
        //    server.SendMessage(expectedMessage1);
        //    string actualMessage1 = client.ReceiceMessage();
        //    string actualMessage2 = client.ReceiceMessage();

        //    // Assert
        //    Assert.AreEqual(expectedMessage1, actualMessage1);
        //    Assert.AreEqual(expectedMessage2, actualMessage2);
        //}
    }
}
