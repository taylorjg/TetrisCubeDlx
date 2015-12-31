using System.Linq;
using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class RotatedPieceTests
    {
        private Piece _piece;

        [SetUp]
        public void SetUp()
        {
            var slice0 = new[]
            {
                "  ",
                " X"
            };

            var slice1 = new[]
            {
                "  ",
                " X"
            };

            var slice2 = new[]
            {
                "X ",
                "XX"
            };

            var initStrings = new[]
            {
                slice0,
                slice1,
                slice2
            };

            _piece = new Piece(initStrings);
        }

        [Test]
        public void NoRotation()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.Normal);

            AssertRotatedPiece(
                rotatedPiece,
                2,
                2,
                3,
                new[]
                {
                    new Coords(1, 0, 0),
                    new Coords(1, 0, 1),
                    new Coords(1, 0, 2),
                    new Coords(0, 0, 2),
                    new Coords(0, 1, 2)
                });
        }

        [Test]
        public void RotatedZ90Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.Z90Cw);

            AssertRotatedPiece(
                rotatedPiece,
                2,
                2,
                3,
                new[]
                {
                    new Coords(0, 0, 0),
                    new Coords(0, 0, 1),
                    new Coords(0, 0, 2),
                    new Coords(0, 1, 2),
                    new Coords(1, 1, 2)
                });
        }

        [Test]
        public void RotatedZ180Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.Z180Cw);

            AssertRotatedPiece(
                rotatedPiece,
                2,
                2,
                3,
                new[]
                {
                    new Coords(0, 1, 0),
                    new Coords(0, 1, 1),
                    new Coords(0, 1, 2),
                    new Coords(1, 1, 2),
                    new Coords(1, 0, 2)
                });
        }

        [Test]
        public void RotatedZ270Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.Z270Cw);

            AssertRotatedPiece(
                rotatedPiece,
                2,
                2,
                3,
                new[]
                {
                    new Coords(1, 1, 0),
                    new Coords(1, 1, 1),
                    new Coords(1, 1, 2),
                    new Coords(1, 0, 2),
                    new Coords(0, 0, 2)
                });
        }

        [Test]
        public void RotatedX90Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.X90Cw);

            AssertRotatedPiece(
                rotatedPiece,
                2,
                3,
                2,
                new[]
                {
                    new Coords(0, 2, 0),
                    new Coords(0, 2, 1),
                    new Coords(1, 2, 1),
                    new Coords(1, 1, 1),
                    new Coords(1, 0, 1)
                });
        }

        private static void AssertRotatedPiece(
            RotatedPiece rotatedPiece,
            int width,
            int height,
            int depth,
            Coords[] hasSquaresAt)
        {
            Assert.That(rotatedPiece.Width, Is.EqualTo(width), "width");
            Assert.That(rotatedPiece.Height, Is.EqualTo(height), "height");
            Assert.That(rotatedPiece.Depth, Is.EqualTo(depth), "depth");

            var doesNotHaveSquaresAt = rotatedPiece.AllSquares.Except(hasSquaresAt);

            Assert.That(hasSquaresAt, Is.All.Matches<Coords>(rotatedPiece.HasSquareAt), "hasSquaresAt");
            Assert.That(doesNotHaveSquaresAt, Is.All.Matches<Coords>(coords => !rotatedPiece.HasSquareAt(coords)), "doesNotHaveSquaresAt");
        }
    }
}
