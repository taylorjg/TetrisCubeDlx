using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class RotatedPiece
    {
        public RotatedPiece(Piece piece, Orientation orientation)
        {
            _piece = piece;

            var transforms = CalculateTransforms(orientation, _piece.Width, _piece.Height);
            var combinedRotationsMatrix = transforms.Item1;
            _combinedRotationsAndTranslationsMatrix = transforms.Item2;

            var dimensions = new Coords(_piece.Width, _piece.Height, _piece.Depth);
            var transformedDimensions = combinedRotationsMatrix.Multiply(dimensions);

            Width = Math.Abs(transformedDimensions.X);
            Height = Math.Abs(transformedDimensions.Y);
            Depth = Math.Abs(transformedDimensions.Z);
        }

        private static Tuple<Matrix, Matrix> CalculateTransforms(
            Orientation orientation,
            int originalWidth,
            int originalHeight)
        {
            switch (orientation)
            {
                case Orientation.Normal:
                    return Tuple.Create(Matrix.Identity, Matrix.Identity);

                case Orientation.Z90Cw:
                {
                    var r1 = Matrix.Z90Cw;
                    var tx = -(originalWidth - 1);
                    var t1 = Matrix.Translation(tx, 0, 0);
                    var m1 = r1;
                    var m2 = Matrix.MultiplyMatrices(r1, t1);
                    return Tuple.Create(m1, m2);
                }

                case Orientation.Z180Cw:
                {
                    var r1 = Matrix.Z180Cw;
                    var tx = -(originalWidth - 1);
                    var ty = -(originalHeight - 1);
                    var t1 = Matrix.Translation(tx, ty, 0);
                    var m1 = r1;
                    var m2 = Matrix.MultiplyMatrices(r1, t1);
                    return Tuple.Create(m1, m2);
                }

                case Orientation.Z270Cw:
                {
                    var r1 = Matrix.Z270Cw;
                    var ty = -(originalHeight - 1);
                    var t1 = Matrix.Translation(0, ty, 0);
                    var m1 = r1;
                    var m2 = Matrix.MultiplyMatrices(r1, t1);
                    return Tuple.Create(m1, m2);
                }

                case Orientation.X90Cw:
                {
                    var r1 = Matrix.X90Cw;
                    var ty = -(originalHeight - 1);
                    var t1 = Matrix.Translation(0, ty, 0);
                    var m1 = r1;
                    var m2 = Matrix.MultiplyMatrices(r1, t1);
                    return Tuple.Create(m1, m2);
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        private readonly Piece _piece;
        private readonly Matrix _combinedRotationsAndTranslationsMatrix;

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
            var transformedCoords = _combinedRotationsAndTranslationsMatrix.InverseMultiply(coords);
            return _piece.HasSquareAt(transformedCoords);
        }
    }
}
