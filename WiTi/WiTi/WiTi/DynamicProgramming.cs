using System;
using System.Collections.Generic;
using System.Linq;

namespace WiTi
{
    public class DynamicProgramming
    {
        private readonly List<Task> tasks;
        private readonly int?[] totalWeightedTardiness;

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

            var tasksSublist = Enumerable.Range(0, tasks.Count)
                .Where(i => DoesPermutationContainIndex(permutation, i))
                .Select(i => new {t = tasks[i], i});

            totalWeightedTardiness[permutation] = tasksSublist.Select(ti =>
                Math.Max(-ti.t.Deadline + tasksSublist.Select(ti => ti.t.PerformTime).Sum(), 0) * ti.t.PenaltyWeight +
                GetTotalWeightedTardiness(permutation, ti.i)).Min();

            return totalWeightedTardiness[permutation].Value;
        }

        private static bool DoesPermutationContainIndex(int permutation, int i)
        {
            return (permutation >> i & 1) == 1;
        }

        private int GetTotalWeightedTardiness(int permutation, int i)
        {
            int newPermutation = permutation & ~(1 << i);
            return totalWeightedTardiness[newPermutation] ?? SolveUsingRecursion(newPermutation);
        }

        public static int SolveUsingIteration(List<Task> tasks)
        {
            int numberOfPermutations = Convert.ToInt32(Math.Pow(2, tasks.Count));
            int[] totalWeightedTardiness = new int[numberOfPermutations];
            for (int k = 1; k < numberOfPermutations; k++)
            {
                var tasksSublist = Enumerable.Range(0, tasks.Count)
                    .Where(i => DoesPermutationContainIndex(k, i))
                    .Select(i => new {t = tasks[i], i});

                totalWeightedTardiness[k] = tasksSublist.Select(ti =>
                    Math.Max(-ti.t.Deadline + tasksSublist.Select(ti => ti.t.PerformTime).Sum(), 0) *
                    ti.t.PenaltyWeight + totalWeightedTardiness[k & ~(1 << ti.i)]).Min();
            }

            return totalWeightedTardiness.Last();
        }
    }
}