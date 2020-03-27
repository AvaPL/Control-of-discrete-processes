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
            List<Task> unorderedTasksCopy = new List<Task>(unorderedTasks);
            List<Task> readyTasks = new List<Task>();
            List<Task> orderedTasks = new List<Task>(unorderedTasksCopy.Count);
            int time = MinReadyTime(unorderedTasksCopy);
            int maxQuitTime = 0;
            Task lastTask = null;

            while (readyTasks.Count > 0 || unorderedTasksCopy.Count > 0)
            {
                Task task;
                while (unorderedTasksCopy.Count > 0 && MinReadyTime(unorderedTasksCopy) <= time)
                {
                    task = unorderedTasksCopy.MinBy(t => t.ReadyTime).First();
                    readyTasks.Add(task);
                    unorderedTasksCopy.Remove(task);
                    if (lastTask != null && task.QuitTime > lastTask.QuitTime)
                    {
                        Task modifiedTask = new Task(lastTask.ReadyTime, time - task.ReadyTime, lastTask.QuitTime);
                        time = task.ReadyTime;
                        if (modifiedTask.PerformTime > 0)
                            readyTasks.Add(modifiedTask);
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
                    time = MinReadyTime(unorderedTasksCopy);
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