using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class PlacedPiece
    {
        public PlacedPiece(RotatedPiece rotatedPiece, Coords location)
        {
            _rotatedPiece = rotatedPiece;
            _location = location;
        }

        private readonly RotatedPiece _rotatedPiece;
        private readonly Coords _location;

        public string Name => _rotatedPiece.Name;
        public Colour Colour => _rotatedPiece.Colour;

        public IEnumerable<Coords> OccupiedSquares()
        {
            return _rotatedPiece.OccupiedSquares().Select(coords => coords + _location);
        }
    }
}
