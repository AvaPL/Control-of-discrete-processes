﻿using System.Linq;
 using System.Text.RegularExpressions;

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
            Regex regex = new Regex(@"\s+");
            int[] times = regex.Split(taskString).Select(int.Parse).ToArray();
            return new Task(times[0], times[1], times[2]);
        }

        protected bool Equals(Task other)
        {
            return ReadyTime == other.ReadyTime
                   && PerformTime == other.PerformTime
                   && QuitTime == other.QuitTime;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Task)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ReadyTime;
                hashCode = (hashCode * 397) ^ PerformTime;
                hashCode = (hashCode * 397) ^ QuitTime;
                return hashCode;
            }
        }
    }
}