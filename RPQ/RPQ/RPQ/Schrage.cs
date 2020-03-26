using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace RPQ
{
    public class Schrage
    {
        public static List<Task> Solve(List<Task> unorderedTasks)
        {
            List<Task> readyTasks = new List<Task>();
            List<Task> orderedTasks = new List<Task>(unorderedTasks.Count);
            int time = MinReadyTime(unorderedTasks);

            while (readyTasks.Count > 0 || unorderedTasks.Count > 0)
            {
                Task task;
                while (unorderedTasks.Count > 0 && MinReadyTime(unorderedTasks) <= time)
                {
                    task = unorderedTasks.MinBy(t =>t.ReadyTime).First();
                    readyTasks.Add(task);
                    unorderedTasks.Remove(task); // TODO: optimize
                }

                if (readyTasks.Count > 0)
                {
                    task = readyTasks.MaxBy(t => t.QuitTime).First();
                    readyTasks.Remove(task);
                    orderedTasks.Add(task);
                    time += task.PerformTime;
                }
                else
                {
                    time = MinReadyTime(unorderedTasks);
                }
            }

            return orderedTasks;
        }

        private static int MinReadyTime(List<Task> unorderedTasks)
        {
            return unorderedTasks.Min(t => t.ReadyTime);
        }
    }
}