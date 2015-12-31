using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace TetrisCubeDlx
{
    public class Matrix
    {
        public Matrix(
            int a1, int a2, int a3, int a4,
            int b1, int b2, int b3, int b4,
            int c1, int c2, int c3, int c4,
            int d1, int d2, int d3, int d4)
        {
            A1 = a1;
            A2 = a2;
            A3 = a3;
            A4 = a4;

            B1 = b1;
            B2 = b2;
            B3 = b3;
            B4 = b4;

            C1 = c1;
            C2 = c2;
            C3 = c3;
            C4 = c4;

            D1 = d1;
            D2 = d2;
            D3 = d3;
            D4 = d4;
        }

        private int A1 { get; }
        private int A2 { get; }
        private int A3 { get; }
        private int A4 { get; }

        private int B1 { get; }
        private int B2 { get; }
        private int B3 { get; }
        private int B4 { get; }

        private int C1 { get; }
        private int C2 { get; }
        private int C3 { get; }
        private int C4 { get; }
        
        private int D1 { get; }
        private int D2 { get; }
        private int D3 { get; }
        private int D4 { get; }

        public Matrix Multiply(Matrix other)
        {
            return new Matrix(
                A1 * other.A1 + A2 * other.B1 + A3 * other.C1 + A4 * other.D1,
                A1 * other.A2 + A2 * other.B2 + A3 * other.C2 + A4 * other.D2,
                A1 * other.A3 + A2 * other.B3 + A3 * other.C3 + A4 * other.D3,
                A1 * other.A4 + A2 * other.B4 + A3 * other.C4 + A4 * other.D4,

                B1 * other.A1 + B2 * other.B1 + B3 * other.C1 + B4 * other.D1,
                B1 * other.A2 + B2 * other.B2 + B3 * other.C2 + B4 * other.D2,
                B1 * other.A3 + B2 * other.B3 + B3 * other.C3 + B4 * other.D3,
                B1 * other.A4 + B2 * other.B4 + B3 * other.C4 + B4 * other.D4,

                C1 * other.A1 + C2 * other.B1 + C3 * other.C1 + C4 * other.D1,
                C1 * other.A2 + C2 * other.B2 + C3 * other.C2 + C4 * other.D2,
                C1 * other.A3 + C2 * other.B3 + C3 * other.C3 + C4 * other.D3,
                C1 * other.A4 + C2 * other.B4 + C3 * other.C4 + C4 * other.D4,

                D1 * other.A1 + D2 * other.B1 + D3 * other.C1 + D4 * other.D1,
                D1 * other.A2 + D2 * other.B2 + D3 * other.C2 + D4 * other.D2,
                D1 * other.A3 + D2 * other.B3 + D3 * other.C3 + D4 * other.D3,
                D1 * other.A4 + D2 * other.B4 + D3 * other.C4 + D4 * other.D4);
        }

        public Coords Multiply(Coords coords)
        {
            return new Coords(
                A1 * coords.X + A2 * coords.Y + A3 * coords.Z + A4,
                B1 * coords.X + B2 * coords.Y + B3 * coords.Z + B4,
                C1 * coords.X + C2 * coords.Y + C3 * coords.Z + C4);
        }

        public Coords InverseMultiply(Coords coords)
        {
            double[,] array =
            {
                {A1, A2, A3, A4},
                {B1, B2, B3, B4},
                {C1, C2, C3, C4},
                {D1, D2, D3, D4}
            };

            var matrixBuilder = Matrix<double>.Build;
            var matrix = matrixBuilder.DenseOfArray(array);
            var inverseMatrix = matrix.Inverse();

            var vectorBuilder = Vector<double>.Build;
            var vector = vectorBuilder.DenseOfArray(new double[] {coords.X, coords.Y, coords.Z, 1});
            var inverseVector = inverseMatrix.Multiply(vector);

            var x = (int) System.Math.Round(inverseVector[0]);
            var y = (int) System.Math.Round(inverseVector[1]);
            var z = (int) System.Math.Round(inverseVector[2]);

            return new Coords(x, y, z);
        }

        public static readonly Matrix Identity =
            new Matrix(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

        public static readonly Matrix Z90Cw =
            new Matrix(
                0, 1, 0, 0,
                -1, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

        public static readonly Matrix Z180Cw =
            new Matrix(
                -1, 0, 0, 0,
                0, -1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

        public static readonly Matrix Z270Cw =
            new Matrix(
                0, -1, 0, 0,
                1, 0, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

        public static readonly Matrix X90Cw =
            new Matrix(
                1, 0, 0, 0,
                0, 0, 1, 0,
                0, -1, 0, 0,
                0, 0, 0, 1);

        public static Matrix Translation(int tx, int ty, int tz)
        {
            return new Matrix(
                1, 0, 0, tx,
                0, 1, 0, ty,
                0, 0, 1, tz,
                0, 0, 0, 1);
        }

        public static Matrix MultiplyMatrices(params Matrix[] ms)
        {
            return ms.Aggregate(Identity, (acc, m) => acc.Multiply(m));
        }
    }
}
