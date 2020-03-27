using System;
using System.Collections.Generic;
using System.Linq;

namespace RPQ
{
    public class Carlier
    {
        private List<Task> optimalOrder;
        private int? upperBound;

        public static List<Task> Solve(List<Task> unorderedTasks)
        {
            return new Carlier().GetOptimalOrder(unorderedTasks);
        }

        private List<Task> GetOptimalOrder(List<Task> unorderedTasks)
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
            ModifyTaskPerformTime(schrageTasks, currentTaskIndex.Value, minReadyTime, sumPerformTime);
            ModifyTaskQuitTime(schrageTasks, currentTaskIndex.Value, minQuitTime, sumPerformTime);

            return optimalOrder;
        }

        private void UpdateOptimalOrderAndUpperBound(List<Task> schrageTasks, int maxQuitTime)
        {
            upperBound = maxQuitTime;
            optimalOrder = schrageTasks;
        }

        private static int CalculateLastCriticalTaskIndex(List<Task> schrageTasks, RPQTimes rpqTimes, int maxQuitTime)
        {
            return schrageTasks.Select((task, i) => new {task, i})
                .Where(t => rpqTimes.CompleteTimes[t.i] + t.task.QuitTime == maxQuitTime)
                .Select(t => t.i).Max();
        }

        private static int CalculateFirstCriticalTaskIndex(List<Task> schrageTasks, int lastCriticalTaskIndex,
            int maxQuitTime)
        {
            return schrageTasks.Select((task, i) => new {task, i}).Where(t => t.i <= lastCriticalTaskIndex)
                .Where(t => t.task.ReadyTime + SumPerformTimeRange(schrageTasks, t.i, lastCriticalTaskIndex) +
                    schrageTasks[lastCriticalTaskIndex].QuitTime == maxQuitTime).Select(t => t.i).Min();
        }

        private static int SumPerformTimeRange(List<Task> schrageTasks, int startIndex, int lastCriticalTaskIndex)
        {
            return schrageTasks.GetRange(startIndex, lastCriticalTaskIndex - startIndex + 1)
                .Select(t => t.PerformTime).Sum();
        }

        private static int? CalculateCurrentTaskIndex(List<Task> schrageTasks, int firstCriticalTaskIndex,
            int lastCriticalTaskIndex)
        {
            return schrageTasks.Select((task, i) => new {task, i})
                .Where(t => firstCriticalTaskIndex <= t.i && t.i < lastCriticalTaskIndex &&
                            t.task.QuitTime < schrageTasks[lastCriticalTaskIndex].QuitTime)
                .Select(t => (int?) t.i).Max();
        }

        private void ModifyTaskPerformTime(List<Task> schrageTasks, int currentTaskIndex, int minReadyTime,
            int sumPerformTime)
        {
            Task oldTask = schrageTasks[currentTaskIndex];
            int newReadyTime = Math.Max(oldTask.ReadyTime, minReadyTime + sumPerformTime);
            schrageTasks[currentTaskIndex] = new Task(newReadyTime, oldTask.PerformTime, oldTask.QuitTime);
            int lowerBound = InterruptedSchrage.Solve(schrageTasks);
            if (lowerBound < upperBound)
                GetOptimalOrder(schrageTasks);
            schrageTasks[currentTaskIndex] = oldTask;
        }

        private void ModifyTaskQuitTime(List<Task> schrageTasks, int currentTaskIndex, int minQuitTime,
            int sumPerformTime)
        {
            Task oldTask = schrageTasks[currentTaskIndex];
            int newQuitTime = Math.Max(oldTask.QuitTime, minQuitTime + sumPerformTime);
            schrageTasks[currentTaskIndex] = new Task(oldTask.ReadyTime, oldTask.PerformTime, newQuitTime);
            int lowerBound = InterruptedSchrage.Solve(schrageTasks);
            if (lowerBound < upperBound)
                GetOptimalOrder(schrageTasks);
            schrageTasks[currentTaskIndex] = oldTask;
        }
    }
}