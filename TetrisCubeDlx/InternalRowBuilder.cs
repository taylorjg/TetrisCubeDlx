using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class InternalRowBuilder
    {
        public static IImmutableList<InternalRow> BuildInternalRows()
        {
            var allLocations = (
                from x in Enumerable.Range(0, Puzzle.CubeSize)
                from y in Enumerable.Range(0, Puzzle.CubeSize)
                from z in Enumerable.Range(0, Puzzle.CubeSize)
                select new Coords(x, y, z))
                .ToImmutableList();

            return (
                from piece in Puzzle.Pieces
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
                dimension < Puzzle.CubeSize;
        }
    }
}
