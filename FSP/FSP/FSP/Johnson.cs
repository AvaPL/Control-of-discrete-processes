using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class Johnson
    {
        public static FSPTimes Solve(List<Task> tasks)
        {
            LinkedList<Task> tasksLinkedList = new LinkedList<Task>(tasks);
            Task[] permutation = new Task[tasks.Count];
            int i = 0;
            int j = tasksLinkedList.Count - 1;
            while (tasksLinkedList.Count > 0)
            {
                int minPerformTime = tasksLinkedList.SelectMany(task => task.PerformTimes).Min();
                Task minByPerformTime = tasksLinkedList.First(task => task.PerformTimes.Contains(minPerformTime));
                if (minByPerformTime.PerformTimes[0] < minByPerformTime.PerformTimes[1])
                    permutation[i++] = minByPerformTime;
                else
                    permutation[j--] = minByPerformTime;
                tasksLinkedList.Remove(minByPerformTime);
            }

            return FSPTimes.Calculate(permutation.ToList());
        }
    }
}