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
            @"../../../Data/data001.txt",
            @"../../../Data/data002.txt",
            @"../../../Data/data003.txt",
            @"../../../Data/data004.txt",
            @"../../../Data/data005.txt",
            @"../../../Data/data006.txt"
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Brute Force using permutations");
            SolveForEachInput(BruteForce.SolveUsingPermutations);
            Console.WriteLine("Brute Force using recursion");
            SolveForEachInput(BruteForce.SolveUsingRecursion);
            Console.WriteLine("Johnson");
            SolveForEachInput(Johnson.Solve);
            
            foreach (var level in (BranchAndBound.LowerBoundLevel[]) Enum.GetValues(
                typeof(BranchAndBound.LowerBoundLevel)))
            {
                Console.WriteLine("Branch and bound, const upper bound: " + BranchAndBound.UpperBoundLevel.Level0);
                Console.WriteLine("Lower bound: " + level);
                SolveForEachInput(x => BranchAndBound.Solve(x, BranchAndBound.UpperBoundLevel.Level0, level));
            }
            
            foreach (var level in (BranchAndBound.UpperBoundLevel[]) Enum.GetValues(
                typeof(BranchAndBound.UpperBoundLevel)))
            {
                Console.WriteLine("Branch and bound, const lower bound: " + BranchAndBound.LowerBoundLevel.Level3);
                Console.WriteLine("Upper bound: " + level);
                SolveForEachInput(x => BranchAndBound.Solve(x, level, BranchAndBound.LowerBoundLevel.Level3));
            }
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

        private static void MeasureTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            Console.WriteLine("Time [ms]: " + 1000.0 * stopwatch.ElapsedTicks / Stopwatch.Frequency);
        }

        private static void CalculateMaxCompleteTime(Func<List<Task>, FSPTimes> solve, int fileIndex)
        {
            using StreamReader fileReader = new StreamReader(FilePaths[fileIndex]);
            TaskReader taskReader = new TaskReader();
            List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
            FSPTimes fspTimes = solve.Invoke(tasks);
            Console.WriteLine("Tasks count: " + tasks.Count);
            Console.WriteLine("Machines count: " + tasks.First().GetNumberOfMachines());
            Console.WriteLine("Max complete time: " + fspTimes.GetMaxCompleteTime());
        }
    }
}