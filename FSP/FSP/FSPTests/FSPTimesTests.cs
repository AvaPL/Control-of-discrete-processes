using System.Collections.Generic;
using System.IO;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    [TestFixture]
    public class FSPTimesTests
    {
        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTasksFromFile()
        {
            using StreamReader fileReader = new StreamReader( @"../../../Data/data000.txt");
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            FSPTimes rpqTimes = FSPTimes.Calculate(tasks);
            Assert.AreEqual(20, rpqTimes.GetMaxCompleteTime());
        }
    }
}