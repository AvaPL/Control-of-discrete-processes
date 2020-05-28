using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FSP
{
    internal class Program
    {
        private static readonly string[] FilePaths =
        {
            @"../../../Data/ta/ta001.txt",
            @"../../../Data/ta/ta002.txt",
            @"../../../Data/ta/ta003.txt",
            @"../../../Data/ta/ta004.txt",
            @"../../../Data/ta/ta005.txt",
            @"../../../Data/ta/ta006.txt",
            @"../../../Data/ta/ta007.txt",
            @"../../../Data/ta/ta008.txt",
            @"../../../Data/ta/ta009.txt",
            @"../../../Data/ta/ta010.txt"
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("NEH");
            SolveForEachInput(NEH.Solve);
            
            Console.WriteLine("Simulated annealing");
            
            Console.WriteLine("Varying temperature reduction");
            foreach (var reduceTemperature in new[]
            {
                SimulatedAnnealing.ReduceTemperatureLinear(1000 / 1e3),
                SimulatedAnnealing.ReduceTemperatureLinear(1000 / 1e4),
                SimulatedAnnealing.ReduceTemperatureLinear(1000 / 1e5),
                SimulatedAnnealing.ReduceTemperatureGeometric(0.97),
                SimulatedAnnealing.ReduceTemperatureGeometric(0.95),
                SimulatedAnnealing.ReduceTemperatureGeometric(0.9)
            })
            {
                Console.WriteLine(reduceTemperature.Method.Name);
                double avg = 0;
                for (int i = 0; i < FilePaths.Length; i++)
                {
                    avg += MeasureTime(() =>
                    {
                        List<Task> tasks = ReadTasks(i);
                        SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, 1000, 5, tasks.Count,
                            SimulatedAnnealing.Twist(), reduceTemperature);
                        CalculateMaxCompleteTime(simulatedAnnealing, tasks);
                    });
                }
            
                avg /= FilePaths.Length;
                Console.WriteLine("Average: " + avg);
            
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine("Varying initial temperature");
            foreach (var initialTemperature in new[] {100, 1000, 10000})
            {
                double avg = 0;
                for (int i = 0; i < FilePaths.Length; i++)
                {
                    avg += MeasureTime(() =>
                    {
                        List<Task> tasks = ReadTasks(i);
                        SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, initialTemperature, 5,
                            tasks.Count, SimulatedAnnealing.Twist(),
                            SimulatedAnnealing.ReduceTemperatureGeometric(0.95));
                        CalculateMaxCompleteTime(simulatedAnnealing, tasks);
                    });
                }
            
                avg /= FilePaths.Length;
                Console.WriteLine("Average: " + avg);
            
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine("Varying number of epochs");
            foreach (var calculateEpochs in new Func<int, int>[] {i => (int) Math.Sqrt(i), i => i, i => i * i})
            {
                double avg = 0;
                for (int i = 0; i < FilePaths.Length; i++)
                {
                    avg += MeasureTime(() =>
                    {
                        List<Task> tasks = ReadTasks(i);
                        SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tasks, 1000, 5,
                            calculateEpochs.Invoke(tasks.Count), SimulatedAnnealing.Swap(),
                            SimulatedAnnealing.ReduceTemperatureLinear(1000/1e4));
                        CalculateMaxCompleteTime(simulatedAnnealing, tasks);
                    });
                }

                avg /= FilePaths.Length;
                Console.WriteLine("Average: " + avg);

                Console.WriteLine("------------------------------");
            }
        }

        private static List<Task> ReadTasks(int i)
        {
            using StreamReader fileReader = new StreamReader(FilePaths[i]);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            return tasks;
        }

        private static void CalculateMaxCompleteTime(SimulatedAnnealing simulatedAnnealing, List<Task> tasks)
        {
            FSPTimes fspTimes = simulatedAnnealing.Solve();
            PrintResults(tasks, fspTimes);
        }

        private static void PrintResults(List<Task> tasks, FSPTimes fspTimes)
        {
            Console.WriteLine("Tasks count: " + tasks.Count);
            Console.WriteLine("Machines count: " + tasks.First().GetNumberOfMachines());
            Console.WriteLine("Max complete time: " + fspTimes.GetMaxCompleteTime());
        }

        private static void SolveForEachInput(Func<List<Task>, FSPTimes> solve)
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                var task = System.Threading.Tasks.Task.Run(() => MeasureTime(() => CalculateMaxCompleteTime(solve, i)));
                if (!task.Wait(TimeSpan.FromSeconds(5 * 60)))
                {
                    Console.WriteLine("TIMED OUT");
                    break;
                }
            }

            Console.WriteLine("------------------------------");
        }

        private static double MeasureTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            double timeMs = 1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine("Time [ms]: " + timeMs);
            return timeMs;
        }

        private static void CalculateMaxCompleteTime(Func<List<Task>, FSPTimes> solve, int fileIndex)
        {
            List<Task> tasks = ReadTasks(fileIndex);
            FSPTimes fspTimes = solve.Invoke(tasks);
            PrintResults(tasks, fspTimes);
        }
    }
}