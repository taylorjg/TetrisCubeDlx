using System.Linq;
using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class UniqueRotationsTests
    {
        [Test]
        public void UnitCube()
        {
            var initStrings = new[]
            {
                new[]
                {
                    "X"
                }
            };

            var piece = new Piece(initStrings);
            Assert.That(piece.Width, Is.EqualTo(1));
            Assert.That(piece.Height, Is.EqualTo(1));
            Assert.That(piece.Depth, Is.EqualTo(1));

            var actual = UniqueRotations.OfPiece(piece);

            Assert.That(actual.Count(), Is.EqualTo(1));
        }
    }
}
