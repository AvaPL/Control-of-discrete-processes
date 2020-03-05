using System;
using System.IO;
using NUnit.Framework;
using RPQ;

namespace RPQTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldReadFileCorrectly()
        {
            StreamReader streamReader = new StreamReader(@"../../../Debug/data10.txt");
        }
    }
}