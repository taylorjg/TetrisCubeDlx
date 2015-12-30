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
            _orientation = orientation;

            var transforms = CalculateTransforms(_piece.Width);
            var combinedRotationsMatrix = transforms.Item1;
            _combinedRotationsAndTranslationsMatrix = transforms.Item2;

            var dimensions = new Coords(_piece.Width, _piece.Height, _piece.Depth);
            var transformedDimensions = combinedRotationsMatrix.Multiply(dimensions);

            Width = Math.Abs(transformedDimensions.X);
            Height = Math.Abs(transformedDimensions.Y);
            Depth = Math.Abs(transformedDimensions.Z);
        }

        private Tuple<Matrix, Matrix> CalculateTransforms(int originalWidth)
        {
            switch (_orientation)
            {
                case Orientation.Z90:
                {
                    var matrix1 = new Matrix(
                        0, 1, 0, 0,
                        -1, 0, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1);

                    var matrix2 = new Matrix(
                        1, 0, 0, -(originalWidth - 1),
                        0, 1, 0, 0,
                        0, 0, 1, 0,
                        0, 0, 0, 1);

                    var matrix3 = matrix1.Multiply(matrix2);

                    return Tuple.Create(matrix1, matrix3);
                }

                default:
                    return Tuple.Create(Matrix.Identity, Matrix.Identity);
            }
        }

        private readonly Piece _piece;
        private readonly Orientation _orientation;
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
