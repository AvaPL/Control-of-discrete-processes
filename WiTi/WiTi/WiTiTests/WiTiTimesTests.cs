using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using WiTi;

namespace WiTiTests
{
    [TestFixture]
    public class WiTiTimesTests
    {
        private static readonly string[] FilePaths =
        {
            @"../../../Data/data10.txt",
            @"../../../Data/data11.txt",
            @"../../../Data/data12.txt",
            @"../../../Data/data13.txt",
            @"../../../Data/data14.txt",
            @"../../../Data/data15.txt",
            @"../../../Data/data16.txt",
            @"../../../Data/data17.txt",
            @"../../../Data/data18.txt",
            @"../../../Data/data19.txt",
            @"../../../Data/data20.txt"
        };

        private static readonly int[] ExpectedUnsortedResults =
        {
            4147,
            3978,
            4010,
            3574,
            3736,
            3955,
            5862,
            6083,
            6362,
            7039,
            8891
        };

        private static readonly int[] ExpectedSortedResults =
        {
            2055,
            1463,
            1212,
            681,
            646,
            310,
            321,
            914,
            574,
            747,
            526
        };

        [Test]
        public void ShouldGiveCorrectTotalWeightedTardinessForSingleTask()
        {
            List<Task> tasks = new List<Task>
            {
                new Task(10, 10, 5)
            };
            WiTiTimes wiTiTimes = new WiTiTimes(tasks);
            Assert.AreEqual(50, wiTiTimes.TotalWeightedTardiness);
        }

        [Test]
        public void ShouldGiveCorrectTotalWeightedTardinessForTasksFromFile()
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                WiTiTimes wiTiTimes = new WiTiTimes(tasks);
                Assert.AreEqual(ExpectedUnsortedResults[i], wiTiTimes.TotalWeightedTardiness);
            }
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTasksSortedByDeadline()
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                tasks.Sort();
                WiTiTimes wiTiTimes = new WiTiTimes(tasks);
                Assert.AreEqual(ExpectedSortedResults[i], wiTiTimes.TotalWeightedTardiness);
            }
        }

        [Test]
        public void ShouldGiveCorrectTotalWeightedTardinessForTwoTasks()
        {
            /*
            Task[0]: 1 5 1
            Task[1]: 9 4 3

            StartTimes:         0 1
            CompleteTimes:      1 10
            WeightedTardiness:  0 28
            */
            List<Task> tasks = new List<Task>
            {
                new Task(1, 5, 1),
                new Task(9, 4, 3)
            };
            WiTiTimes wiTiTimes = new WiTiTimes(tasks);
            Assert.AreEqual(28, wiTiTimes.TotalWeightedTardiness);
        }
    }
}