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
        public void RotatedX0Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Rotation.X0Cw);

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
        public void RotatedY0Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Rotation.Y0Cw);

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
        public void RotatedZ0Cw()
        {
            var rotatedPiece = new RotatedPiece(_piece, Rotation.Z0Cw);

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
            var rotatedPiece = new RotatedPiece(_piece, Rotation.Z90Cw);

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
            var rotatedPiece = new RotatedPiece(_piece, Rotation.Z180Cw);

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
            var rotatedPiece = new RotatedPiece(_piece, Rotation.Z270Cw);

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
            var rotatedPiece = new RotatedPiece(_piece, Rotation.X90Cw);

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
            var rotatedPiece = new RotatedPiece(_piece, Rotation.X90Cw, Rotation.Z90Cw);

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

        [TestCase(new[] {Rotation.Z90Cw, Rotation.Z90Cw, Rotation.Z90Cw, Rotation.Z90Cw})]
        [TestCase(new[] {Rotation.Z180Cw, Rotation.Z180Cw})]
        [TestCase(new[] {Rotation.Z90Cw, Rotation.Z180Cw, Rotation.Z90Cw})]
        [TestCase(new[] {Rotation.Z180Cw, Rotation.Z90Cw, Rotation.Z90Cw})]
        [TestCase(new[] {Rotation.Z90Cw, Rotation.Z90Cw, Rotation.Z180Cw})]
        [TestCase(new[] {Rotation.Z270Cw, Rotation.Z90Cw})]
        [TestCase(new[] {Rotation.Z90Cw, Rotation.Z270Cw})]
        public void RotatedZ360Cw(Rotation[] rotations)
        {
            var rotatedPiece = new RotatedPiece(_piece, rotations);
            CollectionAssert.AreEqual(_piece.OccupiedSquares(), rotatedPiece.OccupiedSquares());
        }

        private static void AssertRotatedPiece(
            RotatedPiece rotatedPiece,
            IEnumerable<Coords> occupiedSquares)
        {
            CollectionAssert.AreEquivalent(occupiedSquares, rotatedPiece.OccupiedSquares());
        }
    }
}
