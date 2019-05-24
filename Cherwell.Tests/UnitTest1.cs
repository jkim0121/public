using Cherwell.Controllers;
using NUnit.Framework;

namespace Cherwell.Tests
{
    [TestFixture]
    public class ValuesControllerTests
    {
        private ValuesController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            _controller = new ValuesController();
        }

        [TestCase]
        public void GetTriangleTest()
        {
            var result = _controller.GetTriangleCore("C9");
        }

        [TestCase]
        public void GetCoordinateTest()
        {
            //        [ { "x" : x1, "y" : y1 }, { "x" : x2, "y" : y2 }, { "x" : x3, "y" : y3 } ]
            var input = "[ { \"x\" : 10, \"y\" : 10 }, { \"x\" : 10, \"y\" : 20 }, { \"x\" : 20, \"y\" : 20 } ]";
            var result = _controller.GetCoordinateCore(input);
        }
    }
}