using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class WiTiTimes
    {
        public WiTiTimes(List<Task> tasks)
        {
            StartTimes = new int[tasks.Count];
            CompleteTimes = new int[tasks.Count];
            WeightedTardiness = new int[tasks.Count];
            Permutation = tasks;
            for (int i = 0; i < tasks.Count; i++)
            {
                StartTimes[i] = i - 1 < 0 ? 0 : CompleteTimes[i - 1];
                CompleteTimes[i] = StartTimes[i] + tasks[i].PerformTime;
                WeightedTardiness[i] =
                    Math.Max(CompleteTimes[i] - tasks[i].Deadline, 0) * tasks[i].PenaltyWeight;
            }

            TotalWeightedTardiness = WeightedTardiness.Sum();
        }

        public int[] StartTimes { get; }
        public int[] CompleteTimes { get; }
        public int[] WeightedTardiness { get; }
        public int TotalWeightedTardiness { get; }
        public List<Task> Permutation { get; }
    }
}