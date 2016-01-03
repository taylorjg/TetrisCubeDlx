using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DlxLib;
using TetrisCubeDlx;
using TetrisCubeDlxWpf.Extensions;

namespace TetrisCubeDlxWpf
{
    public class PuzzleSolver
    {
        private readonly IPuzzle _puzzle;
        private readonly Action<IImmutableList<InternalRow>> _onSolutionFound;
        private readonly Action<IImmutableList<InternalRow>> _onSearchStep;
        private readonly SynchronizationContext _synchronizationContext;
        private readonly CancellationToken _cancellationToken;

        public PuzzleSolver(
            IPuzzle puzzle,
            Action<IImmutableList<InternalRow>> onSolutionFound,
            Action<IImmutableList<InternalRow>> onSearchStep,
            SynchronizationContext synchronizationContext,
            CancellationToken cancellationToken)
        {
            _puzzle = puzzle;
            _onSolutionFound = onSolutionFound;
            _onSearchStep = onSearchStep;
            _synchronizationContext = synchronizationContext;
            _cancellationToken = cancellationToken;
        }

        public void SolvePuzzle()
        {
            Task.Factory.StartNew(
                SolvePuzzleInBackground,
                _cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void SolvePuzzleInBackground()
        {
            var internalRows = InternalRowBuilder.BuildInternalRows(_puzzle);
            var dlxMatrix = DlxMatrixBuilder.BuildDlxMatrix(_puzzle, internalRows);

            var dlx = new Dlx(_cancellationToken);

            dlx.SearchStep += (_, searchStepEventArgs) =>
                InvokeActionPassingSubsetOfInternalRows(
                    internalRows,
                    searchStepEventArgs.RowIndexes,
                    _onSearchStep);

            var firstSolution = dlx.Solve(dlxMatrix, rows => rows, row => row.Bits).FirstOrDefault();

            if (firstSolution != null)
                InvokeActionPassingSubsetOfInternalRows(
                    internalRows,
                    firstSolution.RowIndexes,
                    _onSolutionFound);
        }

        private void InvokeActionPassingSubsetOfInternalRows(
            IReadOnlyList<InternalRow> internalRows,
            IEnumerable<int> rowIndexes,
            Action<IImmutableList<InternalRow>> action)
        {
            var subsetOfInternalRows = rowIndexes.Select(rowIndex => internalRows[rowIndex]).ToImmutableList();
            _synchronizationContext.Post(action, subsetOfInternalRows);
        }
    }
}
