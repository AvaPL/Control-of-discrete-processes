using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WiTi
{
    public class Task : IComparable<Task>
    {
        public Task(int performTime, int penaltyWeight, int deadline)
        {
            PerformTime = performTime;
            PenaltyWeight = penaltyWeight;
            Deadline = deadline;
        }

        public int PerformTime { get; }
        public int PenaltyWeight { get;}
        public int Deadline { get;}


        public int CompareTo(Task other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            int readyTimeComparison = PerformTime.CompareTo(other.PerformTime);
            if (readyTimeComparison != 0) return readyTimeComparison;
            return Deadline.CompareTo(other.Deadline);
        }

        public static Task Parse(string taskString)
        {
            Regex regex = new Regex(@"\s+");
            int[] times = regex.Split(taskString).Select(int.Parse).ToArray();
            return new Task(times[0], times[1], times[2]);
        }

        protected bool Equals(Task other)
        {
            return PerformTime == other.PerformTime
                   && PenaltyWeight == other.PenaltyWeight
                   && Deadline == other.Deadline;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Task) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = PerformTime;
                hashCode = (hashCode * 397) ^ PenaltyWeight;
                hashCode = (hashCode * 397) ^ Deadline;
                return hashCode;
            }
        }
    }
}