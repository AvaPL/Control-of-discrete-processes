﻿using System;
using System.Collections.Generic;
using Priority_Queue;

namespace RPQ
{
    public class InterruptedSchrageWithQueue
    {
        private readonly SimplePriorityQueue<Task, int> readyTasksQueue = new SimplePriorityQueue<Task, int>();
        private readonly SimplePriorityQueue<Task, int> unorderedTasksQueue;
        private Task lastTask;
        private int time;
        private int maxQuitTime;

        private InterruptedSchrageWithQueue(List<Task> unorderedTasks)
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
            List<Task> unorderedTasksCopy = new List<Task>(unorderedTasks);
            InterruptedSchrageWithQueue schrageWithQueue = new InterruptedSchrageWithQueue(unorderedTasksCopy);
            schrageWithQueue.OrderTasks();
            return schrageWithQueue.maxQuitTime;
        }

        private void OrderTasks()
        {
            while (readyTasksQueue.Count > 0 || unorderedTasksQueue.Count > 0)
            {
                EnqueueReadyTasks();
                ProcessReadyTask();
            }
        }

        private void EnqueueReadyTasks()
        {
            while (unorderedTasksQueue.Count > 0 && unorderedTasksQueue.First.ReadyTime <= time)
            {
                Task task = unorderedTasksQueue.Dequeue();
                readyTasksQueue.Enqueue(task, -task.QuitTime);
                InterruptIfNeeded(task);
            }
        }

        private void InterruptIfNeeded(Task task)
        {
            if (lastTask != null && task.QuitTime > lastTask.QuitTime) 
                InterruptTask(task);
        }

        private void InterruptTask(Task task)
        {
            Task modifiedTask = new Task(lastTask.ReadyTime, time - task.ReadyTime, lastTask.QuitTime);
            time = task.ReadyTime;
            if (modifiedTask.PerformTime > 0)
                readyTasksQueue.Enqueue(modifiedTask, -modifiedTask.QuitTime);
        }

        private void ProcessReadyTask()
        {
            if (readyTasksQueue.Count > 0)
                AddOrderedTask();
            else
                time = unorderedTasksQueue.First.ReadyTime;
        }

        private void AddOrderedTask()
        {
            Task task = readyTasksQueue.Dequeue();
            time += task.PerformTime;
            lastTask = task;
            maxQuitTime = Math.Max(maxQuitTime, time + task.QuitTime);
        }
    }
}