using System;
using System.Collections.Generic;
using System.Linq;

namespace RPQ
{
    public class RpqTimes
    {
        public List<int> StartTimes { get; }
        public List<int> CompleteTimes { get; }
        public List<int> QuitTimes { get; }

        private RpqTimes(int numberOfTasks)
        {
            StartTimes = new List<int>(numberOfTasks);
            CompleteTimes = new List<int>(numberOfTasks);
            QuitTimes = new List<int>(numberOfTasks);
        }

        public static RpqTimes Calculate(List<Task> tasks)
        {
            RpqTimes result = new RpqTimes(tasks.Count);
            for (int i = 0; i < tasks.Count; i++)
            {
                result.StartTimes[i] =
                    i - 1 < 0 ? tasks[i].ReadyTime : Math.Max(tasks[i].ReadyTime, result.CompleteTimes[i - 1]);
                result.CompleteTimes[i] = result.StartTimes[i] + tasks[i].PerformTime;
                result.QuitTimes[i] = result.CompleteTimes[i] + tasks[i].QuitTime;
            }

            return result;
        }

        public int GetMaxQuitTime()
        {
            return QuitTimes.Max();
        }
    }
}