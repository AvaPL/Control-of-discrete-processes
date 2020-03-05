using System.IO;
using NUnit.Framework;

namespace RpqTests
{
    [TestFixture]
    public class TaskReaderTests
    {
        private const string Filepath = @"../../../Data/data10.txt";
        private StreamReader _fileReader;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new StreamReader(Filepath);
        }

        [TearDown]
        public void TearDown()
        {
            _fileReader.Close();
        }

        [Test]
        public void ShouldFindDataFile()
        {
            Assert.NotNull(_fileReader.ReadLine());
        }

        [Test]
        public void ShouldReadCorrectTaskListLength()
        {
            Assert.Fail(); //TODO: Implement.
        }

        [Test]
        public void ShouldReadListedTasksCorrectly()
        {
            Assert.Fail(); //TODO: Implement.
                           //TODO(optional): Implement equality members in Task. 
        }
    }
}