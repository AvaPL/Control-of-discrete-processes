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
            totalWeightedTardiness = new int?[GetPermutationsCount(tasks.Count)];
            this.tasks = tasks;
        }

        private static int GetPermutationsCount(int tasksCount)
        {
            return Convert.ToInt32(Math.Pow(2, tasksCount));
        }

        public static int SolveUsingRecursion(List<Task> tasks)
        {
            DynamicProgramming dynamicProgramming = new DynamicProgramming(tasks);
            int permutation = GetPermutationsCount(tasks.Count) - 1;
            return dynamicProgramming.SolveUsingRecursion(permutation);
        }

        private int SolveUsingRecursion(int permutation)
        {
            if (permutation <= 0)
                return 0;
            IEnumerable<TaskWithIndex> tasksSublist = GetTasksSublist(tasks, permutation);
            totalWeightedTardiness[permutation] = tasksSublist.Select(t =>
                GetPenaltyPlacedLast(tasksSublist, t) + GetTotalWeightedTardiness(permutation, t.Index)).Min();
            return totalWeightedTardiness[permutation].Value;
        }

        private static IEnumerable<TaskWithIndex> GetTasksSublist(List<Task> tasks, int permutation)
        {
            return Enumerable.Range(0, tasks.Count)
                .Where(i => DoesPermutationContainIndex(permutation, i))
                .Select(i => new TaskWithIndex(tasks[i], i));
        }

        private static bool DoesPermutationContainIndex(int permutation, int index)
        {
            return (permutation >> index & 1) == 1;
        }

        private static int GetPenaltyPlacedLast(IEnumerable<TaskWithIndex> tasksSublist, TaskWithIndex task)
        {
            return Math.Max(-task.Deadline + tasksSublist.Select(t => t.PerformTime).Sum(), 0) * task.PenaltyWeight;
        }

        private int GetTotalWeightedTardiness(int permutation, int index)
        {
            int newPermutation = GetPermutationWithoutIndex(permutation, index);
            return totalWeightedTardiness[newPermutation] ?? SolveUsingRecursion(newPermutation);
        }

        private static int GetPermutationWithoutIndex(int permutation, int index)
        {
            return permutation & ~(1 << index);
        }

        public static int SolveUsingIteration(List<Task> tasks)
        {
            int numberOfPermutations = GetPermutationsCount(tasks.Count);
            int[] totalWeightedTardiness = new int[numberOfPermutations];
            for (int permutation = 1; permutation < numberOfPermutations; permutation++)
            {
                IEnumerable<TaskWithIndex> tasksSublist = GetTasksSublist(tasks, permutation);
                totalWeightedTardiness[permutation] = tasksSublist.Select(t =>
                    GetPenaltyPlacedLast(tasksSublist, t) +
                    totalWeightedTardiness[GetPermutationWithoutIndex(permutation, t.Index)]).Min();
            }

            return totalWeightedTardiness.Last();
        }

        private class TaskWithIndex : Task
        {
            public TaskWithIndex(Task task, int index) : base(task.PerformTime, task.PenaltyWeight, task.Deadline)
            {
                Index = index;
            }

            public int Index { get; }
        }
    }
}