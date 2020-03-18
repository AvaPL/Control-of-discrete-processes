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
            @"../../Data/data10.txt",
            @"../../Data/data20.txt",
            @"../../Data/data50.txt",
            @"../../Data/data100.txt",
            @"../../Data/data200.txt",
            @"../../Data/data500.txt"
        };

        private static readonly int[] ExpectedResults =
        {
            687,
            1309,
            1514,
            3076,
            6416,
            14786
        };

        [Test]
        public void ShouldGiveTheRightOrderForTwoTasks()
        {
            string filepath = @"../../Data/data2.txt";
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
    }
}