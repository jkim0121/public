using NUnit.Framework;
using System;
using System.Linq;

namespace Clear
{
    public class Tests
    {
        private Measure _test
            ;
        [OneTimeSetUp]
        public void Setup()
        {
            _test = new Measure();
        }

        [OneTimeTearDown]
        public void CleanOut()
        {
            // Teardown shall be used when the testing unit uses unmanaged resource (e.g. unmanaged memory block, or file)
            _test.Dispose();
        }

        [Description("Divide by 3 cases")]
        [TestCase(3)]
        [TestCase(12)]
        [TestCase(36)]
        public void Test_Divisible_3(int upperbound)
        {
            var result = _test.FizzBuzz(upperbound);

            Assert.True(result.ElementAt(upperbound - 1) == "fizz");
            Assert.Pass();
        }

        [Description("Divide by 5 cases")]
        [TestCase(5)]
        [TestCase(20)]
        [TestCase(80)]
        public void Test_Divisible_5(int upperbound)
        {
            var result = _test.FizzBuzz(upperbound);

            Assert.True(result.ElementAt(upperbound - 1) == "buzz");
            Assert.Pass();
        }

        [Description("Divide by both 3 and 5 cases")]
        [TestCase(15)]
        [TestCase(45)]
        [TestCase(120)]
        public void Test_Divisible_3_5(int upperbound)
        {
            var result = _test.FizzBuzz(upperbound);

            Assert.True(result.ElementAt(upperbound - 1) == "fizz buzz");
            Assert.Pass();
        }

        [TestCase]
        public void Test_Negative_UpperBound()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var result = _test.FizzBuzz(-1);

                Assert.True(result.ElementAt(0) == "fizz");
                Assert.Pass();
            });
        }

        [Description("Check performance by limiting test case running time to 1 sec.")]
        [TestCase]
        [MaxTime(1000)]
        public void Test_Timeout()
        {
            var result = _test.FizzBuzz(25000).ToList();
            Assert.Pass();
        }

    }
}