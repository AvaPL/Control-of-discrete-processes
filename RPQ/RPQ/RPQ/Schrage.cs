using System;
using System.Collections.Generic;
using MoreLinq;
using Priority_Queue;

namespace RPQ
{
    public class Schrage
    {
        private readonly List<Task> orderedTasks = new List<Task>();
        private readonly SimplePriorityQueue<Task, int> readyTasksQueue = new SimplePriorityQueue<Task, int>();
        private readonly SimplePriorityQueue<Task, int> unorderedTasksQueue;
        private int time;

        public Schrage(List<Task> unorderedTasks)
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
            Schrage schrage = new Schrage(unorderedTasks);
            OrderTasks(schrage);
            return schrage.orderedTasks;
        }

        private static void OrderTasks(Schrage schrage)
        {
            while (schrage.readyTasksQueue.Count > 0 || schrage.unorderedTasksQueue.Count > 0)
            {
                EnqueueReadyTasks(schrage);
                ProcessReadyTask(schrage);
            }
        }

        private static void EnqueueReadyTasks(Schrage schrage)
        {
            while (schrage.unorderedTasksQueue.Count > 0 && schrage.unorderedTasksQueue.First.ReadyTime <= schrage.time)
            {
                Task task = schrage.unorderedTasksQueue.Dequeue();
                schrage.readyTasksQueue.Enqueue(task, -task.QuitTime);
            }
        }

        private static void ProcessReadyTask(Schrage schrage)
        {
            if (schrage.readyTasksQueue.Count > 0)
                AddOrderedTask(schrage);
            else
                schrage.time = schrage.unorderedTasksQueue.First.ReadyTime;
        }

        private static void AddOrderedTask(Schrage schrage)
        {
            Task task = schrage.readyTasksQueue.Dequeue();
            schrage.orderedTasks.Add(task);
            schrage.time += task.PerformTime;
        }
    }
}