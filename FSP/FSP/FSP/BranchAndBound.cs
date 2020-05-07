using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class BranchAndBound
    {
        private int? upperBound;
        private List<Task> optimalPermutation;

        public static FSPTimes Solve(List<Task> tasks)
        {
            BranchAndBound branchAndBound = new BranchAndBound();
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

        private static int CalculateLowerBound(List<Task> orderedTasks, List<Task> unorderedTasks)
        {
            FSPTimes orderedTasksTimes = FSPTimes.Calculate(orderedTasks);
            int maxLowerBound = 0;
            for (int i = 0; i < orderedTasksTimes.GetNumberOfMachines(); i++)
            {
                int lowerBound = orderedTasksTimes.CompleteTimes[i][orderedTasks.Count - 1] +
                                 unorderedTasks.Sum(task => task.PerformTimes[i]);
                if (lowerBound > maxLowerBound)
                    maxLowerBound = lowerBound;
            }
            
            return maxLowerBound;
        }
    }
}