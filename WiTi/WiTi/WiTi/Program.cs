using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WiTi
{
    internal class Program
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

        private static readonly int[] ReferencedTotalWeightedTardiness =
        {
            1004,
            962,
            915,
            681,
            646,
            310,
            321,
            746,
            539,
            688,
            514
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("SortD");
            SolveForEachInput(x =>
            {
                x.Sort();
                return new WiTiTimes(x).TotalWeightedTardiness;
            });
            Console.WriteLine("Brute Force using permutations");
            SolveForEachInput(x => BruteForce.SolveUsingPermutations(x).TotalWeightedTardiness);
            Console.WriteLine("Brute Force using recursion");
            SolveForEachInput(x => BruteForce.SolveUsingRecursion(x).TotalWeightedTardiness);
            Console.WriteLine("Dynamic Programming using iteration");
            SolveForEachInput(DynamicProgramming.SolveUsingIteration);
            Console.WriteLine("Dynamic Programming using recursion");
            SolveForEachInput(DynamicProgramming.SolveUsingRecursion);
        }

        private static void SolveForEachInput(Func<List<Task>, int> solve)
        {
            for (int i = 0; i < FilePaths.Length; i++)
            {
                var task = System.Threading.Tasks.Task.Run(() =>
                    MeasureTime(() => CalculateTotalWeightedTardiness(solve, i)));
                if (!task.Wait(TimeSpan.FromSeconds(5 * 60)))
                {
                    Console.WriteLine("TIMED OUT");
                    break;
                }
            }

            Console.WriteLine("------------------------------");
        }

        private static void MeasureTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            Console.WriteLine("Time [ms]: " + 1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
        }

        private static void CalculateTotalWeightedTardiness(Func<List<Task>, int> solve, int index)
        {
            using StreamReader fileReader = new StreamReader(FilePaths[index]);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            int totalWeightedTardiness = solve.Invoke(tasks);
            Console.WriteLine("Data size: " + tasks.Count);
            Console.WriteLine("Total weighted tardiness: " + totalWeightedTardiness);
            Console.WriteLine("PRD: " + 100 * (totalWeightedTardiness - ReferencedTotalWeightedTardiness[index]) /
                ReferencedTotalWeightedTardiness[index] + "%");
        }
    }
}