namespace TetrisCubeDlx
{
    public class Matrix
    {
        public Matrix(
            int a1, int a2, int a3,
            int b1, int b2, int b3,
            int c1, int c2, int c3)
        {
            A1 = a1;
            A2 = a2;
            A3 = a3;

            B1 = b1;
            B2 = b2;
            B3 = b3;

            C1 = c1;
            C2 = c2;
            C3 = c3;
        }

        public int A1 { get; }
        public int A2 { get; }
        public int A3 { get; }

        public int B1 { get; }
        public int B2 { get; }
        public int B3 { get; }

        public int C1 { get; }
        public int C2 { get; }
        public int C3 { get; }

        public Matrix Multiply(Matrix other)
        {
            return new Matrix(
                A1 * other.A1 + A2 * other.B1 + A3 * other.C1,
                A1 * other.A2 + A2 * other.B2 + A3 * other.C2,
                A1 * other.A3 + A2 * other.B3 + A3 * other.C3,

                B1 * other.A1 + B2 * other.B1 + B3 * other.C1,
                B1 * other.A2 + B2 * other.B2 + B3 * other.C2,
                B1 * other.A3 + B2 * other.B3 + B3 * other.C3,

                C1 * other.A1 + C2 * other.B1 + C3 * other.C1,
                C1 * other.A2 + C2 * other.B2 + C3 * other.C2,
                C1 * other.A3 + C2 * other.B3 + C3 * other.C3);
        }

        public Coords Multiply(Coords coords)
        {
            return new Coords(
                A1 * coords.X + A2 * coords.Y + A3 * coords.Z,
                B1 * coords.X + B2 * coords.Y + B3 * coords.Z,
                C1 * coords.X + C2 * coords.Y + C3 * coords.Z);
        }
    }
}
