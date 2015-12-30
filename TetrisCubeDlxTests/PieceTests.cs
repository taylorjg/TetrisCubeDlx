using System.Linq;
using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class PieceTests
    {
        private string[][] _initStrings;

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

            _initStrings = new[]
            {
                slice0,
                slice1,
                slice2
            };
        }

        [Test]
        public void PieceConstructedFromInitStringsHasCorrectDimensions()
        {
            var piece = new Piece(_initStrings);

            Assert.That(piece.Width, Is.EqualTo(2), "Width");
            Assert.That(piece.Height, Is.EqualTo(2), "Height");
            Assert.That(piece.Depth, Is.EqualTo(3), "Depth");
        }

        [Test]
        public void PieceConstructedFromInitStringsHasCorrectSquares()
        {
            var piece = new Piece(_initStrings);

            var hasSquares = new[]
            {
                new Coords(1, 0, 0),
                new Coords(1, 0, 1),
                new Coords(1, 0, 2),
                new Coords(0, 0, 2),
                new Coords(0, 1, 2)
            };

            var doesNotHaveSquares = piece.AllSquares.Except(hasSquares);

            Assert.That(hasSquares, Is.All.Matches<Coords>(piece.HasSquareAt), "hasSquares");
            Assert.That(doesNotHaveSquares, Is.All.Matches<Coords>(coords => !piece.HasSquareAt(coords)), "doesNotHaveSquares");
        }
    }
}
