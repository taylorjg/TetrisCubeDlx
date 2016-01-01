using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class RotatedPiece
    {
        public RotatedPiece(Piece piece, params Orientation[] orientations)
        {
            _piece = piece;
            _transform = CalculateTransform(orientations);

            // TODO: we can probably just concatenate a translation matrix instead of doing explicit correction.
            var dimensions = new Coords(_piece.Width - 1, _piece.Height - 1, _piece.Depth - 1);
            var transformedDimensions = _transform.Multiply(dimensions);
            _xCorrection = Math.Min(transformedDimensions.X, 0);
            _yCorrection = Math.Min(transformedDimensions.Y, 0);
            _zCorrection = Math.Min(transformedDimensions.Z, 0);
        }

        // TODO: replace this method with a dictionary of Orientation -> Matrix
        private static Matrix OrientationToRotationMatrix(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Normal:
                    return Matrix.Identity;

                case Orientation.Z90Cw:
                    return Matrix.Z90Cw;

                case Orientation.Z180Cw:
                    return Matrix.Z180Cw;

                case Orientation.Z270Cw:
                    return Matrix.Z270Cw;

                case Orientation.X90Cw:
                    return Matrix.X90Cw;

                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        private static Matrix CalculateTransform(IEnumerable<Orientation> orientations)
        {
            return orientations.Aggregate(
                Matrix.Identity,
                (acc, orientation) => acc.Multiply(OrientationToRotationMatrix((orientation))));
        }

        private readonly Piece _piece;
        private readonly Matrix _transform;
        private readonly int _xCorrection;
        private readonly int _yCorrection;
        private readonly int _zCorrection;

        public IEnumerable<Coords> OccupiedSquares()
        {
            return _piece.OccupiedSquares()
                .Select(_transform.Multiply)
                .Select(c => new Coords(c.X - _xCorrection, c.Y - _yCorrection, c.Z - _zCorrection));
        }
    }
}
