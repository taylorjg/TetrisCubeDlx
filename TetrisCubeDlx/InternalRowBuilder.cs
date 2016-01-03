using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class InternalRowBuilder
    {
        public static IImmutableList<InternalRow> BuildInternalRows(IEnumerable<Piece> pieces)
        {
            var allLocations = (
                from x in Enumerable.Range(0, 4)
                from y in Enumerable.Range(0, 4)
                from z in Enumerable.Range(0, 4)
                select new Coords(x, y, z))
                .ToImmutableList();

            return (
                from piece in pieces
                from rotatedPiece in UniqueRotations.OfPiece(piece)
                from location in allLocations
                let placedPiece = new PlacedPiece(rotatedPiece, location)
                where placedPiece.IsWithinCube()
                select new InternalRow(placedPiece))
                .ToImmutableList();
        }

        private static bool IsWithinCube(this PlacedPiece placedPiece)
        {
            return placedPiece.OccupiedSquares.All(IsWithinCube);
        }

        private static bool IsWithinCube(this Coords coords)
        {
            return
                coords.X.IsWithinCube() &&
                coords.Y.IsWithinCube() &&
                coords.Z.IsWithinCube();
        }

        private static bool IsWithinCube(this int dimension)
        {
            return
                dimension >= 0 &&
                dimension <= 3;
        }
    }
}
