using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class DlxMatrixBuilder
    {
        public static IImmutableList<DlxMatrixRow> BuildDlxMatrix(
            IEnumerable<InternalRow> internalRows,
            IImmutableList<Piece> pieces)
        {
            var dictionary = BuildPieceNameToPieceIndexDictionary(pieces);
            return internalRows
                .Select(internalRow => InternalRowToDlxMatrixRow(dictionary, internalRow))
                .ToImmutableList();
        }

        private static IReadOnlyDictionary<string, int> BuildPieceNameToPieceIndexDictionary(IEnumerable<Piece> pieces)
        {
            var pieceIndex = 0;
            return pieces.ToDictionary(piece => piece.Name, _ => pieceIndex++);
        }

        private static DlxMatrixRow InternalRowToDlxMatrixRow(
            IReadOnlyDictionary<string, int> dictionary,
            InternalRow internalRow)
        {
            var pieceColumnIndex = dictionary[internalRow.Name];
            var numPieces = dictionary.Count;
            var bits = new int[numPieces + 64];
            bits[pieceColumnIndex] = 1;
            foreach (var occupiedSquare in internalRow.OccupiedSquares)
            {
                var squareIndex = numPieces + occupiedSquare.X + occupiedSquare.Y*4 + occupiedSquare.Z*16;
                bits[squareIndex] = 1;
            }
            return new DlxMatrixRow(bits, internalRow);
        }
    }
}
