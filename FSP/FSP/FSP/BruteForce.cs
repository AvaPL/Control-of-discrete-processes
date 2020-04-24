using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace FSP
{
    public class BruteForce
    {
        private FSPTimes optimalPermutation;

        public static FSPTimes SolveUsingPermutations(List<Task> tasks)
        {
            Permutations<Task> permutations = new Permutations<Task>(tasks);
            FSPTimes result = FSPTimes.Calculate(tasks);

            foreach (var permutation in permutations)
            {
                FSPTimes fspTimes = FSPTimes.Calculate(permutation.ToList());
                if (result.GetMaxCompleteTime() > fspTimes.GetMaxCompleteTime())
                    result = fspTimes;
            }

            return result;
        }

        public static FSPTimes SolveUsingRecursion(List<Task> tasks)
        {
            BruteForce bruteForce = new BruteForce();
            bruteForce.SolveUsingRecursion(new LinkedList<Task>(tasks), new List<Task>());
            return bruteForce.optimalPermutation;
        }

        private void SolveUsingRecursion(LinkedList<Task> tasksToAdd, List<Task> permutation)
        {
            if (tasksToAdd.Count == 0)
                PickOptimalPermutation(permutation);
            foreach (var task in tasksToAdd)
            {
                LinkedList<Task> tasksToAddCopy = CopyWithoutTask(tasksToAdd, task);
                List<Task> permutationCopy = new List<Task>(permutation) {task};
                SolveUsingRecursion(tasksToAddCopy, permutationCopy);
            }
        }

        private static LinkedList<Task> CopyWithoutTask(LinkedList<Task> tasksToAdd, Task taskToRemove)
        {
            LinkedList<Task> tasksToAddCopy = new LinkedList<Task>(tasksToAdd);
            tasksToAddCopy.Remove(taskToRemove);
            return tasksToAddCopy;
        }

        private void PickOptimalPermutation(List<Task> permutation)
        {
            FSPTimes recursionResult = FSPTimes.Calculate(permutation);
            if (optimalPermutation == null ||
                recursionResult.GetMaxCompleteTime() < optimalPermutation.GetMaxCompleteTime())
                optimalPermutation = recursionResult;
        }
    }
}