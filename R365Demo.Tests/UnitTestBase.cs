using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace R365Demo.Tests
{
    public class UnitTestBase
    {
        static readonly IUnityContainer container;

        static UnitTestBase()
        {
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container = config.Configure(new UnityContainer());
        }

        private ICalculator CalculatorInstance => container.Resolve<ICalculator>();

        protected void Add(string numbers, int expected)
        {
            int actual = CalculatorInstance.Add(numbers);
            string failureReason = $"The expected result should be {expected}, the actual result as {actual}";
            Assert.IsTrue(actual == expected, failureReason);
        }
    }
}
