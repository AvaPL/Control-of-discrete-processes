﻿using System;
 using NUnit.Framework;
using RPQ;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.IO;
 using System.Linq;

 namespace RpqTests
{
    [TestFixture]
    public class RPQTimesTests
    {
        private static readonly string[] FilePaths = {
            @"../../Data/data10.txt",
            @"../../Data/data20.txt",
            @"../../Data/data50.txt",
            @"../../Data/data100.txt",
            @"../../Data/data200.txt",
            @"../../Data/data500.txt"
        };

        private static readonly int[] ExpectedUnsortedResults = {
            927,
            1905,
            2843,
            5324,
            11109,
            26706
        };

        private static readonly int[] ExpectedSortedResults = {
            746,
            1594,
            1915,
            3936,
            8210,
            19609
        };
        
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
            for (int i = 0; i < FilePaths.Length; i++)
            {
                StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                RPQTimes rpqTimes = RPQTimes.Calculate(tasks);
                Assert.AreEqual(ExpectedUnsortedResults[i], rpqTimes.GetMaxQuitTime());
            } 
        }
        
        [Test]
        public void ShouldGiveCorrectMaxQuitTimeForTasksSortedByReadyTime()
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                StreamReader fileReader = new StreamReader(FilePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                Sort(tasks);
                RPQTimes rpqTimes = RPQTimes.Calculate(tasks);
                Assert.AreEqual(ExpectedSortedResults[i], rpqTimes.GetMaxQuitTime());
            } 
        }

        private static void Sort(List<Task> tasks)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            tasks.Sort();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed);
        }
    }
}