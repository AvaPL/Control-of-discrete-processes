using System;
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
            @"../../../Data/data002.txt",
            // @"../../../Data/data003.txt",
            // @"../../../Data/data004.txt",
            // @"../../../Data/data005.txt",
            // @"../../../Data/data006.txt"
        };

        private int[] expectedResults =
        {
            // 412,
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


        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeUsingRecursion()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                FSPTimes fspTimes = BruteForce.SolveUsingRecursion(tasks);
                Assert.AreEqual(expectedResults[i], fspTimes.GetMaxCompleteTime());
            }
        }

        [Test]
        public void ShouldGiveEqualPermutations()
        {
            using StreamReader fileReader = new StreamReader(@"../../../Data/data002.txt");
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            FSPTimes fspTimesFromPermutations = BruteForce.SolveUsingPermutations(tasks);
            FSPTimes fspTimesFromRecursion = BruteForce.SolveUsingRecursion(tasks);
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine("Permutations: " + string.Join(", ", fspTimesFromPermutations.Permutation[i].PerformTimes));
                Console.WriteLine("Recursion: " + string.Join(", ", fspTimesFromRecursion.Permutation[i].PerformTimes));
                Console.WriteLine("--------------");
            }
        }
    }
}