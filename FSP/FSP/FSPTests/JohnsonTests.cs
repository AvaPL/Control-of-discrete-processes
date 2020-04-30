using System.Collections.Generic;
using System.IO;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    [TestFixture]
    public class JohnsonTests
    {
        private readonly string[] filePaths =
        {
            @"../../../Data/data001.txt",
            @"../../../Data/data003.txt",
            @"../../../Data/data005.txt"
        };

        private readonly int[] expectedResults =
        {
            412,
            591,
            654
        };

        [Test]
        public void ShouldGiveOptimalMaxCompleteTime()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                FSPTimes fspTimes = Johnson.Solve(tasks);
                Assert.AreEqual(expectedResults[i], fspTimes.GetMaxCompleteTime());
            }
        }
    }
}