using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FSP;

namespace FSPTests
{
    [TestFixture]
    public class TaskReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            fileReader = new StreamReader(Filepath);
        }

        [TearDown]
        public void TearDown()
        {
            fileReader.Close();
        }

        private const string Filepath = @"../../../Data/data001.txt";
        private StreamReader fileReader;

        [Test]
        public void ShouldFindDataFile()
        {
            Assert.NotNull(fileReader.ReadLine());
        }

        [Test]
        public void ShouldReadCorrectTaskListLength()
        {
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            Assert.AreEqual(8, tasks.Count);
        }

        [Test]
        public void ShouldReadListedTasksCorrectly()
        {
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            Task expected = new Task(new[] {1, 44});
            Assert.AreEqual(tasks[0].PerformTimes, expected.PerformTimes);
            expected = new Task(new[] {72, 12});
            Assert.AreEqual(expected.PerformTimes, tasks.Last().PerformTimes);
        }
    }
}