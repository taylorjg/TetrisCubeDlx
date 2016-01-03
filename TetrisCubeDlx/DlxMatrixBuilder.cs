using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class DlxMatrixBuilder
    {
        public static IImmutableList<DlxMatrixRow> BuildDlxMatrix(IEnumerable<InternalRow> internalRows)
        {
            var dictionary = BuildPieceNameToPieceIndexDictionary(Puzzle.Pieces);

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
            var bits = new int[numPieces + Puzzle.CubeSizeCubed];
            bits[pieceColumnIndex] = 1;
            foreach (var occupiedSquare in internalRow.OccupiedSquares)
            {
                var occupiedSquareIndex =
                    numPieces +
                    occupiedSquare.X +
                    occupiedSquare.Y*Puzzle.CubeSize +
                    occupiedSquare.Z*Puzzle.CubeSizeSquared;
                bits[occupiedSquareIndex] = 1;
            }
            return new DlxMatrixRow(bits, internalRow);
        }
    }
}
