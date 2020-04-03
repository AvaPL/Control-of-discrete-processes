using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class WiTiTimes
    {
        private int? totalWeightedTardiness;

        public WiTiTimes(IEnumerable<Task> tasks)
        {
            StartTimes = new List<int>();
            CompleteTimes = new List<int>();
            WeightedTardiness = new List<int>();
            Permutation = tasks.ToList();
            foreach (var task in tasks)
                Add(task);
            TotalWeightedTardiness = GetTotalWeightedTardiness();
        }

        public WiTiTimes()
        {
            StartTimes = new List<int>();
            CompleteTimes = new List<int>();
            WeightedTardiness = new List<int>();
            Permutation = new List<Task>();
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