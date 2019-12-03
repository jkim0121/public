using System;

using HelloAspCore.Controllers;
using NUnit.Framework;

namespace HelloAspCoreTests
{
    public class MainControllerTests
    {
        private MainController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new MainController();
        }

        [Test]
        public void TestPi()
        {
            var value = _controller.Pi().Value;
            Assert.AreEqual(value, Math.PI.ToString());
        }

        [Test]
        public void TestPiPrecision([Values(2, 3, 4, 5, 6)] int precision)
        {
            var value = _controller.Pi(precision).Value;
            switch (precision)
            {
                case 2:
                    Assert.AreEqual(value, "3.14");
                    break;
                case 3:
                    Assert.AreEqual(value, "3.142");
                    break;
                case 4:
                    Assert.AreEqual(value, "3.1416");
                    break;
                case 5:
                    Assert.AreEqual(value, "3.14159");
                    break;
                case 6:
                    Assert.AreEqual(value, "3.141593");
                    break;
            }
        }
    }
}