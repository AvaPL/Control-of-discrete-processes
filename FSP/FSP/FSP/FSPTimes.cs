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
            StartTimes = InitializeTimesList();
            CompleteTimes = InitializeTimesList();
        }

        private List<List<int>> InitializeTimesList()
        {
            int numberOfMachines = GetNumberOfMachines();
            return InitializeTimesList(numberOfMachines);
        }
        
        private List<List<int>> InitializeTimesList(int numberOfMachines)
        {
            List<List<int>> result = new List<List<int>>(numberOfMachines);
            for (int i = 0; i < numberOfMachines; i++)
                result.Add(new List<int>(Permutation.Count));
            return result;
        }

        public List<Task> Permutation { get; }
        public List<List<int>> StartTimes { get; }
        public List<List<int>> CompleteTimes { get; }

        private int GetNumberOfMachines()
        {
            return Permutation.First().GetNumberOfMachines();
        }
        
        public static FSPTimes Calculate(List<Task> tasks)
        {
            FSPTimes result = new FSPTimes(tasks);
            foreach (var task in tasks) 
                result.Add(task);
            return result;
        }

        private void Add(Task task)
        {
            for (int i = 0; i < GetNumberOfMachines(); i++)
            {
                StartTimes[i].Add(Math.Max(i == 0 ? 0 : CompleteTimes[i - 1].Last(),
                    CompleteTimes[i].Count == 0 ? 0 : CompleteTimes[i].Last()));
                CompleteTimes[i].Add(StartTimes[i].Last() + task.PerformTimes[i]);
            }
        }

        public int GetMaxCompleteTime()
        {
            return CompleteTimes.Last().Last();
        }
    }
}