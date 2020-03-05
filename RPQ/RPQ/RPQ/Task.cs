using System.Linq;

namespace RPQ
{
    public class Task
    {
        public int ReadyTime { get; }
        public int PerformTime { get; }
        public int QuitTime { get; }

        public Task(int readyTime, int performTime, int quitTime)
        {
            ReadyTime = readyTime;
            PerformTime = performTime;
            QuitTime = quitTime;
        }

        public static Task Parse(string taskString)
        {
            int[] times = taskString.Split(' ').Select(int.Parse).ToArray();
            return new Task(times[0], times[1], times[2]);
        }
    }
}