using System;
using Combinatorics.Collections;
using NUnit.Framework;

namespace WiTiTests
{
    [TestFixture]
    public class PermutationsTests
    {
        [Test]
        public void ShouldPrintPermutations()
        {
            int[] numbers = {1, 1, 2};
            Permutations<int> permutations = new Permutations<int>(numbers);

            foreach (var permutation in permutations) 
                Console.WriteLine(string.Join(", ", permutation));
        }
    }
}