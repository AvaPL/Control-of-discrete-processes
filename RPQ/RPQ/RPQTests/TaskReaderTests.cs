using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using RPQ;

namespace RpqTests
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

        private const string Filepath = @"../../Data/data10.txt";
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
            Assert.AreEqual(10, tasks.Count);
        }

        [Test]
        public void ShouldReadListedTasksCorrectly()
        {
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            Task task = new Task(219, 5, 276);
            Assert.AreEqual(tasks[0], task);
            task = new Task(79, 60, 235);
            Assert.AreEqual(task, tasks.Last());
        }
    }
}