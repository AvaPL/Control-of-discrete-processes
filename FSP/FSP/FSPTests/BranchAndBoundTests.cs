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
        public void ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel1()
        {
            ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel(BranchAndBound.LowerBoundLevel.Level1);
        }

        private void ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel(BranchAndBound.LowerBoundLevel level)
        {
            for (int i = 0; i < filePaths.Length; i++)
                ShouldGiveOptimalMaxCompleteTimeForFile(i, BranchAndBound.UpperBoundLevel.Level0, level);
        }

        private void ShouldGiveOptimalMaxCompleteTimeForFile(int fileIndex,
            BranchAndBound.UpperBoundLevel upperBoundLevel, BranchAndBound.LowerBoundLevel lowerBoundLevel)
        {
            using StreamReader fileReader = new StreamReader(filePaths[fileIndex]);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            FSPTimes fspTimes = BranchAndBound.Solve(tasks, upperBoundLevel, lowerBoundLevel);
            Assert.AreEqual(expectedResults[fileIndex], fspTimes.GetMaxCompleteTime());
        }

        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel2()
        {
            ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel(BranchAndBound.LowerBoundLevel.Level2);
        }

        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel3()
        {
            ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel(BranchAndBound.LowerBoundLevel.Level3);
        }

        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel4()
        {
            ShouldGiveOptimalMaxCompleteTimeForLowerBoundLevel(BranchAndBound.LowerBoundLevel.Level4);
        }
        
        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForUpperBoundLevel1()
        {
            ShouldGiveOptimalMaxCompleteTimeForUpperBoundLevel(BranchAndBound.UpperBoundLevel.Level1);
        }

        private void ShouldGiveOptimalMaxCompleteTimeForUpperBoundLevel(BranchAndBound.UpperBoundLevel level)
        {
            for (int i = 0; i < filePaths.Length; i++)
                ShouldGiveOptimalMaxCompleteTimeForFile(i, level, BranchAndBound.LowerBoundLevel.Level3);
        }

        [Test]
        public void ShouldGiveOptimalMaxCompleteTimeForUpperBoundLevel2()
        {
            ShouldGiveOptimalMaxCompleteTimeForUpperBoundLevel(BranchAndBound.UpperBoundLevel.Level2);
        }
    }
}