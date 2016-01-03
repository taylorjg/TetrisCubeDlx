using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class InternalRowBuilder
    {
        public static IImmutableList<InternalRow> BuildInternalRows(IPuzzle puzzle)
        {
            var allLocations = (
                from x in puzzle.AscendingDimensionIndices
                from y in puzzle.AscendingDimensionIndices
                from z in puzzle.AscendingDimensionIndices
                select new Coords(x, y, z))
                .ToImmutableList();

            return (
                from piece in puzzle.Pieces
                from rotatedPiece in UniqueRotations.OfPiece(piece)
                from location in allLocations
                let placedPiece = new PlacedPiece(rotatedPiece, location)
                where placedPiece.IsWithinPuzzle(puzzle)
                select new InternalRow(placedPiece))
                .ToImmutableList();
        }

        private static bool IsWithinPuzzle(this PlacedPiece placedPiece, IPuzzle puzzle)
        {
            return placedPiece.OccupiedSquares.All(sq => sq.IsWithinPuzzle(puzzle));
        }

        private static bool IsWithinPuzzle(this Coords coords, IPuzzle puzzle)
        {
            return
                coords.X.IsWithinPuzzle(puzzle) &&
                coords.Y.IsWithinPuzzle(puzzle) &&
                coords.Z.IsWithinPuzzle(puzzle);
        }

        private static bool IsWithinPuzzle(this int dimension, IPuzzle puzzle)
        {
            return
                dimension >= 0 &&
                dimension < puzzle.CubeSize;
        }
    }
}
