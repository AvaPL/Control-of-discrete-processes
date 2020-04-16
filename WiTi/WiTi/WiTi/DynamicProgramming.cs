using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class DynamicProgramming
    {
        private readonly List<Task> tasks;

        private int?[] totalWeightedTardiness;

        private DynamicProgramming(List<Task> tasks)
        {
            totalWeightedTardiness = new int?[Convert.ToInt32(Math.Pow(2, tasks.Count))];
            this.tasks = tasks;
        }

        public static int SolveUsingRecursion(List<Task> tasks)
        {
            DynamicProgramming dynamicProgramming = new DynamicProgramming(tasks);
            int permutation = Convert.ToInt32(Math.Pow(2, tasks.Count)) - 1;
            return dynamicProgramming.SolveUsingRecursion(permutation);
        }

        private int SolveUsingRecursion(int permutation)
        {
            if (permutation <= 0)
                return 0;

            List<int> indices = new List<int>();
            List<Task> tasksSublist = new List<Task>();
            for (int i = 0; i < tasks.Count; i++)
                if ((permutation >> i & 1) == 1)
                {
                    indices.Add(i);
                    tasksSublist.Add(tasks[i]);
                }

            totalWeightedTardiness[permutation] = tasksSublist.Select((t, i) =>
                Math.Max(-t.Deadline + tasksSublist.Select(t => t.PerformTime).Sum(), 0) * t.PenaltyWeight +
                GetTotalWeightedTardiness(permutation, indices[i])).Min();

            return totalWeightedTardiness[permutation].Value;
        }

        private int GetTotalWeightedTardiness(int permutation, int i)
        {
            int newPermutation = permutation & ~(1 << i);
            return totalWeightedTardiness[newPermutation] ?? SolveUsingRecursion(newPermutation);
        }
    }
}