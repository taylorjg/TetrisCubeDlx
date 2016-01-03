using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class DlxMatrixBuilder
    {
        public static IImmutableList<DlxMatrixRow> BuildDlxMatrix(
            IPuzzle puzzle,
            IEnumerable<InternalRow> internalRows)
        {
            var dictionary = BuildDictionary(puzzle.Pieces);

            return internalRows
                .Select(internalRow => InternalRowToDlxMatrixRow(puzzle, dictionary, internalRow))
                .ToImmutableList();
        }

        private static IReadOnlyDictionary<string, int> BuildDictionary(IEnumerable<Piece> pieces)
        {
            var pieceIndex = 0;
            return pieces.ToDictionary(piece => piece.Name, _ => pieceIndex++);
        }

        private static DlxMatrixRow InternalRowToDlxMatrixRow(
            IPuzzle puzzle,
            IReadOnlyDictionary<string, int> dictionary,
            InternalRow internalRow)
        {
            var pieceColumnIndex = dictionary[internalRow.Name];
            var numPieces = dictionary.Count;
            var bits = new int[numPieces + puzzle.CubeSizeCubed];
            bits[pieceColumnIndex] = 1;
            foreach (var occupiedSquare in internalRow.OccupiedSquares)
            {
                var occupiedSquareIndex =
                    numPieces +
                    occupiedSquare.X +
                    occupiedSquare.Y*puzzle.CubeSize +
                    occupiedSquare.Z*puzzle.CubeSizeSquared;
                bits[occupiedSquareIndex] = 1;
            }
            return new DlxMatrixRow(bits, internalRow);
        }
    }
}
