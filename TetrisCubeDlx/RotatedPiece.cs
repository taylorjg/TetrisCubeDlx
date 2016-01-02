using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public class RotatedPiece
    {
        public RotatedPiece(Piece piece, params Rotation[] rotations)
        {
            var rotationMatrix = CalculateRotationMatrix(rotations);
            var correctionFactor = CalculateCorrectionFactor(piece, rotationMatrix);

            Name = piece.Name;
            Colour = piece.Colour;
            OccupiedSquares = piece.OccupiedSquares
                .Select(rotationMatrix.Multiply)
                .Select(coords => coords - correctionFactor)
                .ToImmutableList();
        }

        public string Name { get; }
        public Colour Colour { get; }
        public IImmutableList<Coords> OccupiedSquares { get; }

        private static readonly IDictionary<Rotation, Matrix> RotationToMatrixDictionary = new Dictionary<Rotation, Matrix>
        {
            {Rotation.X90Cw, Matrix.X90Cw},
            {Rotation.X180Cw, Matrix.X180Cw},
            {Rotation.X270Cw, Matrix.X270Cw},
            {Rotation.Y90Cw, Matrix.Y90Cw},
            {Rotation.Y180Cw, Matrix.Y180Cw},
            {Rotation.Y270Cw, Matrix.Y270Cw},
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

        private static Coords CalculateCorrectionFactor(Piece piece, Matrix rotationMatrix)
        {
            var dimensions = new Coords(piece.Width - 1, piece.Height - 1, piece.Depth - 1);
            var transformedDimensions = rotationMatrix.Multiply(dimensions);
            var xCorrection = Math.Min(transformedDimensions.X, 0);
            var yCorrection = Math.Min(transformedDimensions.Y, 0);
            var zCorrection = Math.Min(transformedDimensions.Z, 0);
            return new Coords(xCorrection, yCorrection, zCorrection);
        }
    }
}
