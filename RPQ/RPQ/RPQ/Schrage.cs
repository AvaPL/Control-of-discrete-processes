using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Priority_Queue;

namespace RPQ
{
    public class Schrage
    {
        public static List<Task> Solve(List<Task> unorderedTasks)
        {
            SimplePriorityQueue<Task, int> unorderedTasksQueue = new SimplePriorityQueue<Task, int>();
            foreach (var task in unorderedTasks)
            {
                unorderedTasksQueue.Enqueue(task, task.ReadyTime);
            }

            //List<Task> readyTasks = new List<Task>();
            SimplePriorityQueue<Task, int> readyTasksQueue = new SimplePriorityQueue<Task, int>();
            List<Task> orderedTasks = new List<Task>(unorderedTasks.Count);
            //int time = MinReadyTime(unorderedTasks);
            int time = unorderedTasksQueue.First.ReadyTime;

            while (readyTasksQueue.Count > 0 || unorderedTasksQueue.Count > 0)
            {
                Task task;
                while (unorderedTasksQueue.Count > 0 && unorderedTasksQueue.First.ReadyTime <= time)
                {
                    //task = unorderedTasks.MinBy(t =>t.ReadyTime).First();
                    task = unorderedTasksQueue.Dequeue();
                    //readyTasks.Add(task);
                    readyTasksQueue.Enqueue(task, -task.QuitTime);
                    //unorderedTasks.Remove(task);
                }

                if (readyTasksQueue.Count > 0)
                {
                    //task = readyTasksQueue.MaxBy(t => t.QuitTime).First();
                    task = readyTasksQueue.Dequeue();
                    //readyTasksQueue.Remove(task);
                    orderedTasks.Add(task);
                    time += task.PerformTime;
                }
                else
                {
                    //time = MinReadyTime(unorderedTasks);
                    time = unorderedTasksQueue.First.ReadyTime;
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