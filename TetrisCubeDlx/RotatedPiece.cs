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
            _transform = CalculateTransform(_piece.Width);

            var dimensions = new Coords(_piece.Width, _piece.Height, _piece.Depth);

            //var transformedDimensions = _transform.Multiply(dimensions);
            //Width = Math.Abs(transformedDimensions.X);
            //Height = Math.Abs(transformedDimensions.Y);
            //Depth = Math.Abs(transformedDimensions.Z);

            Width = _piece.Width;
            Height = _piece.Height;
            Depth = _piece.Depth;
        }

        private Matrix CalculateTransform(int originalWidth)
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

                    return matrix3;
                }

                default:
                    return Matrix.Identity;
            }
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
            var transformedCoords = _transform.InverseMultiply(coords);
            return _piece.HasSquareAt(transformedCoords);
        }
    }
}
