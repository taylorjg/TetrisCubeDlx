using System;
using System.Collections.Generic;
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

        // X
        [TestCase("X90Cw", "X90Cw", "X90Cw", "X90Cw")]
        [TestCase("X180Cw", "X180Cw")]
        [TestCase("X180Cw", "X90Cw", "X90Cw")]
        [TestCase("X90Cw", "X180Cw", "X90Cw")]
        [TestCase("X90Cw", "X90Cw", "X180Cw")]
        [TestCase("X270Cw", "X90Cw")]
        [TestCase("X90Cw", "X270Cw")]
        // Y
        [TestCase("Y90Cw", "Y90Cw", "Y90Cw", "Y90Cw")]
        [TestCase("Y180Cw", "Y180Cw")]
        [TestCase("Y180Cw", "Y90Cw", "Y90Cw")]
        [TestCase("Y90Cw", "Y180Cw", "Y90Cw")]
        [TestCase("Y90Cw", "Y90Cw", "Y180Cw")]
        [TestCase("Y270Cw", "Y90Cw")]
        [TestCase("Y90Cw", "Y270Cw")]
        // Z
        [TestCase("Z90Cw", "Z90Cw", "Z90Cw", "Z90Cw")]
        [TestCase("Z180Cw", "Z180Cw")]
        [TestCase("Z180Cw", "Z90Cw", "Z90Cw")]
        [TestCase("Z90Cw", "Z180Cw", "Z90Cw")]
        [TestCase("Z90Cw", "Z90Cw", "Z180Cw")]
        [TestCase("Z270Cw", "Z90Cw")]
        [TestCase("Z90Cw", "Z270Cw")]
        public void FullRotations(params string[] rotationNames)
        {
            var rotationType = typeof(Rotation);
            var rotations = rotationNames.Select(rn => Enum.Parse(rotationType, rn)).Cast<Rotation>().ToArray();
            var rotatedPiece = new RotatedPiece(_piece, rotations);
            CollectionAssert.AreEqual(_piece.OccupiedSquares, rotatedPiece.OccupiedSquares);
        }

        private static void AssertRotatedPiece(
            RotatedPiece rotatedPiece,
            IEnumerable<Coords> occupiedSquares)
        {
            CollectionAssert.AreEquivalent(occupiedSquares, rotatedPiece.OccupiedSquares);
        }
    }
}
