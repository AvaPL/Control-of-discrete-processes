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
        private readonly string[] filePaths =
        {
            @"../../../Data/data001.txt",
            @"../../../Data/data002.txt",
            @"../../../Data/data003.txt",
            @"../../../Data/data004.txt",
            @"../../../Data/data005.txt",
            @"../../../Data/data006.txt"
        };

        private readonly int[] expectedResults =
        {
            412,
            650,
            591,
            727,
            654,
            780
        };

        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeUsingPermutations()
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
    }
}