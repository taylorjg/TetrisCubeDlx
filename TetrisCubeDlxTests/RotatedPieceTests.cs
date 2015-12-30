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
        public void RotateZ90()
        {
            var rotatedPiece = new RotatedPiece(_piece, Orientation.Z90);

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

        private void AssertPlacedPiece(RotatedPiece rotatedPiece, int width, int height, int depth, Coords[] trueSquares)
        {
            //Assert.That(rotatedPiece.Width, Is.EqualTo(width), "width");
            //Assert.That(rotatedPiece.Height, Is.EqualTo(height), "height");
            //Assert.That(rotatedPiece.Depth, Is.EqualTo(depth), "depth");

            var falseSquares = rotatedPiece.AllSquares.Except(trueSquares);

            Assert.That(trueSquares, Is.All.Matches<Coords>(coords => rotatedPiece.HasSquareAt(coords)), "trueSquares");
            Assert.That(falseSquares, Is.All.Matches<Coords>(coords => !rotatedPiece.HasSquareAt(coords)), "falseSquares");
        }
    }
}
