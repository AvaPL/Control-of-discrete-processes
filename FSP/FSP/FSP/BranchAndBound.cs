using System.Collections.Generic;

namespace FSP
{
    public class BranchAndBound
    {
        private int? upperBound;
        private List<Task> optimalPermutation;

        public static List<Task> Solve(List<Task> tasks)
        {
            BranchAndBound branchAndBound = new BranchAndBound();
            foreach (var task in tasks) 
                branchAndBound.Solve(new LinkedList<Task>(tasks), task);

            return branchAndBound.optimalPermutation;
        }

        private void Solve(LinkedList<Task> tasks, Task currentTask)
        {
            List<Task> permutation = new List<Task>();
            permutation.Add(currentTask);
            tasks.Remove(currentTask);
            if (tasks.Count > 0)
            {
                int lowerBound = CalculateLowerBound(permutation);
                if (!upperBound.HasValue || lowerBound <= upperBound)
                    foreach (var task in tasks)
                        Solve(tasks, task);
            }
            else
            {
                FSPTimes fspTimes = FSPTimes.Calculate(permutation);
                int maxCompleteTime = fspTimes.GetMaxCompleteTime();
                if (maxCompleteTime < upperBound)
                {
                    upperBound = maxCompleteTime;
                    optimalPermutation = permutation;
                }
            }
        }

        private static int CalculateLowerBound(List<Task> permutation)
        {
            throw new System.NotImplementedException();
        }
    }
}