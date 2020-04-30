using System.Collections.Generic;
using System.Linq;

namespace FSP
{
    public class Johnson
    {
        public FSPTimes Solve(List<Task> tasks)
        {
            FSPTimes fspTimes = new FSPTimes(2);

            int i = 0;
            int j = tasks.Count-1;
            while (tasks.Count>0)
            {
                tasks.Select(t=>t.PerformTimes)
            }
        }
    }
}