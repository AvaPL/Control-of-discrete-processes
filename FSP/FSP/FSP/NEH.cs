using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace FSP
{
    public class NEH
    {
        public static FSPTimes Solve(List<Task> tasks)
        {
            SimplePriorityQueue<Task, int> tasksOrderedBySumOfPerformTimes = new SimplePriorityQueue<Task, int>();
            foreach (var task in tasks) 
                tasksOrderedBySumOfPerformTimes.Enqueue(task, -task.PerformTimes.Sum());

            List<Task> optimalPermutation = new List<Task>();
            LinkedList<Task> permutation = new LinkedList<Task>();
            while (tasksOrderedBySumOfPerformTimes.Count>0)
            {
                Task longestTask = tasksOrderedBySumOfPerformTimes.Dequeue();
                LinkedListNode<Task> longestTaskNode = new LinkedListNode<Task>(longestTask);
                optimalPermutation = null;
                for(LinkedListNode<Task> node=permutation.First; node != null; node=node.Next)
                {
                    permutation.AddBefore(node, longestTaskNode);
                    if (optimalPermutation == null || FSPTimes.Calculate(permutation.ToList()).GetMaxCompleteTime() < FSPTimes.Calculate(optimalPermutation).GetMaxCompleteTime())
                        optimalPermutation = new List<Task>(permutation);

                    permutation.Remove(longestTaskNode);
                }

                permutation.AddLast(longestTaskNode);
                if (optimalPermutation == null || FSPTimes.Calculate(permutation.ToList()).GetMaxCompleteTime() < FSPTimes.Calculate(optimalPermutation).GetMaxCompleteTime())
                    optimalPermutation = new List<Task>(permutation);
                permutation.Remove(longestTaskNode);
                permutation = new LinkedList<Task>(optimalPermutation);
            }

            return FSPTimes.Calculate(optimalPermutation);
        }
    }
}