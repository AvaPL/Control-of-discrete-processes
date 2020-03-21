using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RPQ;

namespace RpqTests
{
    [TestFixture]
    public class SchrageTests
    {
        private static readonly string[] FilePaths =
        {
            @"../../../Data/data10.txt",
            @"../../../Data/data20.txt",
            @"../../../Data/data50.txt",
            @"../../../Data/data100.txt",
            @"../../../Data/data200.txt",
            @"../../../Data/data500.txt"
        };
        
        private static readonly string[] FilePathsWithInterrupts =
        {
            @"../../../Data/data50.txt",
            @"../../../Data/data100.txt",
            @"../../../Data/data200.txt"
        };

        private static readonly int[] ExpectedResults =
        {
            687,
            1299,
            1513,
            3076,
            6416,
            14822
        };
        
        private static readonly int[] ExpectedResultsWithInterrupts =
        {
            1492,
            3070,
            6398
        };

        [Test]
        public void ShouldGiveTheRightOrderForTwoTasks()
        {
            string filepath = @"../../../Data/data2.txt";
            using StreamReader fileReader = new StreamReader(filepath);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            List<Task> expectedTasks = new List<Task>() {new Task(84, 13, 103), new Task(219, 5, 276)};
            Assert.AreEqual(expectedTasks, Schrage.Solve(tasks));
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTime()
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> unorderedTasks = taskReader.ReadTasksFromFile(fileReader);
                List<Task> orderedTasks = Schrage.Solve(unorderedTasks);
                RPQTimes rpqTimes = RPQTimes.Calculate(orderedTasks);
                Assert.AreEqual(ExpectedResults[i], rpqTimes.GetMaxQuitTime());
            }
        }
        
        [Test]
        public void ShouldGiveCorrectMaxQuitTimeWithInterrupts()
        {
            for (int i = 0; i < FilePathsWithInterrupts.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(FilePathsWithInterrupts[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> unorderedTasks = taskReader.ReadTasksFromFile(fileReader);
                int result = InterruptedSchrage.Solve(unorderedTasks);
                Assert.AreEqual(ExpectedResultsWithInterrupts[i], result);
            }
        }
    }
}