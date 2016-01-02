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

            var xCorrection = Math.Min(transformedDimensions.X, 0);
            var yCorrection = Math.Min(transformedDimensions.Y, 0);
            var zCorrection = Math.Min(transformedDimensions.Z, 0);

            _correctionCoords = new Coords(xCorrection, yCorrection, zCorrection);
        }

        public string Name => _piece.Name;
        public Colour Colour => _piece.Colour;

        public IEnumerable<Coords> OccupiedSquares =>
            _piece.OccupiedSquares
                .Select(_rotationMatrix.Multiply)
                .Select(coords => coords - _correctionCoords);

        private static readonly IDictionary<Rotation, Matrix> RotationToMatrixDictionary = new Dictionary<Rotation, Matrix>
        {
            {Rotation.X0Cw, Matrix.Identity},
            {Rotation.X90Cw, Matrix.X90Cw},
            {Rotation.X180Cw, Matrix.X180Cw},
            {Rotation.X270Cw, Matrix.X270Cw},

            {Rotation.Y0Cw, Matrix.Identity},
            {Rotation.Y90Cw, Matrix.Y90Cw},
            {Rotation.Y180Cw, Matrix.Y180Cw},
            {Rotation.Y270Cw, Matrix.Y270Cw},

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
        private readonly Coords _correctionCoords;
    }
}
