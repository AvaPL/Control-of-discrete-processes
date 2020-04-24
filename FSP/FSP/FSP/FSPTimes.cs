using System;
using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class FSPTimes
    {
        private FSPTimes(List<Task> tasks)
        {
            Permutation = tasks;
            int numberOfMachines = tasks.First().PerformTimes.Count;
            StartTimes = InitializeTimesArray(numberOfMachines, tasks.Count);
            CompleteTimes = InitializeTimesArray(numberOfMachines, tasks.Count);
        }

        private static int[][] InitializeTimesArray(int numberOfMachines, int numberOfTasks)
        {
            int[][] result = new int[numberOfMachines][];
            for (int i = 0; i < result.Length; i++)
                result[i] = new int[numberOfTasks];
            return result;
        }
        
        public List<Task> Permutation { get; }
        public int[][] StartTimes { get; }
        public int[][] CompleteTimes { get; }

        public static FSPTimes Calculate(List<Task> tasks)
        {
            int numberOfMachines = tasks.First().PerformTimes.Count;
            FSPTimes result = new FSPTimes(numberOfMachines, tasks.Count);
            for (int i = 0; i < numberOfMachines; i++)
            for (int j = 0; j < tasks.Count; j++)
            {
                result.StartTimes[i][j] = Math.Max(i == 0 ? 0 : result.CompleteTimes[i - 1][j],
                    j == 0 ? 0 : result.CompleteTimes[i][j - 1]);
                result.CompleteTimes[i][j] = result.StartTimes[i][j] + tasks[j].PerformTimes[i];
            }

            return result;
        }

        public int GetMaxCompleteTime()
        {
            return CompleteTimes.Last().Last();
        }
    }
}