using System;
using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class SimulatedAnnealing
    {
        private static readonly Random Random = new Random();
        private readonly Func<List<Task>, List<Task>> changePermutation;
        private readonly double endTemperature;
        private readonly int numberOfEpochs;
        private readonly Func<double, double> reduceTemperature;

        private FSPTimes bestPermutationTimes;
        private FSPTimes currentPermutationTimes;
        private double temperature;

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
                        double randomValue = Random.NextDouble();
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

        public static Func<List<Task>, List<Task>> Swap()
        {
            return Swap;
        }

        private static List<Task> Swap(List<Task> permutation)
        {
            int index1 = Random.Next(0, permutation.Count);
            int index2 = Random.Next(0, permutation.Count);
            List<Task> newPermutation = new List<Task>(permutation);
            newPermutation[index1] = permutation[index2];
            newPermutation[index2] = permutation[index1];
            return newPermutation;
        }

        public static Func<List<Task>, List<Task>> Twist()
        {
            return Twist;
        }

        private static List<Task> Twist(List<Task> permutation)
        {
            int start = Random.Next(0, permutation.Count);
            int end = Random.Next(0, permutation.Count);
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }

            IEnumerable<Task> firstPart = permutation.Take(start);
            IEnumerable<Task> secondPart = permutation.Skip(start).Take(end - start).Reverse();
            IEnumerable<Task> thirdPart = permutation.Skip(end);
            return firstPart.Concat(secondPart).Concat(thirdPart).ToList();
        }

        public static Func<double, double> ReduceTemperatureLinear(double initialTemperature)
        {
            double linearCoefficient = initialTemperature / 10e3;
            return temperature => ReduceTemperatureLinear(temperature, linearCoefficient);
        }

        private static double ReduceTemperatureLinear(double temperature, double linearCoefficient)
        {
            return temperature - linearCoefficient;
        }

        public static Func<double, double> ReduceTemperatureGeometric(double geometricCoefficient)
        {
            return temperature => ReduceTemperatureGeometric(temperature, geometricCoefficient);
        }

        private static double ReduceTemperatureGeometric(double temperature, double geometricCoefficient)
        {
            return geometricCoefficient * temperature;
        }
    }
}