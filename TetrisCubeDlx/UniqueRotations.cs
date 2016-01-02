using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class UniqueRotations
    {
        private static readonly Rotation[][] SetsOfRotations =
        {
            new Rotation[] {},

            new[] {Rotation.X90Cw},
            new[] {Rotation.X180Cw},
            new[] {Rotation.X270Cw},
            new[] {Rotation.Y90Cw},
            new[] {Rotation.Y180Cw},
            new[] {Rotation.Y270Cw},
            new[] {Rotation.X90Cw},
            new[] {Rotation.X180Cw},
            new[] {Rotation.X270Cw},

            new[] {Rotation.X90Cw, Rotation.Y90Cw},
            new[] {Rotation.X90Cw, Rotation.Y180Cw},
            new[] {Rotation.X90Cw, Rotation.Y270Cw},
            new[] {Rotation.X180Cw, Rotation.Y90Cw},
            new[] {Rotation.X180Cw, Rotation.Y180Cw},
            new[] {Rotation.X180Cw, Rotation.Y270Cw},
            new[] {Rotation.X270Cw, Rotation.Y90Cw},
            new[] {Rotation.X270Cw, Rotation.Y180Cw},
            new[] {Rotation.X270Cw, Rotation.Y270Cw},

            new[] {Rotation.X90Cw, Rotation.Z90Cw},
            new[] {Rotation.X90Cw, Rotation.Z180Cw},
            new[] {Rotation.X90Cw, Rotation.Z270Cw},
            new[] {Rotation.X180Cw, Rotation.Z90Cw},
            new[] {Rotation.X180Cw, Rotation.Z180Cw},
            new[] {Rotation.X180Cw, Rotation.Z270Cw},
            new[] {Rotation.X270Cw, Rotation.Z90Cw},
            new[] {Rotation.X270Cw, Rotation.Z180Cw},
            new[] {Rotation.X270Cw, Rotation.Z270Cw},

            new[] {Rotation.Y90Cw, Rotation.X90Cw},
            new[] {Rotation.Y90Cw, Rotation.X180Cw},
            new[] {Rotation.Y90Cw, Rotation.X270Cw},
            new[] {Rotation.Y180Cw, Rotation.X90Cw},
            new[] {Rotation.Y180Cw, Rotation.X180Cw},
            new[] {Rotation.Y180Cw, Rotation.X270Cw},
            new[] {Rotation.Y270Cw, Rotation.X90Cw},
            new[] {Rotation.Y270Cw, Rotation.X180Cw},
            new[] {Rotation.Y270Cw, Rotation.X270Cw},

            new[] {Rotation.Y90Cw, Rotation.Z90Cw},
            new[] {Rotation.Y90Cw, Rotation.Z180Cw},
            new[] {Rotation.Y90Cw, Rotation.Z270Cw},
            new[] {Rotation.Y180Cw, Rotation.Z90Cw},
            new[] {Rotation.Y180Cw, Rotation.Z180Cw},
            new[] {Rotation.Y180Cw, Rotation.Z270Cw},
            new[] {Rotation.Y270Cw, Rotation.Z90Cw},
            new[] {Rotation.Y270Cw, Rotation.Z180Cw},
            new[] {Rotation.Y270Cw, Rotation.Z270Cw},

            new[] {Rotation.Z90Cw, Rotation.X90Cw},
            new[] {Rotation.Z90Cw, Rotation.X180Cw},
            new[] {Rotation.Z90Cw, Rotation.X270Cw},
            new[] {Rotation.Z180Cw, Rotation.X90Cw},
            new[] {Rotation.Z180Cw, Rotation.X180Cw},
            new[] {Rotation.Z180Cw, Rotation.X270Cw},
            new[] {Rotation.Z270Cw, Rotation.X90Cw},
            new[] {Rotation.Z270Cw, Rotation.X180Cw},
            new[] {Rotation.Z270Cw, Rotation.X270Cw},

            new[] {Rotation.Z90Cw, Rotation.Y90Cw},
            new[] {Rotation.Z90Cw, Rotation.Y180Cw},
            new[] {Rotation.Z90Cw, Rotation.Y270Cw},
            new[] {Rotation.Z180Cw, Rotation.Y90Cw},
            new[] {Rotation.Z180Cw, Rotation.Y180Cw},
            new[] {Rotation.Z180Cw, Rotation.Y270Cw},
            new[] {Rotation.Z270Cw, Rotation.Y90Cw},
            new[] {Rotation.Z270Cw, Rotation.Y180Cw},
            new[] {Rotation.Z270Cw, Rotation.Y270Cw},
        };

        public static IEnumerable<RotatedPiece> OfPiece(Piece piece)
        {
            return SetsOfRotations
                .Select(rotations => new RotatedPiece(piece, rotations))
                .Distinct(new RotatedPieceEqualityComparer());
        }

        private class RotatedPieceEqualityComparer : IEqualityComparer<RotatedPiece>
        {
            public bool Equals(RotatedPiece rotatedPiece1, RotatedPiece rotatedPiece2)
            {
                var occupiedSquares1 = rotatedPiece1.OccupiedSquares.ToImmutableList();
                var occupiedSquares2 = rotatedPiece2.OccupiedSquares.ToImmutableList();

                return
                    occupiedSquares1.Except(occupiedSquares2).IsEmpty() &&
                    occupiedSquares2.Except(occupiedSquares1).IsEmpty();
            }

            public int GetHashCode(RotatedPiece obj)
            {
                return 0;
            }
        }
    }
}
