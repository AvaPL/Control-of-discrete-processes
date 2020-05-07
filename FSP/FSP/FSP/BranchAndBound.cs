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

        public enum UpperBoundLevel
        {
            Level0,
            Level1,
            Level2
        }

        private List<Task> optimalPermutation;
        private int? upperBound;
        private readonly LowerBoundLevel lowerBoundLevel;
        private readonly UpperBoundLevel upperBoundLevel;

        public BranchAndBound(UpperBoundLevel upperBoundLevel, LowerBoundLevel lowerBoundLevel)
        {
            this.upperBoundLevel = upperBoundLevel;
            this.lowerBoundLevel = lowerBoundLevel;
        }

        public static FSPTimes Solve(List<Task> tasks, UpperBoundLevel upperBoundLevel, LowerBoundLevel lowerBoundLevel)
        {
            BranchAndBound branchAndBound = new BranchAndBound(upperBoundLevel, lowerBoundLevel);
            branchAndBound.upperBound = branchAndBound.CalculateUpperBound(tasks);
            foreach (var task in tasks)
                branchAndBound.Solve(tasks, new List<Task>(), task);

            return FSPTimes.Calculate(branchAndBound.optimalPermutation);
        }

        private int? CalculateUpperBound(List<Task> tasks)
        {
            return upperBoundLevel switch
            {
                UpperBoundLevel.Level0 => new int?(),
                UpperBoundLevel.Level1 => CalculateUpperBoundLevel1(tasks),
                UpperBoundLevel.Level2 => CalculateUpperBoundLevel2(tasks),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private int CalculateUpperBoundLevel1(List<Task> tasks)
        {
            List<Task> randomShuffled = RandomShuffle(tasks);
            FSPTimes fspTimes = FSPTimes.Calculate(randomShuffled);
            return fspTimes.GetMaxCompleteTime();
        }

        private List<Task> RandomShuffle(List<Task> tasks)
        {
            Random rng = new Random();
            return tasks.OrderBy(task => rng.Next()).ToList();
        }

        private int CalculateUpperBoundLevel2(List<Task> tasks)
        {
            int upperBound = int.MaxValue;
            for (int i = 0; i < tasks.Count; i++)
            {
                List<Task> randomShuffled = RandomShuffle(tasks);
                FSPTimes fspTimes = FSPTimes.Calculate(randomShuffled);
                if (upperBound > fspTimes.GetMaxCompleteTime())
                    upperBound = fspTimes.GetMaxCompleteTime();
            }
            return upperBound;
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
                                 SumOfMinimumPerformTimes(ConcatenateLists(orderedTasks, unorderedTasks), i);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }

            return maxLowerBound;
        }

        private static List<Task> ConcatenateLists(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            List<Task> allTasks = new List<Task>(orderedTasks);
            allTasks.AddRange(unorderedTasks);
            return allTasks;
        }

        private static int SumOfMinimumPerformTimes(List<Task> tasks, int i)
        {
            int sum = 0;
            for (int k = i + 1; k < tasks.First().GetNumberOfMachines(); k++)
                sum += tasks.Min(task => task.PerformTimes[k]);

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