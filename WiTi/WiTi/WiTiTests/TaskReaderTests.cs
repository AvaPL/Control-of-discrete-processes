using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WiTi;

namespace WiTiTests
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

        private const string Filepath = @"../../../Data/data10.txt";
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
            Task task = new Task(1, 9, 144);
            Assert.AreEqual(tasks[0], task);
            task = new Task(58, 9, 455);
            Assert.AreEqual(task, tasks.Last());
        }
    }
}