using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class InternalRowBuilder
    {
        public static IEnumerable<InternalRow> BuildInternalRows(IEnumerable<Piece> pieces)
        {
            var allLocations = (
                from x in Enumerable.Range(0, 4)
                from y in Enumerable.Range(0, 4)
                from z in Enumerable.Range(0, 4)
                select new Coords(x, y, z))
                .ToList();

            return
                from piece in pieces
                from rotatedPiece in UniqueRotations.OfPiece(piece)
                from location in allLocations
                let placedPiece = new PlacedPiece(rotatedPiece, location)
                where PlacedPieceIsWithinCube(placedPiece)
                select new InternalRow(placedPiece);
        }

        private static bool PlacedPieceIsWithinCube(PlacedPiece placedPiece)
        {
            return placedPiece.OccupiedSquares.All(OccupiedSquareIsWithinCube);
        }

        private static bool OccupiedSquareIsWithinCube(Coords coords)
        {
            return
                DimensionIsWithinCube(coords.X) &&
                DimensionIsWithinCube(coords.Y) &&
                DimensionIsWithinCube(coords.Z);
        }

        private static bool DimensionIsWithinCube(int d)
        {
            return d >= 0 && d <= 3;
        }
    }
}
