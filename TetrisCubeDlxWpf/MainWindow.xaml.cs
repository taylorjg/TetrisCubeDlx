using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using TetrisCubeDlx;
using _3DTools;

namespace TetrisCubeDlxWpf
{
    public partial class MainWindow
    {
        private readonly IPuzzle _puzzle = new Puzzle();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Queue<IImmutableList<InternalRow>> _queue = new Queue<IImmutableList<InternalRow>>();
        private CancellationTokenSource _cancellationTokenSource;
        private PuzzleSolver _puzzleSolver;

        public MainWindow()
        {
            InitializeComponent();

            _timer.Tick += (_, __) => OnTick();
            _timer.Interval = TimeSpan.FromMilliseconds(100);

            ContentRendered += (_, __) =>
            {
                DrawWireframeAxes();
                DrawWireframeCube();

                _cancellationTokenSource = new CancellationTokenSource();

                _puzzleSolver = new PuzzleSolver(
                    _puzzle,
                    OnSolutionFound,
                    OnSearchStep,
                    SynchronizationContext.Current,
                    _cancellationTokenSource.Token);

                _puzzleSolver.SolvePuzzle();

            };

            Closed += (_, __) => _cancellationTokenSource?.Cancel();
        }

        private void DrawWireframeAxes()
        {
            var points = new Point3DCollection
            {
                new Point3D(-100, 0, 0),
                new Point3D(+100, 0, 0),
                new Point3D(0, -100, 0),
                new Point3D(0, +100, 0),
                new Point3D(0, 0, -100),
                new Point3D(0, 0, +100)
            };

            var wireframeAxes = new ScreenSpaceLines3D
            {
                Points = points,
                Color = Colors.Red,
                Thickness = 0.5
            };

            Viewport3D.Children.Add(wireframeAxes);
        }

        private void DrawWireframeCube()
        {
            var points = new Point3DCollection();

            for (var z = 0; z >= -_puzzle.CubeSize; z--)
            {
                // Vertical lines
                for (var x = 0; x <= _puzzle.CubeSize; x++)
                {
                    points.Add(new Point3D(x, 0, z));
                    points.Add(new Point3D(x, _puzzle.CubeSize, z));
                }

                // Horizontal lines
                for (var y = 0; y <= _puzzle.CubeSize; y++)
                {
                    points.Add(new Point3D(0, y, z));
                    points.Add(new Point3D(_puzzle.CubeSize, y, z));
                }
            }

            for (var x = 0; x <= _puzzle.CubeSize; x++)
            {
                for (var y = 0; y <= _puzzle.CubeSize; y++)
                {
                    points.Add(new Point3D(x, y, 0));
                    points.Add(new Point3D(x, y, -_puzzle.CubeSize));
                }
            }

            var wireframeCube = new ScreenSpaceLines3D
            {
                Points = points,
                Color = Colors.Black,
                Thickness = 0.5
            };

            Viewport3D.Children.Add(wireframeCube);
        }

        private void OnTick()
        {
            if (!_queue.Any()) return;

            var internalRows = _queue.Dequeue();

            if (internalRows == null)
            {
                _timer.Stop();
                // return;
            }
        }

        private void OnSolutionFound(IImmutableList<InternalRow> internalRows)
        {
            _queue.Enqueue(null);
            _puzzleSolver = null;
            _cancellationTokenSource = null;
        }

        private void OnSearchStep(IImmutableList<InternalRow> internalRows)
        {
            _queue.Enqueue(internalRows);

            if (!_timer.IsEnabled)
                _timer.Start();
        }
    }
}
