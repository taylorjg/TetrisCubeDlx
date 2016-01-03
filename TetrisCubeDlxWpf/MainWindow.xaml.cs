using System.Windows.Media;
using System.Windows.Media.Media3D;
using TetrisCubeDlx;
using _3DTools;

namespace TetrisCubeDlxWpf
{
    public partial class MainWindow
    {
        private readonly IPuzzle _puzzle = new Puzzle();

        public MainWindow()
        {
            InitializeComponent();
            DrawWireframeAxes();
            DrawWireframeCube();
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
    }
}
