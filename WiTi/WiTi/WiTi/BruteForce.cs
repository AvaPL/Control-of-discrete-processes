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
    }
}