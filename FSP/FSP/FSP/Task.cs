using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FSP
{
    public class Task : IComparable<Task>
    {
        public Task(IEnumerable<int> performTimes)
        {
            PerformTimes = new List<int>(performTimes);
        }

        public List<int> PerformTimes { get; }

        public static Task Parse(string taskString)
        {
            Regex regex = new Regex(@"\s+");
            int[] times = regex.Split(taskString).Select(int.Parse).ToArray();
            return new Task(times.Where((t, i) => i % 2 == 1));
        }
        
        public int CompareTo(Task other)
        {
            return GetHashCode().CompareTo(other.GetHashCode());
        }
    }
}