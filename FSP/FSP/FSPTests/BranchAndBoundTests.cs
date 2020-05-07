using System;
using System.Collections.Generic;
using System.IO;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    [TestFixture]
    public class BranchAndBoundTests
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
        public void ShouldGiveOptimalMaxCompleteTimeForLevel1()
        {
            ShouldGiveOptimalMaxCompleteTimeForLevel(BranchAndBound.LowerBoundLevel.Level1);
        }

        private void ShouldGiveOptimalMaxCompleteTimeForLevel(BranchAndBound.LowerBoundLevel lowerBoundLevel)
        {
            for (int i = 0; i < filePaths.Length; i++)
                ShouldGiveOptimalMaxCompleteTimeForFile(i, lowerBoundLevel);
        }

        private void ShouldGiveOptimalMaxCompleteTimeForFile(int fileIndex, BranchAndBound.LowerBoundLevel level)
        {
            using StreamReader fileReader = new StreamReader(filePaths[fileIndex]);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            FSPTimes fspTimes = BranchAndBound.Solve(tasks, level);
            Assert.AreEqual(expectedResults[fileIndex], fspTimes.GetMaxCompleteTime());
        }
        
        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLevel2()
        {
            ShouldGiveOptimalMaxCompleteTimeForLevel(BranchAndBound.LowerBoundLevel.Level2);
        }
        
        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLevel3()
        {
            ShouldGiveOptimalMaxCompleteTimeForLevel(BranchAndBound.LowerBoundLevel.Level3);
        }
        
        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLevel4()
        {
            ShouldGiveOptimalMaxCompleteTimeForLevel(BranchAndBound.LowerBoundLevel.Level4);
        }
    }
}