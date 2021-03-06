﻿using System;
using System.Security.Authentication.ExtendedProtection;
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

        [TestMethod]
        public void WithDelimiter()
        {
            Add("//;\n1;2", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MissingDelimiter()
        {
            Add("//\n1;2", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TooManyDelimiters()
        {
            Add("//;;\n1;2", 3);
        }

        [TestMethod]
        public void WithNegativeNumbers()
        {
            try
            {
                Add("1, -3, -6, 8", 9);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Console.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void BigNumbers()
        {
            Add("1,5,2003", 6);
        }

        [TestMethod]
        public void LongDelimiter()
        {
            Add("//[***]\n1***2***3", 6);
        }

        [TestMethod]
        public void MultipleDelimiters()
        {
            Add("//[*][%]\n1*2%3", 6);
        }

        [TestMethod]
        public void MultipleLongDelimiters()
        {
            Add("//[***][$$$][@]\n1$$$2@3***4", 10);
        }

        [TestMethod]
        public void CalculateAdder()
        {
            Calculate(Operation.Add, "1,2,3", 6);
        }

        [TestMethod]
        public void CalculateSubstracter()
        {
            Calculate(Operation.Subtract, "10,3,2", 5);
        }

        [TestMethod]
        public void CalculateMultiplier()
        {
            Calculate(Operation.Multiply, "10,2,3", 60);
        }

        [TestMethod]
        public void CalculateDivider()
        {
            Calculate(Operation.Divide, "100,5,5", 4);
        }
    }
}
