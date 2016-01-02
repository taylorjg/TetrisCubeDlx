using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class DlxMatrixBuilder
    {
        public static IEnumerable<DlxMatrixRow> BuildDlxMatrix(
            IEnumerable<InternalRow> internalRows,
            IEnumerable<Piece> pieces)
        {
            var pieceIndex = 0;
            var pieceNameToPieceIndexDictionary = pieces.ToDictionary(piece => piece.Name, _ => pieceIndex++);
            var numPieceIndexColumns = pieceNameToPieceIndexDictionary.Count;
            return internalRows.Select(internalRow =>
                InternalRowToDlxMatrixRow(internalRow, pieceNameToPieceIndexDictionary, numPieceIndexColumns));
        }

        private static DlxMatrixRow InternalRowToDlxMatrixRow(
            InternalRow internalRow,
            IReadOnlyDictionary<string, int> pieceNameToPieceIndexDictionary,
            int numPieceIndexColumns)
        {
            var pieceColumnIndex = pieceNameToPieceIndexDictionary[internalRow.Name];
            var bits = new int[numPieceIndexColumns + 64];
            bits[pieceColumnIndex] = 1;
            foreach (var occupiedSquare in internalRow.OccupiedSquares)
            {
                var squareIndex = numPieceIndexColumns + occupiedSquare.X + occupiedSquare.Y*4 + occupiedSquare.Z*16;
                bits[squareIndex] = 1;
            }
            return new DlxMatrixRow(bits, internalRow);
        }
    }
}
