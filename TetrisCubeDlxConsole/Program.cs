using System;
using System.Collections.Generic;
using System.Linq;
using DlxLib;
using TetrisCubeDlx;

namespace TetrisCubeDlxConsole
{
    internal static class Program
    {
        private static void Main()
        {
            var pieces = Pieces.MakePieces();
            var internalRows = InternalRowBuilder.BuildInternalRows(pieces);
            var dlxMatrix = DlxMatrixBuilder.BuildDlxMatrix(internalRows, pieces).ToList();

            var solution = new Dlx().Solve(dlxMatrix, rows => rows, row => row.Bits).First();

            if (solution != null)
            {
                DumpSolutionSimple(solution, dlxMatrix);
            }
        }

        private static void DumpSolutionSimple(Solution solution, IReadOnlyList<DlxMatrixRow> dlxMatrix)
        {
            foreach (var rowIndex in solution.RowIndexes)
            {
                var dlxMatrixRow = dlxMatrix[rowIndex];
                var internalRow = dlxMatrixRow.InternalRow;
                Console.WriteLine($"Name: {internalRow.Name}; Colour: {internalRow.Colour}");
                var occupiedSquares = internalRow.OccupiedSquares;
                Console.WriteLine($"\tOccupied squares: {string.Join(", ", occupiedSquares.Select(c => c.ToString()))}");
            }
        }
    }
}
