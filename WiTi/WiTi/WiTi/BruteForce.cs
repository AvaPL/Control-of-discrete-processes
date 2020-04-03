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

            foreach (var permutation in permutations)
            {
                WiTiTimes wiTiTimes = new WiTiTimes(permutation);
                if (result.TotalWeightedTardiness > wiTiTimes.TotalWeightedTardiness)
                    result = wiTiTimes;
            }

            return result;
        }

        public static WiTiTimes SolveUsingRecursion(IEnumerable<Task> tasks)
        {
            WiTiTimes result = null;
            foreach (var task in tasks)
            {
                LinkedList<Task> tasksCopy = new LinkedList<Task>(tasks);
                tasksCopy.Remove(task);
                WiTiTimes recursionResult = SolveUsingRecursion(tasksCopy);
                recursionResult.AddTask(task);
                if (result == null || recursionResult.TotalWeightedTardiness < result.TotalWeightedTardiness)
                    result = recursionResult;
            }

            return result ?? new WiTiTimes();
        }
    }
}