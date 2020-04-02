using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WiTi;

namespace WiTiTests
{
    [TestFixture]
    public class BruteForceTests
    {
        [Test]
        public void ShouldGiveOptimalTotalWeightedTardinessFor10Tasks()
        {
            using StreamReader fileReader = new StreamReader(@"../../../Data/data10.txt");
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            WiTiTimes wiTiTimes = BruteForce.SolveUsingPermutations(tasks);
            Assert.AreEqual(1004, wiTiTimes.TotalWeightedTardiness);
        }

        [Test]
        public void ShouldGiveOptimalTotalWeightedTardinessFor11Tasks()
        {
            using StreamReader fileReader = new StreamReader(@"../../../Data/data11.txt");
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            WiTiTimes wiTiTimes = BruteForce.SolveUsingPermutations(tasks);
            Assert.AreEqual(962, wiTiTimes.TotalWeightedTardiness);
        }
    }
}