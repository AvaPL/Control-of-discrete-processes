using NUnit.Framework;
using WiTi;

namespace WiTiTests
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void ShouldParseTask()
        {
            string stringTask = "10 11 12";
            Task task = new Task(10, 11, 12);
            Assert.AreEqual(task, Task.Parse(stringTask));
        }
    }
}