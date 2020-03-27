using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RPQ
{
    internal class Program
    {
        private static readonly string[] FilePaths =
        {
            @"../../../Data/data10.txt",
            @"../../../Data/data20.txt",
            @"../../../Data/data50.txt",
            @"../../../Data/data100.txt",
            @"../../../Data/data200.txt",
            @"../../../Data/data500.txt"
        };

        private static readonly int[] ReferenceMaxQuitTimes =
        {
            641,
            1267,
            1492,
            3070,
            6398,
            14785
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Natural permutation");
            SolveForEachInput(x => x);
            Console.WriteLine("Sorted by ready time");
            SolveForEachInput(x =>
            {
                x.Sort();
                return x;
            });
            Console.WriteLine("Sorted by ready and quit time");
            SolveForEachInput(x =>
            {
                x.Sort();
                return x;
            });
            Console.WriteLine("Schrage");
            SolveForEachInput(x => Schrage.Solve(x));
            Console.WriteLine("Schrage with priority queue");
            SolveForEachInput(x => SchrageWithQueue.Solve(x));
            Console.WriteLine("Schrage PMTN");
            SolveForEachInput(x => InterruptedSchrage.Solve(x));
            Console.WriteLine("Schrage PMTN with priority queue");
            SolveForEachInput(x => InterruptedSchrageWithQueue.Solve(x));
            Console.WriteLine("Carlier");
            SolveForEachInput(x => Carlier.Solve(x));
        }

        private static void SolveForEachInput(Func<List<Task>, List<Task>> solve)
        {
            for (int i = 0; i < FilePaths.Length; i++)
                MeasureTime(() => CalculateMaxQuitTime(solve, i));
            Console.WriteLine("------------------------------");
        }

        private static void MeasureTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            Console.WriteLine("Time [ms]: " + 1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
        }

        private static void CalculateMaxQuitTime(Func<List<Task>, List<Task>> solve, int index)
        {
            CalculateMaxQuitTime(x =>
            {
                List<Task> orderedTasks = solve.Invoke(x);
                RPQTimes rpqTimes = RPQTimes.Calculate(orderedTasks);
                return rpqTimes.GetMaxQuitTime();
            }, index);
        }

        private static void CalculateMaxQuitTime(Func<List<Task>, int> solve, int index)
        {
            using StreamReader fileReader = new StreamReader(FilePaths[index]);
            TaskReader taskReader = new TaskReader();
            List<Task> unorderedTasks = taskReader.ReadTasksFromFile(fileReader);
            int maxQuitTime = solve.Invoke(unorderedTasks);
            Console.WriteLine("Data size: " + unorderedTasks.Count);
            Console.WriteLine("Max quit time: " + maxQuitTime);
            Console.WriteLine("PRD: " + 100 * (maxQuitTime - ReferenceMaxQuitTimes[index]) /
                ReferenceMaxQuitTimes[index] + "%");
        }

        private static void SolveForEachInput(Func<List<Task>, int> solve)
        {
            for (int i = 0; i < FilePaths.Length; i++)
                MeasureTime(() => CalculateMaxQuitTime(solve, i));
            Console.WriteLine("------------------------------");
        }
    }
}