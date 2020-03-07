﻿using NUnit.Framework;
using RPQ;

namespace RpqTests
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void ShouldParseTask ()
        {
            string stringTask = "10 10 10";
            Task task = new Task(10, 10, 10);
            Assert.AreEqual(task, Task.Parse(stringTask));
        }
    }
}