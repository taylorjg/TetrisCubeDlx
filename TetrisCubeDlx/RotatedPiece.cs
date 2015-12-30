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
                    var r1 = Matrix.Z90Cw;
                    var tx = -(originalWidth - 1);
                    var t1 = Matrix.Translation(tx, 0, 0);
                    var m1 = r1;
                    var m2 = Matrix.MultiplyMatrices(r1, t1);
                    return Tuple.Create(m1, m2);
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
