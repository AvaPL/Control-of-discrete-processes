using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RPQ;

namespace RpqTests
{
    [TestFixture]
    public class CarlierTests
    {
        private static void ShouldGiveCorrectMaxQuitTime(string filePath, int expectedResult)
        {
            using StreamReader fileReader = new StreamReader(filePath);
            TaskReader taskReader = new TaskReader();
            List<Task> unorderedTasks = taskReader.ReadTasksFromFile(fileReader);
            List<Task> orderedTasks = Carlier.Solve(unorderedTasks);
            RPQTimes rpqTimes = RPQTimes.Calculate(orderedTasks);
            Assert.AreEqual(expectedResult, rpqTimes.GetMaxQuitTime());
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor100Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data100.txt", 3070);
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor10Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data10.txt", 641);
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor200Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data200.txt", 6398);
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor20Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data20.txt", 1267);
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor500Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data500.txt", 14785);
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeFor50Tasks()
        {
            ShouldGiveCorrectMaxQuitTime(@"../../../Data/data50.txt", 1492);
        }
    }
}