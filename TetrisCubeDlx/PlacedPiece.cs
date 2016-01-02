using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public class PlacedPiece
    {
        public PlacedPiece(RotatedPiece rotatedPiece, Coords location)
        {
            Name = rotatedPiece.Name;
            Colour = rotatedPiece.Colour;
            OccupiedSquares = rotatedPiece.OccupiedSquares
                .Select(coords => coords + location)
                .ToImmutableList();
        }

        public string Name { get; }
        public Colour Colour { get; }
        public IImmutableList<Coords> OccupiedSquares { get; }
    }
}
