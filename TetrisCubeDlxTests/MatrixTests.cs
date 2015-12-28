using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void MatrixMultiplyMatrix()
        {
            var matrix1 = new Matrix(
                3, 12, 4,
                5, 6, 8, 
                1, 0, 2);

            var matrix2 = new Matrix(
                7, 3, 8,
                11, 9, 5,
                6, 8, 4);

            var actual = matrix1.Multiply(matrix2);

            Assert.That(actual.A1, Is.EqualTo(177));
            Assert.That(actual.A2, Is.EqualTo(149));
            Assert.That(actual.A3, Is.EqualTo(100));

            Assert.That(actual.B1, Is.EqualTo(149));
            Assert.That(actual.B2, Is.EqualTo(133));
            Assert.That(actual.B3, Is.EqualTo(102));

            Assert.That(actual.C1, Is.EqualTo(19));
            Assert.That(actual.C2, Is.EqualTo(19));
            Assert.That(actual.C3, Is.EqualTo(16));
        }

        [Test]
        public void MatrixMultiplyCoords()
        {
            var matrix = new Matrix(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);

            var coords = new Coords(2, 1, 3);

            var actual = matrix.Multiply(coords);

            Assert.That(actual.X, Is.EqualTo(13));
            Assert.That(actual.Y, Is.EqualTo(31));
            Assert.That(actual.Z, Is.EqualTo(49));
        }
    }
}
