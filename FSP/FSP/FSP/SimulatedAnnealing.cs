using System;
using System.Collections.Generic;

namespace FSP
{
    public class SimulatedAnnealing
    {
        private Random random = new Random();
        private double temperature;
        private double endTemperature;
        private int numberOfEpochs;
        private Func<double, double> reduceTemperature;
        private Func<List<Task>, List<Task>> changePermutation;

        private FSPTimes bestPermutationTimes;
        private FSPTimes currentPermutationTimes;

        public SimulatedAnnealing(List<Task> tasks, double initialTemperature, double endTemperature,
            int numberOfEpochs, Func<List<Task>, List<Task>> changePermutation, Func<double, double> reduceTemperature)
        {
            bestPermutationTimes = FSPTimes.Calculate(tasks);
            currentPermutationTimes = bestPermutationTimes;
            temperature = initialTemperature;
            this.endTemperature = endTemperature;
            this.numberOfEpochs = numberOfEpochs;
            this.changePermutation = changePermutation;
            this.reduceTemperature = reduceTemperature;
        }

        public FSPTimes Solve()
        {
            while (temperature > endTemperature)
            {
                for (int i = 0; i < numberOfEpochs; i++)
                {
                    List<Task> newPermutation = changePermutation.Invoke(currentPermutationTimes.Permutation);
                    FSPTimes newPermutationTimes = FSPTimes.Calculate(newPermutation);
                    if (newPermutationTimes.GetMaxCompleteTime() > currentPermutationTimes.GetMaxCompleteTime())
                    {
                        double randomValue = random.NextDouble();
                        if (randomValue >
                            Math.Exp((currentPermutationTimes.GetMaxCompleteTime() -
                                      newPermutationTimes.GetMaxCompleteTime()) / temperature))
                            newPermutationTimes = currentPermutationTimes;
                    }

                    currentPermutationTimes = newPermutationTimes;
                    if (currentPermutationTimes.GetMaxCompleteTime() < bestPermutationTimes.GetMaxCompleteTime())
                        bestPermutationTimes = currentPermutationTimes;
                }

                temperature = reduceTemperature.Invoke(temperature);
            }

            return bestPermutationTimes;
        }
    }
}