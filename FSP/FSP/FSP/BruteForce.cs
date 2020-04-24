using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace FSP
{
    public class BruteForce
    {
        public static FSPTimes SolveUsingPermutations(List<Task> tasks)
        {
            Permutations<Task> permutations = new Permutations<Task>(tasks);
            FSPTimes result = FSPTimes.Calculate(tasks);

            foreach (var permutation in permutations)
            {
                FSPTimes fspTimes = FSPTimes.Calculate(permutation.ToList());
                if (result.GetMaxCompleteTime() > fspTimes.GetMaxCompleteTime())
                    result = fspTimes;
            }

            return result;
        }

        // public static WiTiTimes SolveUsingRecursion(IEnumerable<Task> tasks)
        // {
        //     WiTiTimes result = null;
        //     foreach (var task in tasks)
        //     {
        //         LinkedList<Task> tasksCopy = new LinkedList<Task>(tasks);
        //         tasksCopy.Remove(task);
        //         WiTiTimes recursionResult = SolveUsingRecursion(tasksCopy);
        //         recursionResult.AddTask(task);
        //         if (result == null || recursionResult.TotalWeightedTardiness < result.TotalWeightedTardiness)
        //             result = recursionResult;
        //     }
        //
        //     return result ?? new WiTiTimes();
        // }
    }
}