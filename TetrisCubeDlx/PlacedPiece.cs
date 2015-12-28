using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class PlacedPiece
    {
        public PlacedPiece(Piece piece, Orientation orientation)
        {
            _piece = piece;
            _orientation = orientation;
            _transform = Matrix.Identity;

            var dimensions = new Coords(_piece.Width, _piece.Height, _piece.Depth);
            var transformedDimensions = _transform.Multiply(dimensions);

            Width = Math.Abs(transformedDimensions.X);
            Height = Math.Abs(transformedDimensions.Y);
            Depth = Math.Abs(transformedDimensions.Z);
        }

        private readonly Piece _piece;
        private readonly Orientation _orientation;
        private readonly Matrix _transform;

        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public IEnumerable<Coords> AllSquares =>
            from x in Enumerable.Range(0, Width)
            from y in Enumerable.Range(0, Height)
            from z in Enumerable.Range(0, Depth)
            select new Coords(x, y, z);

        public bool HasSquareAt(Coords coords)
        {
            // Need inverse transform here ???
            var transformedCoords = _transform.Multiply(coords);
            return _piece.HasSquareAt(transformedCoords);
        }
    }
}
