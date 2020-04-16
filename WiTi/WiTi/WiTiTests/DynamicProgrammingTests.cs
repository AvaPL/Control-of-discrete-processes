using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WiTi;

namespace WiTiTests
{
    [TestFixture]
    public class DynamicProgrammingTests
    {
        private readonly string[] filePaths =
        {
            @"../../../Data/data3.txt",
            @"../../../Data/data10.txt",
            @"../../../Data/data11.txt",
            @"../../../Data/data12.txt",
            @"../../../Data/data13.txt",
            @"../../../Data/data14.txt",
            @"../../../Data/data15.txt",
            @"../../../Data/data16.txt",
            @"../../../Data/data17.txt",
            @"../../../Data/data18.txt",
            @"../../../Data/data19.txt",
            @"../../../Data/data20.txt"
        };

        private readonly int[] expectedValues =
        {
            525,
            1004,
            962,
            915,
            681,
            646,
            310,
            321,
            746,
            539,
            688,
            514
        };            
            
        [Test]
        public void ShouldGiveOptimalTotalWeightedTardinessUsingRecursion()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                Assert.AreEqual(expectedValues[i], DynamicProgramming.SolveUsingRecursion(tasks));   
            }
        }
        
        [Test]
        public void ShouldGiveOptimalTotalWeightedTardinessUsingIteration()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                using StreamReader fileReader = new StreamReader(filePaths[i]);
                TaskReader taskReader = new TaskReader();
                List<Task> tasks = taskReader.ReadTasksFromFile(fileReader);
                Assert.AreEqual(expectedValues[i], DynamicProgramming.SolveUsingIteration(tasks));   
            }
        }
    }
}