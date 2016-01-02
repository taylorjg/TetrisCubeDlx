using System.Collections.Immutable;

namespace TetrisCubeDlx
{
    public class InternalRow
    {
        public InternalRow(PlacedPiece placedPiece)
        {
            Name = placedPiece.Name;
            Colour = placedPiece.Colour;
            OccupiedSquares = placedPiece.OccupiedSquares;
        }

        public string Name { get; }
        public Colour Colour { get; }
        public IImmutableList<Coords> OccupiedSquares { get; }
    }
}
