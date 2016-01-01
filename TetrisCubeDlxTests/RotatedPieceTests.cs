using System.Collections.Generic;
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
                new[]
                {
                    new Coords(0, 2, 0),
                    new Coords(0, 2, 1),
                    new Coords(1, 2, 1),
                    new Coords(1, 1, 1),
                    new Coords(1, 0, 1)
                });
        }

        [Test]
        public void RotatedX90CwZ90Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.X90Cw, Orientation.Z90Cw);

            AssertRotatedPiece(
                rotatedPiece,
                new[]
                {
                    new Coords(0, 2, 0),
                    new Coords(1, 2, 0),
                    new Coords(0, 2, 1),
                    new Coords(0, 1, 1),
                    new Coords(0, 0, 1)
                });
        }

        private static void AssertRotatedPiece(
            RotatedPiece rotatedPiece,
            IEnumerable<Coords> occupiedSquares)
        {
            CollectionAssert.AreEquivalent(occupiedSquares, rotatedPiece.OccupiedSquares());
        }
    }
}
