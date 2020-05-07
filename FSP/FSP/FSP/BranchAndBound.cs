using System;
using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class BranchAndBound
    {
        public enum LowerBoundLevel
        {
            Level1,
            Level2,
            Level3,
            Level4
        }

        private List<Task> optimalPermutation;
        private int? upperBound;
        private readonly LowerBoundLevel lowerBoundLevel;

        public BranchAndBound(LowerBoundLevel lowerBoundLevel)
        {
            this.lowerBoundLevel = lowerBoundLevel;
        }

        public static FSPTimes Solve(List<Task> tasks, LowerBoundLevel lowerBoundLevel)
        {
            BranchAndBound branchAndBound = new BranchAndBound(lowerBoundLevel);
            foreach (var task in tasks)
                branchAndBound.Solve(tasks, new List<Task>(), task);

            return FSPTimes.Calculate(branchAndBound.optimalPermutation);
        }

        private void Solve(IEnumerable<Task> unorderedTasks, List<Task> permutation, Task currentTask)
        {
            LinkedList<Task> unorderedTasksCopy = new LinkedList<Task>(unorderedTasks);
            unorderedTasksCopy.Remove(currentTask);
            List<Task> permutationCopy = new List<Task>(permutation) {currentTask};
            if (unorderedTasksCopy.Count > 0)
            {
                int lowerBound = CalculateLowerBound(permutationCopy, unorderedTasksCopy.ToList());
                if (!upperBound.HasValue || lowerBound <= upperBound)
                    foreach (var task in unorderedTasksCopy)
                        Solve(unorderedTasksCopy, permutationCopy, task);
            }
            else
            {
                FSPTimes fspTimes = FSPTimes.Calculate(permutationCopy);
                int maxCompleteTime = fspTimes.GetMaxCompleteTime();
                if (!upperBound.HasValue || maxCompleteTime < upperBound)
                {
                    upperBound = maxCompleteTime;
                    optimalPermutation = permutationCopy;
                }
            }
        }

        private int CalculateLowerBound(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            return lowerBoundLevel switch
            {
                LowerBoundLevel.Level1 => CalculateLowerBoundLevel1(orderedTasks, unorderedTasks),
                LowerBoundLevel.Level2 => CalculateLowerBoundLevel2(orderedTasks, unorderedTasks),
                LowerBoundLevel.Level3 => CalculateLowerBoundLevel3(orderedTasks, unorderedTasks),
                LowerBoundLevel.Level4 => CalculateLowerBoundLevel4(orderedTasks, unorderedTasks),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static int CalculateLowerBoundLevel1(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            FSPTimes orderedTasksTimes = FSPTimes.Calculate(orderedTasks);
            int maxLowerBound = 0;
            for (int i = 0; i < orderedTasks.First().GetNumberOfMachines(); i++)
            {
                int lowerBound = orderedTasksTimes.CompleteTimes[i][orderedTasks.Count - 1] +
                                 unorderedTasks.Sum(task => task.PerformTimes[i]);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }

            return maxLowerBound;
        }

        private static int CalculateLowerBoundLevel2(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            FSPTimes orderedTasksTimes = FSPTimes.Calculate(orderedTasks);
            int maxLowerBound = 0;
            for (int i = 0; i < orderedTasks.First().GetNumberOfMachines(); i++)
            {
                int lowerBound = orderedTasksTimes.CompleteTimes[i][orderedTasks.Count - 1] +
                                 unorderedTasks.Sum(task => task.PerformTimes[i]) +
                                 SumOfMinimumPerformTimes(orderedTasks, unorderedTasks, i);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }

            return maxLowerBound;
        }

        private static int SumOfMinimumPerformTimes(List<Task> orderedTasks, List<Task> unorderedTasks, int i)
        {
            List<Task> allTasks = new List<Task>(orderedTasks);
            allTasks.AddRange(unorderedTasks);
            int sum = 0;
            for (int k = i + 1; k < allTasks.First().GetNumberOfMachines(); k++)
                sum += allTasks.Min(task => task.PerformTimes[k]);

            return sum;
        }

        private static int CalculateLowerBoundLevel3(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            FSPTimes orderedTasksTimes = FSPTimes.Calculate(orderedTasks);
            int maxLowerBound = 0;
            for (int i = 0; i < orderedTasks.First().GetNumberOfMachines(); i++)
            {
                int lowerBound = orderedTasksTimes.CompleteTimes[i][orderedTasks.Count - 1] +
                                 unorderedTasks.Sum(task => task.PerformTimes[i]) +
                                 SumOfMinimumPerformTimes(unorderedTasks, i);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }

            return maxLowerBound;
        }

        private static int SumOfMinimumPerformTimes(List<Task> unorderedTasks, int i)
        {
            int sum = 0;
            for (int k = i + 1; k < unorderedTasks.First().GetNumberOfMachines(); k++)
                sum += unorderedTasks.Min(task => task.PerformTimes[k]);

            return sum;
        }

        private static int CalculateLowerBoundLevel4(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            FSPTimes orderedTasksTimes = FSPTimes.Calculate(orderedTasks);
            int maxLowerBound = 0;
            for (int i = 0; i < orderedTasks.First().GetNumberOfMachines(); i++)
            {
                int lowerBound = orderedTasksTimes.CompleteTimes[i][orderedTasks.Count - 1] +
                                 unorderedTasks.Sum(task => task.PerformTimes[i]) +
                                 MinimumSumOfPerformTimes(unorderedTasks, i);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }

            return maxLowerBound;
        }

        private static int MinimumSumOfPerformTimes(List<Task> unorderedTasks, int i)
        {
            int minimum = 0;
            foreach (var task in unorderedTasks)
            {
                int sum = task.PerformTimes.Skip(i).Sum();
                if (sum < minimum)
                    minimum = sum;
            }

            return minimum;
        }
    }
}