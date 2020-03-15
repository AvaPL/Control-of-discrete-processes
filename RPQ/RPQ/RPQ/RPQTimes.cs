using System;
using System.Collections.Generic;
using System.Linq;

namespace RPQ
{
    public class RPQTimes
    {
        private RPQTimes(int numberOfTasks)
        {
            StartTimes = new int[numberOfTasks];
            CompleteTimes = new int[numberOfTasks];
            QuitTimes = new int[numberOfTasks];
        }

        public int[] StartTimes { get; }
        public int[] CompleteTimes { get; }
        public int[] QuitTimes { get; }

        public static RPQTimes Calculate(List<Task> tasks)
        {
            RPQTimes result = new RPQTimes(tasks.Count);
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