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
            _transform = CalculateRotationTransform(orientations);

            // TODO: we can probably just concatenate a translation matrix instead of doing explicit correction.
            var dimensions = new Coords(_piece.Width - 1, _piece.Height - 1, _piece.Depth - 1);
            var transformedDimensions = _transform.Multiply(dimensions);
            _xCorrection = Math.Min(transformedDimensions.X, 0);
            _yCorrection = Math.Min(transformedDimensions.Y, 0);
            _zCorrection = Math.Min(transformedDimensions.Z, 0);
        }

        private static readonly IDictionary<Orientation, Matrix> OrientationToRotationMatrixDictionary = new Dictionary<Orientation, Matrix>
        {
            {Orientation.X0Cw, Matrix.Identity},
            {Orientation.X90Cw, Matrix.X90Cw},

            {Orientation.Y0Cw, Matrix.Identity},

            {Orientation.Z0Cw, Matrix.Identity},
            {Orientation.Z90Cw, Matrix.Z90Cw},
            {Orientation.Z180Cw, Matrix.Z180Cw},
            {Orientation.Z270Cw, Matrix.Z270Cw}
        };

        private static Matrix CalculateRotationTransform(IEnumerable<Orientation> orientations)
        {
            return orientations.Aggregate(
                Matrix.Identity,
                (acc, orientation) => acc.Multiply(OrientationToRotationMatrixDictionary[orientation]));
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
