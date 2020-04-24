using NUnit.Framework;
using FSP;

namespace FSPTests
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void ShouldParseTask()
        {
            string stringTask = "1 10 2 15 3 20";
            Task expected = new Task(new[] {10, 15, 20});
            Assert.AreEqual(expected.PerformTimes, Task.Parse(stringTask).PerformTimes);
        }
    }
}