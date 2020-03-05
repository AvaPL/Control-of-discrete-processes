using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RPQ;

namespace RpqTests
{
    [TestFixture]
    public class TaskReaderTests
    {
        private const string Filepath = @"../../../Data/data10.txt"; // Change every time 
        private StreamReader fileReader;

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
            Assert.AreEqual(tasks.Count, 10);
        }

        [Test]
        public void ShouldReadListedTasksCorrectly()
        {
            Assert.Fail(); //TODO: Implement.
                           //TODO(optional): Implement equality members in Task. 
        }
    }
}