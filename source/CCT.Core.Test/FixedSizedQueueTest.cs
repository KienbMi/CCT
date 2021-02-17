using FixedSizedQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.Core.Test
{
    [TestClass]
    public class FixedSizedQueueTest
    {
        [TestMethod]
        public void T01_EnqueFourItems_GetFourItems_Ok()
        {
            // Arrange
            int queueSize = 100;
            FixedSizedQueue<string> queue = new FixedSizedQueue<string>(queueSize);
            int expected = 4;

            // Act
            queue.Enqueue("one");
            queue.Enqueue("two");
            queue.Enqueue("three");
            queue.Enqueue("four");

            // Assert
            Assert.AreEqual(expected, queue.Count);
        }

        [TestMethod]
        public void T02_EnqueFourItemsWithLimit_GetThreeItems_Ok()
        {
            // Arrange
            int queueSize = 3;
            FixedSizedQueue<string> queue = new FixedSizedQueue<string>(queueSize);
            int expected = 3;

            // Act
            queue.Enqueue("one");
            queue.Enqueue("two");
            queue.Enqueue("three");
            queue.Enqueue("four");

            // Assert
            Assert.AreEqual(expected, queue.Count);
        }
    }
}
