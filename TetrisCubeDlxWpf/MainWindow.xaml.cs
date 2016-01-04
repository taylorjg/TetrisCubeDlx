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
        private readonly Dictionary<string, Tuple<InternalRow, Model3DGroup>> _dictionary =
            new Dictionary<string, Tuple<InternalRow, Model3DGroup>>();
        private readonly InternalRowEqualityComparer _comparer = new InternalRowEqualityComparer();
        private readonly Dictionary<Colour, Color> _colourLookup =
            new Dictionary<Colour, Color>
            {
                //{ Colour.Orange, Colors.Orange},
                //{ Colour.Cerise, Colors.DeepPink},
                //{ Colour.Magenta, Colors.Magenta},
                //{ Colour.Red, Colors.OrangeRed},
                //{ Colour.Green, Colors.MediumSeaGreen},
                //{ Colour.Yellow, Colors.Yellow},
                //{ Colour.Blue, Colors.DodgerBlue},
                { Colour.Orange, Color.FromRgb(0xFB, 0x89, 0x29)},
                { Colour.Cerise, Color.FromRgb(0xEB, 0x2A, 0x6D)},
                { Colour.Magenta, Color.FromRgb(0xE8, 0x3C, 0x91)},
                { Colour.Red, Color.FromRgb(0xFB, 0x24, 0x39)},
                { Colour.Green, Color.FromRgb(0x55, 0xAE, 0x59)},
                { Colour.Yellow, Color.FromRgb(0xFD, 0xD2, 0x23)},
                { Colour.Blue, Color.FromRgb(0x2A, 0x82, 0xB0)}
            };

        public MainWindow()
        {
            InitializeComponent();

            _timer.Tick += (_, __) => OnTick();
            _timer.Interval = TimeSpan.FromMilliseconds(1);

            ContentRendered += (_, __) =>
            {
                //DrawWireframeAxes();
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
                for (var x = 0; x <= _puzzle.CubeSize; x++)
                {
                    points.Add(new Point3D(x, 0, z));
                    points.Add(new Point3D(x, _puzzle.CubeSize, z));
                }

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

        private GeometryModel3D CreateUnitCubeGeometryModel(InternalRow internalRow, Coords coords)
        {
            var positions = new Point3DCollection();
            var triangleIndices = new Int32Collection();

            AddFace(positions, triangleIndices, Face.Front, coords);
            AddFace(positions, triangleIndices, Face.Back, coords);
            AddFace(positions, triangleIndices, Face.Left, coords);
            AddFace(positions, triangleIndices, Face.Right, coords);
            AddFace(positions, triangleIndices, Face.Top, coords);
            AddFace(positions, triangleIndices, Face.Bottom, coords);

            return new GeometryModel3D
            {
                Geometry = new MeshGeometry3D
                {
                    Positions = positions,
                    TriangleIndices = triangleIndices
                },
                Material = new MaterialGroup
                {
                    Children = new MaterialCollection
                    {
                        new DiffuseMaterial(new SolidColorBrush(_colourLookup[internalRow.Colour]))
                        {
                            Brush = {Opacity = 1.0d}
                        },
                        new SpecularMaterial
                        {
                            Color = Colors.White,
                            SpecularPower = 30.0d
                        }
                    }
                }
            };
        }

        private enum Face
        {
            Front,
            Back,
            Left,
            Right,
            Top,
            Bottom
        }

        private static void AddFace(Point3DCollection positions, Int32Collection triangleIndices, Face face, Coords coords)
        {
            switch (face)
            {
                case Face.Front:
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z));
                    break;

                case Face.Back:
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z - 1));
                    break;

                case Face.Left:
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z - 1));
                    break;

                case Face.Right:
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z));
                    break;

                case Face.Top:
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z));
                    positions.Add(new Point3D(coords.X + 1, coords.Y + 1, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X, coords.Y + 1, -coords.Z - 1));
                    break;

                case Face.Bottom:
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z - 1));
                    positions.Add(new Point3D(coords.X + 1, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z));
                    positions.Add(new Point3D(coords.X, coords.Y, -coords.Z - 1));
                    break;
            }

            Enumerable.Range(triangleIndices.Count, 6).ToList().ForEach(triangleIndices.Add);
        }

        private Model3DGroup CreateModelGroupForInternalRow(InternalRow internalRow)
        {
            var modelGroup = new Model3DGroup();
            foreach (var coords in internalRow.OccupiedSquares)
            {
                var geometryModel = CreateUnitCubeGeometryModel(internalRow, coords);
                modelGroup.Children.Add(geometryModel);
            }
            return modelGroup;
        }

        private void AddModelGroupForInternalRow(InternalRow internalRow)
        {
            var modelGroup = CreateModelGroupForInternalRow(internalRow);
            CubeGroup.Children.Add(modelGroup);
            _dictionary[internalRow.Name] = Tuple.Create(internalRow, modelGroup);
        }

        private void RemoveModelGroupForInternalRow(Tuple<InternalRow, Model3DGroup> tuple)
        {
            var internalRow = tuple.Item1;
            var geometryModel = tuple.Item2;
            CubeGroup.Children.Remove(geometryModel);
            _dictionary.Remove(internalRow.Name);
        }

        private void OnTick()
        {
            if (_queue.IsEmpty())
                return;

            var internalRows = _queue.Dequeue();

            if (internalRows == null)
            {
                _timer.Stop();
                return;
            }

            AdjustPlacedPieces(internalRows);
        }

        private void AdjustPlacedPieces(IImmutableList<InternalRow> internalRows)
        {
            foreach (var internalRow in internalRows)
            {
                Tuple<InternalRow, Model3DGroup> tuple;
                if (_dictionary.TryGetValue(internalRow.Name, out tuple))
                    if (!_comparer.Equals(internalRow, tuple.Item1))
                        RemoveModelGroupForInternalRow(tuple);
            }

            foreach (var internalRow in internalRows)
                if (!_dictionary.ContainsKey(internalRow.Name))
                    AddModelGroupForInternalRow(internalRow);
        }

        private class InternalRowEqualityComparer : IEqualityComparer<InternalRow>
        {
            public bool Equals(InternalRow internalRow1, InternalRow internalRow2)
            {
                var occupiedSquares1 = internalRow1.OccupiedSquares;
                var occupiedSquares2 = internalRow2.OccupiedSquares;

                return
                    occupiedSquares1.Except(occupiedSquares2).IsEmpty() &&
                    occupiedSquares2.Except(occupiedSquares1).IsEmpty();
            }

            public int GetHashCode(InternalRow _)
            {
                return 0;
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
