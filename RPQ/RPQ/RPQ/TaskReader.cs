﻿using System.Collections.Generic;
using System.IO;

namespace RPQ
{
    public class TaskReader
    {
        public List<Task> ReadTasksFromFile(StreamReader file)
        {
            string buffer;
            int numberOfTasks = int.Parse(file.ReadLine().Split(' ')[0]);
            List<Task> result = new List<Task>(numberOfTasks);
            while ((buffer = file.ReadLine()) != null)
                result.Add(Task.Parse(buffer));
            return result;
        }
    }
}