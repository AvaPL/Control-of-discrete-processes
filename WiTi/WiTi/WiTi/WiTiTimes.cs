using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class WiTiTimes
    {
        private int? totalWeightedTardiness;

        public WiTiTimes(List<Task> tasks)
        {
            StartTimes = new List<int>(tasks.Count);
            CompleteTimes = new List<int>(tasks.Count);
            WeightedTardiness = new List<int>(tasks.Count);
            Permutation = tasks;
            foreach (var task in tasks)
                Add(task);

            TotalWeightedTardiness = GetTotalWeightedTardiness();
        }

        public WiTiTimes(int numberOfTasks)
        {
            StartTimes = new List<int>(numberOfTasks);
            CompleteTimes = new List<int>(numberOfTasks);
            WeightedTardiness = new List<int>(numberOfTasks);
            Permutation = new List<Task>(numberOfTasks);
            TotalWeightedTardiness = -1;
        }

        public WiTiTimes(WiTiTimes wiTiTimes)
        {
            StartTimes = wiTiTimes.StartTimes;
            CompleteTimes = wiTiTimes.CompleteTimes;
            WeightedTardiness = wiTiTimes.WeightedTardiness;
            TotalWeightedTardiness = wiTiTimes.TotalWeightedTardiness;
            Permutation = wiTiTimes.Permutation;
        }

        public List<int> StartTimes { get; }
        public List<int> CompleteTimes { get; }
        public List<int> WeightedTardiness { get; }

        public int TotalWeightedTardiness
        {
            get => totalWeightedTardiness ?? GetTotalWeightedTardiness();
            private set => totalWeightedTardiness = value;
        }

        public List<Task> Permutation { get; }

        private void Add(Task task)
        {
            StartTimes.Add(CompleteTimes.Count > 0 ? CompleteTimes.Last() : 0);
            CompleteTimes.Add(StartTimes.Last() + task.PerformTime);
            WeightedTardiness.Add(Math.Max(CompleteTimes.Last() - task.Deadline, 0) * task.PenaltyWeight);
        }

        public void AddTask(Task task)
        {
            Permutation.Add(task);
            Add(task);
        }

        private int GetTotalWeightedTardiness()
        {
            return WeightedTardiness.Sum();
        }
    }
}