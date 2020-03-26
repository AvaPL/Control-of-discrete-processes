using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace RPQ
{
    public class InterruptedSchrage
    {
        public static int Solve(List<Task> unorderedTasks)
        {
            List<Task> readyTasks = new List<Task>();
            List<Task> orderedTasks = new List<Task>(unorderedTasks.Count);
            int time = MinReadyTime(unorderedTasks);
            int maxQuitTime = 0;
            Task lastTask = null;

            while (readyTasks.Count > 0 || unorderedTasks.Count > 0)
            {
                Task task;
                while (unorderedTasks.Count > 0 && MinReadyTime(unorderedTasks) <= time)
                {
                    task = unorderedTasks.MinBy(t => t.ReadyTime).First();
                    readyTasks.Add(task);
                    unorderedTasks.Remove(task);
                    if (lastTask != null && task.QuitTime > lastTask.QuitTime)
                    {
                        lastTask.PerformTime = time - task.ReadyTime;
                        time = task.ReadyTime;
                        if (lastTask.PerformTime > 0)
                            readyTasks.Add(lastTask);
                    }
                }

                if (readyTasks.Count > 0)
                {
                    task = readyTasks.MaxBy(t => t.QuitTime).First();
                    readyTasks.Remove(task);
                    orderedTasks.Add(task);
                    time += task.PerformTime;
                    lastTask = task;
                    maxQuitTime = Math.Max(maxQuitTime, time + task.QuitTime);
                }
                else
                {
                    time = MinReadyTime(unorderedTasks);
                }
            }

            return maxQuitTime;
        }

        private static int MinReadyTime(List<Task> unorderedTasks)
        {
            return unorderedTasks.Min(t => t.ReadyTime);
        }
    }
}