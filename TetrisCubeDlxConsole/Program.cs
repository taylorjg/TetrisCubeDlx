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
            var puzzle = new Puzzle();
            var internalRows = InternalRowBuilder.BuildInternalRows(puzzle);
            var dlxMatrix = DlxMatrixBuilder.BuildDlxMatrix(puzzle, internalRows);

            var dlx = new Dlx();
            var solution = dlx.Solve(dlxMatrix, rows => rows, row => row.Bits).FirstOrDefault();

            if (solution != null)
            {
                DumpSolutionSimple(solution, dlxMatrix);
                Console.WriteLine();
                DumpSolutionCube(puzzle, solution, dlxMatrix);
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

        private static void DumpSolutionCube(IPuzzle puzzle, Solution solution, IReadOnlyList<DlxMatrixRow> dlxMatrix)
        {
            var internalRows = solution.RowIndexes
                .Select(rowIndex => dlxMatrix[rowIndex].InternalRow)
                .ToImmutableList();

            foreach (var y in puzzle.DescendingDimensionIndices)
                DumpSolutionCubeHorizontalSlice(puzzle, internalRows, y);
        }

        private static void DumpSolutionCubeHorizontalSlice(
            IPuzzle puzzle,
            IReadOnlyCollection<InternalRow> internalRows,
            int y)
        {
            foreach (var z in puzzle.DescendingDimensionIndices)
                DumpLine(puzzle, internalRows, y, z);

            Console.WriteLine();
        }

        private static void DumpLine(IPuzzle puzzle, IEnumerable<InternalRow> internalRows, int y, int z)
        {
            var line = BuildLine(puzzle, internalRows, y, z);
            Console.WriteLine(line);
        }

        private static string BuildLine(IPuzzle puzzle, IEnumerable<InternalRow> internalRows, int y, int z)
        {
            var names = puzzle.AscendingDimensionIndices.Select(x =>
            {
                var coords = new Coords(x, y, z);
                var internalRow = FindInternalRowAt(internalRows, coords);
                return internalRow.Name;
            });

            return string.Concat(names);
        }

        private static InternalRow FindInternalRowAt(IEnumerable<InternalRow> internalRows, Coords coords)
        {
            return internalRows.Single(internalRow =>
                internalRow.OccupiedSquares.Any(occupiedSquare =>
                    occupiedSquare.Equals(coords)));
        }
    }
}
