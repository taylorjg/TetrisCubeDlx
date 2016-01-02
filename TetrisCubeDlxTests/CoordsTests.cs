using System.Collections.Generic;
using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class CoordsTests
    {
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IEnumerable<ITestCaseData> EqualsTestCaseSource()
        {
            yield return new TestCaseData(new Coords(1, 2, 3), new Coords(1, 2, 3)).SetName("Equivalent coords objects");

            var coords = new Coords(4, 5, 6);
            yield return new TestCaseData(coords, coords).SetName("Same coords objects");
        }

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public void Equals(Coords c1, Coords c2)
        {
            Assert.True(c1.Equals(c2));
            Assert.True(c2.Equals(c1));
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IEnumerable<ITestCaseData> NotEqualsTestCaseSource()
        {
            yield return new TestCaseData(new Coords(1, 2, 3), new Coords(99, 2, 3)).SetName("Different X values");
            yield return new TestCaseData(new Coords(1, 2, 3), new Coords(1, 99, 3)).SetName("Different Y values");
            yield return new TestCaseData(new Coords(1, 2, 3), new Coords(1, 2, 99)).SetName("Different Z values");
            yield return new TestCaseData(new Coords(1, 2, 3), null).SetName("Other is null");
            yield return new TestCaseData(new Coords(1, 2, 3), "Hello").SetName("Other is wrong type");
        }

        [TestCaseSource(nameof(NotEqualsTestCaseSource))]
        public void NotEquals(Coords c1, object c2)
        {
            Assert.False(c1.Equals(c2));
        }

        [Test]
        public void Addition()
        {
            var c1 = new Coords(10, 20, 30);
            var c2 = new Coords(1, 2, 3);
            var actual = c1 + c2;
            Assert.That(actual, Is.EqualTo(new Coords(10 + 1, 20 + 2, 30 + 3)));
        }

        [Test]
        public void Subtraction()
        {
            var c1 = new Coords(10, 20, 30);
            var c2 = new Coords(1, 2, 3);
            var actual = c1 - c2;
            Assert.That(actual, Is.EqualTo(new Coords(10 - 1, 20 - 2, 30 - 3)));
        }
    }
}
