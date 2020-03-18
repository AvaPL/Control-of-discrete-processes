using System;
using System.Collections.Generic;
using Priority_Queue;

namespace RPQ
{
    public class InterruptedSchrage
    {
        private readonly SimplePriorityQueue<Task, int> readyTasksQueue = new SimplePriorityQueue<Task, int>();
        private readonly SimplePriorityQueue<Task, int> unorderedTasksQueue;
        private Task lastTask;
        private int time;
        private int maxQuitTime = 0;

        public InterruptedSchrage(List<Task> unorderedTasks)
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

        public static int Solve(List<Task> unorderedTasks)
        {
            InterruptedSchrage schrage = new InterruptedSchrage(unorderedTasks);
            OrderTasks(schrage);
            return schrage.maxQuitTime;
        }

        private static void OrderTasks(InterruptedSchrage schrage)
        {
            while (schrage.readyTasksQueue.Count > 0 || schrage.unorderedTasksQueue.Count > 0)
            {
                EnqueueReadyTasks(schrage);
                ProcessReadyTask(schrage);
            }
        }

        private static void EnqueueReadyTasks(InterruptedSchrage schrage)
        {
            while (schrage.unorderedTasksQueue.Count > 0 && schrage.unorderedTasksQueue.First.ReadyTime <= schrage.time)
            {
                Task task = schrage.unorderedTasksQueue.Dequeue();
                schrage.readyTasksQueue.Enqueue(task, -task.QuitTime);
                InterruptIfNeeded(schrage, task);
            }
        }

        private static void InterruptIfNeeded(InterruptedSchrage schrage, Task task)
        {
            if (schrage.lastTask != null && task.QuitTime > schrage.lastTask.QuitTime) 
                InterruptTask(schrage, task);
        }

        private static void InterruptTask(InterruptedSchrage schrage, Task task)
        {
            schrage.lastTask.PerformTime = schrage.time - task.ReadyTime;
            schrage.time = task.ReadyTime;
            if (schrage.lastTask.PerformTime > 0)
                schrage.readyTasksQueue.Enqueue(schrage.lastTask, -schrage.lastTask.QuitTime);
        }

        private static void ProcessReadyTask(InterruptedSchrage schrage)
        {
            if (schrage.readyTasksQueue.Count > 0)
                AddOrderedTask(schrage);
            else
                schrage.time = schrage.unorderedTasksQueue.First.ReadyTime;
        }

        private static void AddOrderedTask(InterruptedSchrage schrage)
        {
            Task task = schrage.readyTasksQueue.Dequeue();
            schrage.time += task.PerformTime;
            schrage.lastTask = task;
            schrage.maxQuitTime = Math.Max(schrage.maxQuitTime, schrage.time + task.QuitTime);
        }
    }
}