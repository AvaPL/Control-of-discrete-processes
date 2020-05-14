using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    [TestFixture]
    public class NEHTests
    {
        // private readonly string[] filePaths = Enumerable.Range(1, 120).Select(i => "ta" + i.ToString("D3")).ToArray();

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
        public void ShouldGiveOptimalMaxQuitTime()
        {
            foreach (string filePath in filePaths)
            {
                using StreamReader fileReader = new StreamReader(filePath);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                FSPTimes fspTimes = NEH.Solve(tasks);
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }
    }
}