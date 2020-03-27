using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace RPQ
{
    public class Carlier
    {
        private static List<Task> optimalOrder;
        private static int? upperBound;

        public static List<Task> Solve(List<Task> unorderedTasks)
        {
            List<Task> schrageTasks = SchrageWithQueue.Solve(unorderedTasks);
            RPQTimes rpqTimes = RPQTimes.Calculate(schrageTasks);
            int maxQuitTime = rpqTimes.GetMaxQuitTime();

            if (!upperBound.HasValue || maxQuitTime < upperBound)
                UpdateOptimalOrderAndUpperBound(schrageTasks, maxQuitTime);

            int lastCriticalTaskIndex = CalculateLastCriticalTaskIndex(schrageTasks, rpqTimes, maxQuitTime);
            int firstCriticalTaskIndex =
                CalculateFirstCriticalTaskIndex(schrageTasks, lastCriticalTaskIndex, maxQuitTime);
            int? currentTaskIndex =
                CalculateCurrentTaskIndex(schrageTasks, firstCriticalTaskIndex, lastCriticalTaskIndex);

            if (!currentTaskIndex.HasValue)
                return optimalOrder;

            List<Task> buffer = schrageTasks.GetRange(currentTaskIndex.Value + 1,
                lastCriticalTaskIndex - currentTaskIndex.Value);
            int minReadyTime = buffer.Select(t => t.ReadyTime).Min();
            int minQuitTime = buffer.Select(t => t.QuitTime).Min();
            int sumPerformTime = buffer.Select(t => t.PerformTime).Sum();

            Task oldTask = schrageTasks[currentTaskIndex.Value];
            schrageTasks[currentTaskIndex.Value] = new Task(Math.Max(oldTask.ReadyTime, minReadyTime + sumPerformTime),
                oldTask.PerformTime, oldTask.QuitTime);
            int lowerBound = InterruptedSchrage.Solve(schrageTasks);
            if (lowerBound < upperBound)
                Solve(schrageTasks);
            schrageTasks[currentTaskIndex.Value] = oldTask;

            oldTask = schrageTasks[currentTaskIndex.Value];
            schrageTasks[currentTaskIndex.Value] = new Task(oldTask.ReadyTime, oldTask.PerformTime,
                Math.Max(oldTask.QuitTime, minQuitTime + sumPerformTime));
            lowerBound = InterruptedSchrage.Solve(schrageTasks);
            if (lowerBound < upperBound)
                Solve(schrageTasks);
            schrageTasks[currentTaskIndex.Value] = oldTask;

            return optimalOrder;
        }


        private static int CalculateLastCriticalTaskIndex(List<Task> schrageTasks, RPQTimes rpqTimes, int maxQuitTime)
        {
            return schrageTasks.Select((task, i) => new {task, i})
                .Where(t => rpqTimes.CompleteTimes[t.i] + t.task.QuitTime == maxQuitTime).Select(t => t.i).Max();
        }

        private static int CalculateFirstCriticalTaskIndex(List<Task> schrageTasks, int lastCriticalTaskIndex,
            int maxQuitTime)
        {
            return schrageTasks.Select((task, i) => new {task, i}).Where(t => t.i <= lastCriticalTaskIndex).Where(t =>
                t.task.ReadyTime +
                schrageTasks.GetRange(t.i, lastCriticalTaskIndex - t.i + 1).Select(t => t.PerformTime).Sum() +
                schrageTasks[lastCriticalTaskIndex].QuitTime == maxQuitTime).Select(t => t.i).Min();
        }

        private static int? CalculateCurrentTaskIndex(List<Task> schrageTasks, int firstCriticalTaskIndex,
            int lastCriticalTaskIndex)
        {
            return schrageTasks.Select((task, i) => new {task, i}).Where(t =>
                firstCriticalTaskIndex <= t.i && t.i < lastCriticalTaskIndex &&
                t.task.QuitTime < schrageTasks[lastCriticalTaskIndex].QuitTime).Select(t => (int?) t.i).Max();
        }

        private static void UpdateOptimalOrderAndUpperBound(List<Task> schrageTasks, int maxQuitTime)
        {
            upperBound = maxQuitTime;
            optimalOrder = schrageTasks;
        }
    }
}