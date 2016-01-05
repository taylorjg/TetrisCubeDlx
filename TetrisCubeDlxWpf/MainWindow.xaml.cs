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
            _timer.Interval = TimeSpan.FromMilliseconds(500);

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

        // private void DrawWireframeAxes()
        // {
        //     var points = new Point3DCollection
        //     {
        //         new Point3D(-100, 0, 0),
        //         new Point3D(+100, 0, 0),
        //         new Point3D(0, -100, 0),
        //         new Point3D(0, +100, 0),
        //         new Point3D(0, 0, -100),
        //         new Point3D(0, 0, +100)
        //     };
        // 
        //     var wireframeAxes = new ScreenSpaceLines3D
        //     {
        //         Points = points,
        //         Color = Colors.Red,
        //         Thickness = 0.5
        //     };
        // 
        //     Viewport3D.Children.Add(wireframeAxes);
        // }

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
                            SpecularPower = 50.0d
                        }
                    }
                },
                BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Silver))
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
            const int density = 5;
            const double delta = 1d/density;

            switch (face)
            {
                case Face.Front:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X, coords.Y, -coords.Z),
                        new Point3D(coords.X + delta, coords.Y, -coords.Z),
                        new Point3D(coords.X + delta, coords.Y + delta, -coords.Z),
                        new Point3D(coords.X, coords.Y + delta, -coords.Z),
                        density,
                        0, delta, 0,
                        delta, 0, 0);
                    break;

                case Face.Back:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X + delta, coords.Y, -coords.Z - 1),
                        new Point3D(coords.X, coords.Y, -coords.Z - 1),
                        new Point3D(coords.X, coords.Y + delta, -coords.Z - 1),
                        new Point3D(coords.X + delta, coords.Y + delta, -coords.Z - 1),
                        density,
                        0, delta, 0,
                        delta, 0, 0);
                    break;

                case Face.Left:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X, coords.Y, -coords.Z - delta),
                        new Point3D(coords.X, coords.Y, -coords.Z),
                        new Point3D(coords.X, coords.Y + delta, -coords.Z),
                        new Point3D(coords.X, coords.Y + delta, -coords.Z - delta),
                        density,
                        0, delta, 0,
                        0, 0, -delta);
                    break;

                case Face.Right:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X + 1, coords.Y, -coords.Z),
                        new Point3D(coords.X + 1, coords.Y, -coords.Z - delta),
                        new Point3D(coords.X + 1, coords.Y + delta, -coords.Z - delta),
                        new Point3D(coords.X + 1, coords.Y + delta, -coords.Z),
                        density,
                        0, delta, 0,
                        0, 0, -delta);
                    break;

                case Face.Top:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X, coords.Y + 1, -coords.Z),
                        new Point3D(coords.X + delta, coords.Y + 1, -coords.Z),
                        new Point3D(coords.X + delta, coords.Y + 1, -coords.Z - delta),
                        new Point3D(coords.X, coords.Y + 1, -coords.Z - delta),
                        density,
                        0, 0, -delta,
                        delta, 0, 0);
                    break;

                case Face.Bottom:
                    AddFaceTriangles3(
                        positions, triangleIndices,
                        new Point3D(coords.X + delta, coords.Y, -coords.Z - delta),
                        new Point3D(coords.X + delta, coords.Y, -coords.Z),
                        new Point3D(coords.X, coords.Y, -coords.Z),
                        new Point3D(coords.X, coords.Y, -coords.Z - delta),
                        density,
                        delta, 0, 0,
                        0, 0, -delta);
                    break;
            }
        }

        // private static void AddFaceTriangles1(
        //     Point3DCollection positions,
        //     Int32Collection triangleIndices,
        //     Point3D pt1,
        //     Point3D pt2,
        //     Point3D pt3,
        //     Point3D pt4)
        // {
        //     positions.Add(pt1);
        //     positions.Add(pt2);
        //     positions.Add(pt4);
        //     Enumerable.Range(triangleIndices.Count, 3).ToList().ForEach(triangleIndices.Add);
        // 
        //     positions.Add(pt2);
        //     positions.Add(pt3);
        //     positions.Add(pt4);
        //     Enumerable.Range(triangleIndices.Count, 3).ToList().ForEach(triangleIndices.Add);
        // }

        // private static void AddFaceTriangles2(
        //     Point3DCollection positions,
        //     Int32Collection triangleIndices,
        //     Point3D pt1,
        //     Point3D pt2,
        //     Point3D pt3,
        //     Point3D pt4)
        // {
        //     const int density = 10;
        //     const double size = 1d/density;
        // 
        //     var bottomLeftPoints = (
        //         from x in Enumerable.Range(0, density)
        //         from y in Enumerable.Range(0, density)
        //         select new Point3D(pt1.X + x*size, pt1.Y + y*size, pt1.Z)).ToList();
        // 
        //     bottomLeftPoints.ForEach(pt =>
        //     {
        //         positions.Add(pt);
        //         positions.Add(new Point3D(pt.X + size, pt.Y, pt.Z));
        //         positions.Add(new Point3D(pt.X, pt.Y + size, pt.Z));
        //         positions.Add(new Point3D(pt.X + size, pt.Y, pt.Z));
        //         positions.Add(new Point3D(pt.X + size, pt.Y + size, pt.Z));
        //         positions.Add(new Point3D(pt.X, pt.Y + size, pt.Z));
        //     });
        // 
        //     Enumerable.Range(triangleIndices.Count, 600).ToList().ForEach(triangleIndices.Add);
        // }

        private static void AddFaceTriangles3(
            Point3DCollection positions,
            Int32Collection triangleIndices,
            Point3D pt1,
            Point3D pt2,
            Point3D pt3,
            Point3D pt4,
            int density,
            double dxRow,
            double dyRow,
            double dzRow,
            double dxCol,
            double dyCol,
            double dzCol)
        {
            foreach (var row in Enumerable.Range(0, density))
            {
                foreach (var col in Enumerable.Range(0, density))
                {
                    var pt1A = new Point3D(pt1.X + row*dxRow + col*dxCol, pt1.Y + row*dyRow + col*dyCol, pt1.Z + row*dzRow + col*dzCol);
                    var pt2A = new Point3D(pt2.X + row*dxRow + col*dxCol, pt2.Y + row*dyRow + col*dyCol, pt2.Z + row*dzRow + col*dzCol);
                    var pt3A = new Point3D(pt3.X + row*dxRow + col*dxCol, pt3.Y + row*dyRow + col*dyCol, pt3.Z + row*dzRow + col*dzCol);
                    var pt4A = new Point3D(pt4.X + row*dxRow + col*dxCol, pt4.Y + row*dyRow + col*dyCol, pt4.Z + row*dzRow + col*dzCol);
                    positions.Add(pt1A);
                    positions.Add(pt2A);
                    positions.Add(pt4A);
                    positions.Add(pt2A);
                    positions.Add(pt3A);
                    positions.Add(pt4A);
                }
            }

            Enumerable.Range(triangleIndices.Count, 6*density*density).ToList().ForEach(triangleIndices.Add);
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
