using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class Johnson
    {
        private int i;
        private int j;
        private readonly Task[] permutation;
        private readonly LinkedList<Task> tasks;

        private Johnson(List<Task> tasks)
        {
            this.tasks = new LinkedList<Task>(tasks);
            permutation = new Task[tasks.Count];
            i = 0;
            j = tasks.Count - 1;
        }

        public static FSPTimes Solve(List<Task> tasks)
        {
            Johnson johnson = new Johnson(tasks);
            return johnson.Solve();
        }

        private FSPTimes Solve()
        {
            while (tasks.Count > 0)
                ProcessTask();
            return FSPTimes.Calculate(permutation.ToList());
        }

        private void ProcessTask()
        {
            int minPerformTime = GetMinPerformTime();
            Task minByPerformTime = GetMinByPerformTime(minPerformTime);
            PutTaskIntoPermutation(minByPerformTime);
            tasks.Remove(minByPerformTime);
        }

        private int GetMinPerformTime()
        {
            return tasks.SelectMany(task => task.PerformTimes).Min();
        }

        private Task GetMinByPerformTime(int minPerformTime)
        {
            return tasks.First(task => task.PerformTimes.Contains(minPerformTime));
        }

        private void PutTaskIntoPermutation(Task minByPerformTime)
        {
            if (minByPerformTime.PerformTimes[0] < minByPerformTime.PerformTimes[1])
                permutation[i++] = minByPerformTime;
            else
                permutation[j--] = minByPerformTime;
        }
    }
}