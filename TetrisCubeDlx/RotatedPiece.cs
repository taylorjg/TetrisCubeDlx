using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class RotatedPiece
    {
        public RotatedPiece(Piece piece, params Rotation[] rotations)
        {
            _piece = piece;
            _rotationMatrix = CalculateRotationMatrix(rotations);

            var dimensions = new Coords(_piece.Width - 1, _piece.Height - 1, _piece.Depth - 1);
            var transformedDimensions = _rotationMatrix.Multiply(dimensions);

            _xCorrection = -Math.Min(transformedDimensions.X, 0);
            _yCorrection = -Math.Min(transformedDimensions.Y, 0);
            _zCorrection = -Math.Min(transformedDimensions.Z, 0);
        }

        public IEnumerable<Coords> OccupiedSquares()
        {
            return _piece.OccupiedSquares()
                .Select(_rotationMatrix.Multiply)
                .Select(c =>
                    new Coords(
                        c.X + _xCorrection,
                        c.Y + _yCorrection,
                        c.Z + _zCorrection));
        }

        private static readonly IDictionary<Rotation, Matrix> RotationToMatrixDictionary = new Dictionary<Rotation, Matrix>
        {
            {Rotation.X0Cw, Matrix.Identity},
            {Rotation.X90Cw, Matrix.X90Cw},

            {Rotation.Y0Cw, Matrix.Identity},

            {Rotation.Z0Cw, Matrix.Identity},
            {Rotation.Z90Cw, Matrix.Z90Cw},
            {Rotation.Z180Cw, Matrix.Z180Cw},
            {Rotation.Z270Cw, Matrix.Z270Cw}
        };

        private static Matrix CalculateRotationMatrix(IEnumerable<Rotation> rotations)
        {
            return rotations.Aggregate(
                Matrix.Identity,
                (acc, orientation) => acc.Multiply(RotationToMatrixDictionary[orientation]));
        }

        private readonly Piece _piece;
        private readonly Matrix _rotationMatrix;
        private readonly int _xCorrection;
        private readonly int _yCorrection;
        private readonly int _zCorrection;
    }
}
