using System.Collections.Generic;
using System.IO;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    [TestFixture]
    public class BruteForceTests
    {
        private string[] filePaths =
        {
            // @"../../../Data/data001.txt",
            // @"../../../Data/data002.txt",
            // @"../../../Data/data003.txt",
            // @"../../../Data/data004.txt",
            @"../../../Data/data005.txt",
            // @"../../../Data/data006.txt"
        };

        private int[] expectedResults =
        {
            412,
            650,
            591,
            727,
            654,
            780
        };

        [Test]
        public void ShouldGiveOptimalMaxCompleteTime()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                FSPTimes fspTimes = BruteForce.SolveUsingPermutations(tasks);
                Assert.AreEqual(expectedResults[i], fspTimes.GetMaxCompleteTime());
            }
        }

        // [Test]
        // public void ShouldGiveOptimalTotalWeightedTardinessFor11Tasks()
        // {
        //     using StreamReader fileReader = new StreamReader(@"../../../Data/data11.txt");
        //     TaskReader taskReader = new TaskReader();
        //     List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
        //     WiTiTimes wiTiTimes = BruteForce.SolveUsingPermutations(tasks);
        //     Assert.AreEqual(962, wiTiTimes.TotalWeightedTardiness);
        // }
        //
        // [Test]
        // public void ShouldGiveOptimalTotalWeightedTardinessFor10TasksUsingRecursion()
        // {
        //     using StreamReader fileReader = new StreamReader(@"../../../Data/data10.txt");
        //     TaskReader taskReader = new TaskReader();
        //     List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
        //     WiTiTimes wiTiTimes = BruteForce.SolveUsingRecursion(tasks);
        //     Assert.AreEqual(1004, wiTiTimes.TotalWeightedTardiness);
        // }
        //
        // [Test]
        // public void ShouldGiveOptimalTotalWeightedTardinessFor11TasksUsingRecursion()
        // {
        //     using StreamReader fileReader = new StreamReader(@"../../../Data/data11.txt");
        //     TaskReader taskReader = new TaskReader();
        //     List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
        //     WiTiTimes wiTiTimes = BruteForce.SolveUsingRecursion(tasks);
        //     Assert.AreEqual(962, wiTiTimes.TotalWeightedTardiness);
        // }
    }
}