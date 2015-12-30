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

            AssertPlacedPiece(
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

            AssertPlacedPiece(
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

            AssertPlacedPiece(
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

            AssertPlacedPiece(
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

        private void AssertPlacedPiece(
            RotatedPiece rotatedPiece,
            int width,
            int height,
            int depth,
            Coords[] hasSquares)
        {
            Assert.That(rotatedPiece.Width, Is.EqualTo(width), "width");
            Assert.That(rotatedPiece.Height, Is.EqualTo(height), "height");
            Assert.That(rotatedPiece.Depth, Is.EqualTo(depth), "depth");

            var doesNotHaveSquares = rotatedPiece.AllSquares.Except(hasSquares);

            Assert.That(hasSquares, Is.All.Matches<Coords>(rotatedPiece.HasSquareAt), "hasSquares");
            Assert.That(doesNotHaveSquares, Is.All.Matches<Coords>(coords => !rotatedPiece.HasSquareAt(coords)), "doesNotHaveSquares");
        }
    }
}
