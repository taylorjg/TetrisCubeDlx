using System.Collections.Generic;

namespace TetrisCubeDlx
{
    public class InternalRow
    {
        private readonly PlacedPiece _placedPiece;

        public InternalRow(PlacedPiece placedPiece)
        {
            _placedPiece = placedPiece;
        }

        public string Name => _placedPiece.Name;
        public Colour Colour => _placedPiece.Colour;
        public IEnumerable<Coords> OccupiedSquares => _placedPiece.OccupiedSquares;
    }
}
