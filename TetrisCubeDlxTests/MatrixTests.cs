using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void RotationAndInverse()
        {
            var matrix = new Matrix(
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            var coords1 = new Coords(2, 0, 0);
            var coords2 = matrix.Multiply(coords1);
            var coords3 = matrix.InverseMultiply(coords2);

            Assert.That(coords2, Is.Not.EqualTo(coords1));
            Assert.That(coords3, Is.EqualTo(coords1));
        }

        [Test]
        public void TranslationAndInverse()
        {
            var matrix = new Matrix(
                1, 0, 0, 3,
                0, 1, 0, 3,
                0, 0, 1, 0,
                0, 0, 0, 1);

            var coords1 = new Coords(2, 0, 0);
            var coords2 = matrix.Multiply(coords1);
            var coords3 = matrix.InverseMultiply(coords2);

            Assert.That(coords2, Is.Not.EqualTo(coords1));
            Assert.That(coords3, Is.EqualTo(coords1));
        }

        [Test]
        public void RotationThenTranslationAndInverse()
        {
            var matrix1 = new Matrix(
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            var matrix2 = new Matrix(
                1, 0, 0, -1,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            var matrix3 = matrix1.Multiply(matrix2);

            var coords1 = new Coords(1, 0, 0);
            var coords2 = matrix3.Multiply(coords1);
            var coords3 = matrix3.InverseMultiply(coords2);

            Assert.That(coords2, Is.Not.EqualTo(coords1));
            Assert.That(coords3, Is.EqualTo(coords1));
        }
    }
}
