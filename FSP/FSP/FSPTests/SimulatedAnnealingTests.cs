using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private static readonly Random random = new Random();
        
        private static List<Task> Swap(List<Task> permutation)
        {
            int index1 = random.Next(0, permutation.Count);
            int index2 = random.Next(0, permutation.Count);
            List<Task> newPermutation = new List<Task>(permutation);
            newPermutation[index1] = permutation[index2];
            newPermutation[index2] = permutation[index1];
            return newPermutation;
        }

        private static List<Task> Twist(List<Task> permutation)
        {
            int start = random.Next(0, permutation.Count);
            int end = random.Next(0, permutation.Count);
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }

            IEnumerable<Task> firstPart = permutation.Take(start);
            IEnumerable<Task> secondPart = permutation.Skip(start).Take(end - start).Reverse();
            IEnumerable<Task> thirdPart = permutation.Skip(end);
            return firstPart.Concat(secondPart).Concat(thirdPart).ToList();
        }

        private static Func<double, double> ReduceTemperatureLinear(double initialTemperature)
        {
            double linearCoefficient = initialTemperature / 10e3;
            return temperature => ReduceTemperatureLinear(temperature, linearCoefficient);
        }

        private static double ReduceTemperatureLinear(double temperature, double linearCoefficient)
        {
            return temperature - linearCoefficient;
        }

        private static Func<double, double> ReduceTemperatureGeometric(double geometricCoefficient)
        {
            return temperature => ReduceTemperatureGeometric(temperature, geometricCoefficient);
        }
        
        private static double ReduceTemperatureGeometric(double temperature, double geometricCoefficient)
        {
            return geometricCoefficient * temperature;
        }

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
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature, endTemperature, tasks.Count, Swap, ReduceTemperatureLinear(initialTemperature));
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
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature, endTemperature, tasks.Count, Twist, ReduceTemperatureLinear(initialTemperature));
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
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature, endTemperature, tasks.Count, Swap, ReduceTemperatureGeometric(0.97));
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
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature, endTemperature, tasks.Count, Twist, ReduceTemperatureGeometric(0.97));
                FSPTimes fspTimes = simulatedAnnealing.Solve();
                Console.WriteLine(filePath + ": " + fspTimes.GetMaxCompleteTime());
            }
        }
    }
}