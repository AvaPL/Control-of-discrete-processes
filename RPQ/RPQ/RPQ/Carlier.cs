using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace RPQ
{
    public class Carlier
    {
        List<Task> optimalOrder = null;
        private int? upperBound = null;

        public static List<Task> Solve(List<Task> unorderedTasks)
        {
             Carlier carlier = new Carlier();
            
             List<Task> schrageTasks = SchrageWithQueue.Solve(unorderedTasks);
             RPQTimes rpqTimes = RPQTimes.Calculate(schrageTasks);
             int maxQuitTime = rpqTimes.GetMaxQuitTime();
            
             if (!carlier.upperBound.HasValue || maxQuitTime < carlier.upperBound)
             {
                 carlier.upperBound = maxQuitTime;
                 carlier.optimalOrder = schrageTasks;
             }
            
             int lastCriticalTaskIndex = schrageTasks.Select((task, i) => new {task, i})
                 .Where(t => rpqTimes.CompleteTimes[t.i] + t.task.QuitTime == maxQuitTime).Select(t => t.i).Max();
            
             int firstCriticalTaskIndex = schrageTasks.Select((task, i) => new {task, i}).Where(t=> t.i<=lastCriticalTaskIndex).Where(t =>
                     t.task.ReadyTime + schrageTasks.GetRange(t.i, lastCriticalTaskIndex - t.i + 1)
                         .Select(t => t.PerformTime).Sum() + schrageTasks[lastCriticalTaskIndex].QuitTime ==
                     maxQuitTime)
                 .Select(t => t.i).Min();
            
             int? currentTaskIndex = schrageTasks.Select((task, i) => new {task, i}).Where(t =>
                 firstCriticalTaskIndex <= t.i && t.i < lastCriticalTaskIndex &&
                 t.task.QuitTime < schrageTasks[lastCriticalTaskIndex].QuitTime).Select(t => (int?) t.i).Max();
            
             if (!currentTaskIndex.HasValue)
                 return carlier.optimalOrder;
            
             List<Task> buffer = schrageTasks.GetRange(currentTaskIndex.Value + 1,
                 lastCriticalTaskIndex - currentTaskIndex.Value);
             int minReadyTime = buffer.Select(t => t.ReadyTime).Min();
             int minQuitTime = buffer.Select(t => t.QuitTime).Min();
             int sumPerformTime = buffer.Select(t => t.PerformTime).Sum();
            
             // int oldReadyTime = schrageTasks[currentTaskIndex.Value].ReadyTime;
             // schrageTasks[currentTaskIndex.Value].ReadyTime = Math.Max(
             //     schrageTasks[currentTaskIndex.Value].ReadyTime,
             //     minReadyTime + sumPerformTime);
             
             Task oldTask = schrageTasks[currentTaskIndex.Value];
             schrageTasks[currentTaskIndex.Value] = new Task(Math.Max(
                 oldTask.ReadyTime,
                 minReadyTime + sumPerformTime), oldTask.PerformTime, oldTask.QuitTime);
            
             int lowerBound = InterruptedSchrage.Solve(schrageTasks);
             if (lowerBound < carlier.upperBound)
                 Solve(schrageTasks);
            
             //schrageTasks[currentTaskIndex.Value].ReadyTime = oldReadyTime;

             schrageTasks[currentTaskIndex.Value] = oldTask;
             
             // int oldQuitTime = schrageTasks[currentTaskIndex.Value].QuitTime;
             // schrageTasks[currentTaskIndex.Value].QuitTime = Math.Max(
             //     schrageTasks[currentTaskIndex.Value].QuitTime,
             //     minQuitTime + sumPerformTime);

             oldTask = schrageTasks[currentTaskIndex.Value];
             schrageTasks[currentTaskIndex.Value] = new Task(oldTask.ReadyTime, oldTask.PerformTime, Math.Max(
                 oldTask.QuitTime,
                 minQuitTime + sumPerformTime));
             
             lowerBound = InterruptedSchrage.Solve(schrageTasks);
             if (lowerBound < carlier.upperBound)
                 Solve(schrageTasks);
            
             //schrageTasks[currentTaskIndex.Value].QuitTime = oldQuitTime;

             schrageTasks[currentTaskIndex.Value] = oldTask;
             
            return carlier.optimalOrder;
        }
    }
}