using System;
using System.Collections.Generic;
using System.IO;
using FSP;
using NUnit.Framework;

namespace FSPTests
{
    public class SimulatedAnnealingTests
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

        [Test]
        public void ShouldGiveMaxQuitTimeUsingSwapAndLinearTemperatureReduction()
        {
            foreach (string filePath in filePaths)
            {
                using StreamReader fileReader = new StreamReader(filePath);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                double initialTemperature = 100;
                double endTemperature = 5;
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature,
                    endTemperature, tasks.Count, SimulatedAnnealing.Swap(),
                    SimulatedAnnealing.ReduceTemperatureLinear(initialTemperature));
                FSPTimes fspTimes = simulatedAnnealing.Solve();
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }

        [Test]
        public void ShouldGiveMaxQuitTimeUsingTwistAndLinearTemperatureReduction()
        {
            foreach (string filePath in filePaths)
            {
                using StreamReader fileReader = new StreamReader(filePath);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                double initialTemperature = 100;
                double endTemperature = 5;
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature,
                    endTemperature, tasks.Count, SimulatedAnnealing.Twist(),
                    SimulatedAnnealing.ReduceTemperatureLinear(initialTemperature));
                FSPTimes fspTimes = simulatedAnnealing.Solve();
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }

        [Test]
        public void ShouldGiveMaxQuitTimeUsingSwapAndGeometricTemperatureReduction()
        {
            foreach (string filePath in filePaths)
            {
                using StreamReader fileReader = new StreamReader(filePath);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                double initialTemperature = 100;
                double endTemperature = 5;
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature,
                    endTemperature, tasks.Count, SimulatedAnnealing.Swap(),
                    SimulatedAnnealing.ReduceTemperatureGeometric(0.97));
                FSPTimes fspTimes = simulatedAnnealing.Solve();
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }

        [Test]
        public void ShouldGiveMaxQuitTimeUsingTwistAndGeometricTemperatureReduction()
        {
            foreach (string filePath in filePaths)
            {
                using StreamReader fileReader = new StreamReader(filePath);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                double initialTemperature = 100;
                double endTemperature = 5;
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature,
                    endTemperature, tasks.Count, SimulatedAnnealing.Twist(),
                    SimulatedAnnealing.ReduceTemperatureGeometric(0.97));
                FSPTimes fspTimes = simulatedAnnealing.Solve();
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }
    }
}