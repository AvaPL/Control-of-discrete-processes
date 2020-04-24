using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FSP
{
    public class TaskReader
    {
        public List<Task> ReadTasksFromFile(StreamReader file)
        {
            string buffer;
            Regex regex = new Regex(@"\s+");
            int numberOfTasks = int.Parse(regex.Split(ReadLine(file))[0]);
            List<Task> result = new List<Task>(numberOfTasks);
            while ((buffer = ReadLine(file)) != null)
                result.Add(Task.Parse(buffer));
            return result;
        }

        private static string ReadLine(StreamReader file)
        {
            return file.ReadLine()?.Trim();
        }
    }
}