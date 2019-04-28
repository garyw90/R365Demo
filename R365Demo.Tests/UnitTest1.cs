using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace R365Demo.Tests
{
    [TestClass]
    public class UnitTest1 : UnitTestBase
    {
        [TestMethod]
        public void EmptyString()
        {
            Add("", 0);
        }

        [TestMethod]
        public void OneNumber()
        {
            Add("1", 1);
        }

        [TestMethod]
        public void TwoNumbers()
        {
            Add("1,5", 6);
        }

        [TestMethod]
        public void ThreeNumbers()
        {
            Add("1, 5, 9", 15);
        }

        [TestMethod]
        public void MultipleCommas()
        {
            Add("1,,,,,5", 6);
        }

        [TestMethod]
        public void WithLetters()
        {
            Add("1,b", 1);
        }

        [TestMethod]
        public void FiveNumbers()
        {
            Add("1,2,3,4,5", 15);
        }

        [TestMethod]
        public void WithNewlines()
        {
            Add("1\n2,3", 6);
        }
    }
}
