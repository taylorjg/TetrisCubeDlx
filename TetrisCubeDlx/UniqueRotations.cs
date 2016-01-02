using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class UniqueRotations
    {
        public static IEnumerable<RotatedPiece> OfPiece(Piece piece)
        {
            var setsOfRotations = new[]
            {
                new [] {Rotation.X90Cw},
                new [] {Rotation.X180Cw},
                new [] {Rotation.X270Cw},
                new [] {Rotation.Y90Cw},
                new [] {Rotation.Y180Cw},
                new [] {Rotation.Y270Cw},
                new [] {Rotation.X90Cw},
                new [] {Rotation.X180Cw},
                new [] {Rotation.X270Cw}
            };

            return setsOfRotations
                .Select(rotations => new RotatedPiece(piece, rotations))
                .Distinct(new RotatedPieceEqualityComparer());
        }

        private class RotatedPieceEqualityComparer : IEqualityComparer<RotatedPiece>
        {
            public bool Equals(RotatedPiece rotatedPiece1, RotatedPiece rotatedPiece2)
            {
                var occupiedSquares1 = rotatedPiece1.OccupiedSquares().ToList();
                var occupiedSquares2 = rotatedPiece2.OccupiedSquares().ToList();

                return
                    IsEmpty(occupiedSquares1.Except(occupiedSquares2)) &&
                    IsEmpty(occupiedSquares2.Except(occupiedSquares1));
            }

            public int GetHashCode(RotatedPiece obj)
            {
                return 0;
            }

            private static bool IsEmpty<T>(IEnumerable<T> source)
            {
                return !source.Any();
            }
        }
    }
}
