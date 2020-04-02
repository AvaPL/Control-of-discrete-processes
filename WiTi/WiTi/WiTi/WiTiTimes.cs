using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class WiTiTimes
    {
        public int[] StartTimes { get; }
        public int[] CompleteTimes { get; }
        public int[] WeightedTardiness { get; }

        public WiTiTimes(int numberOfTasks)
        {
            StartTimes = new int[numberOfTasks];
            CompleteTimes = new int[numberOfTasks];
            WeightedTardiness = new int[numberOfTasks];
        }

        public static WiTiTimes Calculate(List<Task> tasks)
        {
            WiTiTimes result = new WiTiTimes(tasks.Count);
            for (int i = 0; i < tasks.Count; i++)
            {
                result.StartTimes[i] = i - 1 < 0 ? 0 : result.CompleteTimes[i - 1];
                result.CompleteTimes[i] = result.StartTimes[i] + tasks[i].PerformTime;
                result.WeightedTardiness[i] = Math.Max(result.CompleteTimes[i] - tasks[i].Deadline, 0) * tasks[i].PenaltyWeight;
            }

            return result;
        }

        public int GetTotalWeightedTardiness()
        {
            return WeightedTardiness.Sum();
        }
    }
}