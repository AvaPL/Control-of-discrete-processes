using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RPQ;

namespace RpqTests
{
    [TestFixture]
    public class CarlierTests
    {
        private static readonly string[] FilePaths =
        {
            @"../../../Data/data50.txt",
            @"../../../Data/data100.txt",
            @"../../../Data/data200.txt"
        };
        
        private static readonly int[] ExpectedResults =
        {
            1492,
            3070,
            6398
        };

        [Test]
        public void ShouldGiveCorrectMaxQuitTimes()
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> unorderedTasks = taskReader.ReadTasksFromFile(fileReader);
                List<Task> orderedTasks = Carlier.Solve(unorderedTasks);
                RPQTimes rpqTimes = RPQTimes.Calculate(orderedTasks);
                Assert.AreEqual(ExpectedResults[i], rpqTimes.GetMaxQuitTime());
            }
        }
    }
}