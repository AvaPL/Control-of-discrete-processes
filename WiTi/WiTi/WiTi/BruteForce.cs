using System;
using System.Collections.Generic;
using Combinatorics.Collections;

namespace WiTi
{
    public class BruteForce
    {
        public static WiTiTimes SolveUsingPermutations(List<Task> tasks)
        {
            Permutations<Task> permutations = new Permutations<Task>(tasks);
            WiTiTimes result = new WiTiTimes(tasks);

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (List<Task> permutation in permutations)
            {
                WiTiTimes wiTiTimes = new WiTiTimes(permutation);
                if (result.TotalWeightedTardiness > wiTiTimes.TotalWeightedTardiness)
                    result = wiTiTimes;
            }

            return result;
        }
        
        // public static WiTiTimes SolveUsingRecursion(List<Task> tasks)
        // {
        //     WiTiTimes result = new WiTiTimes(tasks);
        //     foreach (var task in tasks)
        //     {
        //         LinkedList<Task> tasksCopy = new LinkedList<Task>(tasks);
        //         WiTiTimes wiTiTimes = new WiTiTimes(tasks.Count);
        //         tasksCopy.Remove(task);
        //         wiTiTimes.AddTask(task);
        //         WiTiTimes wiTiTimesFromRecursion = SolveUsingRecursion(tasksCopy, wiTiTimes);
        //         if (result.TotalWeightedTardiness > wiTiTimesFromRecursion.TotalWeightedTardiness)
        //             result = wiTiTimes;
        //     }     
        // }
        //
        // public static WiTiTimes SolveUsingRecursion(LinkedList<Task> tasks, WiTiTimes wiTiTimes)
        // {
        //     foreach (var task in tasks)
        //     {
        //         LinkedList<Task> tasksCopy = new LinkedList<Task>(tasks);
        //         WiTiTimes wiTiTimesCopy = new WiTiTimes(wiTiTimes);
        //         tasksCopy.Remove(task);
        //         wiTiTimes.AddTask(task);
        //         SolveUsingRecursion(tasksCopy, wiTiTimesCopy);
        //     }
        // }
    }
}