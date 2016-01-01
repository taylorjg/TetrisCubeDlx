using System;
using System.Linq;
using NUnit.Framework;
using TetrisCubeDlx;

namespace TetrisCubeDlxTests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void RotationAndCorrectionExperiment()
        {
            var matrix = Matrix.Z90Cw.Multiply(Matrix.X90Cw);

            const int width = 3;
            const int height = 2;
            const int depth = 1;

            var allCoords = (
                from x in Enumerable.Range(0, width)
                from y in Enumerable.Range(0, height)
                from z in Enumerable.Range(0, depth)
                select new Coords(x, y, z))
                .ToList();

            allCoords.ForEach(c => Console.WriteLine($"coords {c} => {matrix.Multiply(c)}"));

            var dimensions = new Coords(width - 1, height - 1, depth - 1);
            var transformedDimensions = matrix.Multiply(dimensions);
            Console.WriteLine($"dimensions {dimensions} => {transformedDimensions}");

            var xCorrection = Math.Min(transformedDimensions.X, 0);
            var yCorrection = Math.Min(transformedDimensions.Y, 0);
            var zCorrection = Math.Min(transformedDimensions.Z, 0);
            Console.WriteLine($"corrections {xCorrection} {yCorrection} {zCorrection}");

            allCoords.ForEach(c =>
            {
                var coords2 = matrix.Multiply(c);
                var coords3 = new Coords(
                    coords2.X - xCorrection,
                    coords2.Y - yCorrection,
                    coords2.Z - zCorrection);
                Console.WriteLine($"corrected coords {c} => {coords3}");
            });
        }
    }
}
