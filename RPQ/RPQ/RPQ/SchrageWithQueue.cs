using System.Collections.Generic;
using Priority_Queue;

namespace RPQ
{
    public class SchrageWithQueue
    {
        private readonly List<Task> orderedTasks = new List<Task>();
        private readonly SimplePriorityQueue<Task, int> readyTasksQueue = new SimplePriorityQueue<Task, int>();
        private readonly SimplePriorityQueue<Task, int> unorderedTasksQueue;
        private int time;

        public SchrageWithQueue(List<Task> unorderedTasks)
        {
            unorderedTasksQueue = CreateUnorderedTasksQueue(unorderedTasks);
            time = unorderedTasksQueue.First.ReadyTime;
        }

        private static SimplePriorityQueue<Task, int> CreateUnorderedTasksQueue(List<Task> unorderedTasks)
        {
            SimplePriorityQueue<Task, int> unorderedTasksQueue = new SimplePriorityQueue<Task, int>();
            foreach (var task in unorderedTasks)
                unorderedTasksQueue.Enqueue(task, task.ReadyTime);
            return unorderedTasksQueue;
        }

        public static List<Task> Solve(List<Task> unorderedTasks)
        {
            SchrageWithQueue schrageWithQueue = new SchrageWithQueue(unorderedTasks);
            OrderTasks(schrageWithQueue);
            return schrageWithQueue.orderedTasks;
        }

        private static void OrderTasks(SchrageWithQueue schrageWithQueue)
        {
            while (schrageWithQueue.readyTasksQueue.Count > 0 || schrageWithQueue.unorderedTasksQueue.Count > 0)
            {
                EnqueueReadyTasks(schrageWithQueue);
                ProcessReadyTask(schrageWithQueue);
            }
        }

        private static void EnqueueReadyTasks(SchrageWithQueue schrageWithQueue)
        {
            while (schrageWithQueue.unorderedTasksQueue.Count > 0 && schrageWithQueue.unorderedTasksQueue.First.ReadyTime <= schrageWithQueue.time)
            {
                Task task = schrageWithQueue.unorderedTasksQueue.Dequeue();
                schrageWithQueue.readyTasksQueue.Enqueue(task, -task.QuitTime);
            }
        }

        private static void ProcessReadyTask(SchrageWithQueue schrageWithQueue)
        {
            if (schrageWithQueue.readyTasksQueue.Count > 0)
                AddOrderedTask(schrageWithQueue);
            else
                schrageWithQueue.time = schrageWithQueue.unorderedTasksQueue.First.ReadyTime;
        }

        private static void AddOrderedTask(SchrageWithQueue schrageWithQueue)
        {
            Task task = schrageWithQueue.readyTasksQueue.Dequeue();
            schrageWithQueue.orderedTasks.Add(task);
            schrageWithQueue.time += task.PerformTime;
        }
    }
}