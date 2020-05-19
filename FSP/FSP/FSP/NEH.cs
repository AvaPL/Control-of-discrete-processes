using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace FSP
{
    public class NEH
    {
        private readonly SimplePriorityQueue<Task, int> tasksOrderedBySumOfPerformTimes;
        private List<Task> bestPermutation = new List<Task>();
        private LinkedList<Task> permutation = new LinkedList<Task>();

        private NEH(List<Task> tasks)
        {
            tasksOrderedBySumOfPerformTimes = OrderBySumOfPerformTimes(tasks);
        }

        public static FSPTimes Solve(List<Task> tasks)
        {
            NEH neh = new NEH(tasks);
            return neh.Solve();
        }

        private FSPTimes Solve()
        {
            while (tasksOrderedBySumOfPerformTimes.Count > 0)
                CalculateBestPermutation();

            return FSPTimes.Calculate(bestPermutation);
        }

        private void CalculateBestPermutation()
        {
            LinkedListNode<Task> longestTaskNode = GetLongestTaskNode();
            bestPermutation = null;
            CalculatePermutationWithTask(longestTaskNode);
            permutation = new LinkedList<Task>(bestPermutation);
        }

        private LinkedListNode<Task> GetLongestTaskNode()
        {
            Task longestTask = tasksOrderedBySumOfPerformTimes.Dequeue();
            LinkedListNode<Task> longestTaskNode = new LinkedListNode<Task>(longestTask);
            return longestTaskNode;
        }

        private void CalculatePermutationWithTask(LinkedListNode<Task> longestTaskNode)
        {
            for (LinkedListNode<Task> node = permutation.First; node != null; node = node.Next)
                CalculatePermutationWithTaskInsertedBefore(node, longestTaskNode);

            CalculatePermutationWithTaskInsertedLast(longestTaskNode);
        }

        private void CalculatePermutationWithTaskInsertedBefore(LinkedListNode<Task> node,
            LinkedListNode<Task> longestTaskNode)
        {
            permutation.AddBefore(node, longestTaskNode);
            ChooseBestPermutation();
            permutation.Remove(longestTaskNode);
        }

        private void ChooseBestPermutation()
        {
            if (bestPermutation == null)
                bestPermutation = new List<Task>(permutation);
            int permutationMaxCompleteTime = FSPTimes.Calculate(permutation.ToList()).GetMaxCompleteTime();
            int bestMaxCompleteTime = FSPTimes.Calculate(bestPermutation).GetMaxCompleteTime();
            if (permutationMaxCompleteTime < bestMaxCompleteTime)
                bestPermutation = new List<Task>(permutation);
        }

        private void CalculatePermutationWithTaskInsertedLast(LinkedListNode<Task> longestTaskNode)
        {
            permutation.AddLast(longestTaskNode);
            ChooseBestPermutation();
            permutation.Remove(longestTaskNode);
        }

        private static SimplePriorityQueue<Task, int> OrderBySumOfPerformTimes(List<Task> tasks)
        {
            SimplePriorityQueue<Task, int> tasksOrderedBySumOfPerformTimes = new SimplePriorityQueue<Task, int>();
            foreach (var task in tasks)
                tasksOrderedBySumOfPerformTimes.Enqueue(task, -task.PerformTimes.Sum());
            return tasksOrderedBySumOfPerformTimes;
        }
    }
}