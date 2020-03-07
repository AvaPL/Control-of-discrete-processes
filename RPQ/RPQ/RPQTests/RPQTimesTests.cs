﻿using NUnit.Framework;
using RPQ;
using System.Collections.Generic;
using System.IO;

namespace RpqTests
{
    [TestFixture]
    public class RPQTimesTests
    {
        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForSingleTask()
        {
            List<Task> tasks = new List<Task>
            {
                new Task (10,10,10)
            };
            RPQTimes rpqTimes =  RPQTimes.Calculate(tasks);
            Assert.AreEqual(30, rpqTimes.GetMaxQuitTime());
        }
        
        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTwoTasks()
        {
            /*
            Task[0]: 1 5 1
            Task[1]: 9 4 3

            StartTimes:    1 9
            CompleteTimes: 6 13
            QuitTimes:     7 16
            */
            List<Task> tasks = new List<Task>
            {
                new Task (1,5,1),
                new Task (9,4,3)
            };
            RPQTimes rpqTimes = RPQTimes.Calculate(tasks);
            Assert.AreEqual(16, rpqTimes.GetMaxQuitTime());
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTwoTasks2()
        {
            /*
            Task[0]: 1 5 11
            Task[1]: 1 4 3

            StartTimes:    1  6
            CompleteTimes: 6  10
            QuitTimes:     17 13
            */
            List<Task> tasks = new List<Task>
            {
                new Task (1,5,11),
                new Task (1,4,3)
            };
            RPQTimes rpqTimes = RPQTimes.Calculate(tasks);
            Assert.AreEqual(17, rpqTimes.GetMaxQuitTime());
        }

        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTasksFromFile()
        {
            string[] filePaths = 
            {
                @"../../Data/data10.txt",
                @"../../Data/data20.txt",
                @"../../Data/data50.txt",
                @"../../Data/data100.txt",
                @"../../Data/data200.txt",
                @"../../Data/data500.txt"
            };
            int[] expectedResults = 
            {
                927,
                1905,
                2843,
                5324,
                11109,
                26706
            };

            for (int i = 0; i < filePaths.Length; i++)
            {
                StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                RPQTimes rpqTimes = RPQTimes.Calculate(tasks);
                Assert.AreEqual(expectedResults[i], rpqTimes.GetMaxQuitTime());
            } 
        }
    }
}