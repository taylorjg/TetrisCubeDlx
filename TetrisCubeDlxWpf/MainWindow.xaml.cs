using System.Windows.Media;
using System.Windows.Media.Media3D;
using _3DTools;

namespace TetrisCubeDlxWpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var points = new Point3DCollection
            {
                new Point3D(0, 0, 0),
                new Point3D(0, 4, 0),

                new Point3D(0, 4, 0),
                new Point3D(4, 4, 0),

                new Point3D(4, 4, 0),
                new Point3D(4, 0, 0),

                new Point3D(4, 0, 0),
                new Point3D(0, 0, 0),

                new Point3D(0, 0, 0),
                new Point3D(0, 0, -4),

                new Point3D(0, 0, -4),
                new Point3D(0, 4, -4),

                new Point3D(0, 4, -4),
                new Point3D(4, 4, -4),

                new Point3D(4, 4, -4),
                new Point3D(4, 0, -4),

                new Point3D(4, 0, -4),
                new Point3D(0, 0, -4),
            };

            var wireframe = new ScreenSpaceLines3D
            {
                Points = points,
                Color = Colors.Black,
                Thickness = 0.5
            };

            //Model3DGroup.Children.Add(wireframe);
            Viewport3D.Children.Add(wireframe);
            //GeometryModel3D.Geometry.GetValue()
            //ModelVisual3D.Children.Add(wireframe);
        }
    }
}
