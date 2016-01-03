using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DlxLib;
using TetrisCubeDlx;

namespace TetrisCubeDlxConsole
{
    internal static class Program
    {
        private static void Main()
        {
            var internalRows = InternalRowBuilder.BuildInternalRows();
            var dlxMatrix = DlxMatrixBuilder.BuildDlxMatrix(internalRows);

            var dlx = new Dlx();
            var solution = dlx.Solve(dlxMatrix, rows => rows, row => row.Bits).FirstOrDefault();

            if (solution != null)
            {
                DumpSolutionSimple(solution, dlxMatrix);
                Console.WriteLine();
                DumpSolutionCube(solution, dlxMatrix);
            }
        }

        private static void DumpSolutionSimple(Solution solution, IReadOnlyList<DlxMatrixRow> dlxMatrix)
        {
            foreach (var rowIndex in solution.RowIndexes)
            {
                var dlxMatrixRow = dlxMatrix[rowIndex];
                var internalRow = dlxMatrixRow.InternalRow;
                Console.WriteLine($"Name: {internalRow.Name}; Colour: {internalRow.Colour}");
            }
        }

        private static void DumpSolutionCube(Solution solution, IReadOnlyList<DlxMatrixRow> dlxMatrix)
        {
            var internalRows = solution.RowIndexes
                .Select(rowIndex => dlxMatrix[rowIndex].InternalRow)
                .ToImmutableList();

            DumpSolutionCubeHorizontalSlice(internalRows, 3);
            DumpSolutionCubeHorizontalSlice(internalRows, 2);
            DumpSolutionCubeHorizontalSlice(internalRows, 1);
            DumpSolutionCubeHorizontalSlice(internalRows, 0);
        }

        private static void DumpSolutionCubeHorizontalSlice(IReadOnlyCollection<InternalRow> internalRows, int y)
        {
            for (var z = 3; z >= 0; z--)
            {
                var line = "";
                for (var x = 0; x <= 3; x++)
                {
                    var coords = new Coords(x, y, z);
                    var internalRow = FindInternalRowAt(internalRows, coords);
                    var name = internalRow.Name;
                    line += name;
                }
                Console.WriteLine(line);
            }

            Console.WriteLine();
        }

        private static InternalRow FindInternalRowAt(IEnumerable<InternalRow> internalRows, Coords coords)
        {
            return internalRows.Single(internalRow =>
                internalRow.OccupiedSquares.Any(occupiedSquare =>
                    occupiedSquare.Equals(coords)));
        }
    }
}
