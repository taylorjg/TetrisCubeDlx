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

        private int A1 { get; }
        private int A2 { get; }
        private int A3 { get; }

        private int B1 { get; }
        private int B2 { get; }
        private int B3 { get; }

        private int C1 { get; }
        private int C2 { get; }
        private int C3 { get; }

        public Matrix Multiply(Matrix other)
        {
            return new Matrix(
                A1*other.A1 + A2*other.B1 + A3*other.C1,
                A1*other.A2 + A2*other.B2 + A3*other.C2,
                A1*other.A3 + A2*other.B3 + A3*other.C3,

                B1*other.A1 + B2*other.B1 + B3*other.C1,
                B1*other.A2 + B2*other.B2 + B3*other.C2,
                B1*other.A3 + B2*other.B3 + B3*other.C3,

                C1*other.A1 + C2*other.B1 + C3*other.C1,
                C1*other.A2 + C2*other.B2 + C3*other.C2,
                C1*other.A3 + C2*other.B3 + C3*other.C3);
        }

        public Coords Multiply(Coords coords)
        {
            return new Coords(
                A1*coords.X + A2*coords.Y + A3*coords.Z,
                B1*coords.X + B2*coords.Y + B3*coords.Z,
                C1*coords.X + C2*coords.Y + C3*coords.Z);
        }

        public static readonly Matrix Identity =
            new Matrix(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1);

        public static readonly Matrix X90Cw =
            new Matrix(
                1, 0, 0,
                0, 0, 1,
                0, -1, 0);

        public static readonly Matrix X180Cw =
            new Matrix(
                1, 0, 0,
                0, -1, 0,
                0, 0, -1);

        public static readonly Matrix X270Cw =
            new Matrix(
                1, 0, 0,
                0, 0, -1,
                0, 1, 0);

        public static readonly Matrix Y90Cw =
            new Matrix(
                0, 0, -1,
                0, 1, 0,
                1, 0, 0);

        public static readonly Matrix Y180Cw =
            new Matrix(
                -1, 0, 0,
                0, 1, 0,
                0, 0, -1);

        public static readonly Matrix Y270Cw =
            new Matrix(
                0, 0, 1,
                0, 1, 0,
                -1, 0, 0);

        public static readonly Matrix Z90Cw =
            new Matrix(
                0, 1, 0,
                -1, 0, 0,
                0, 0, 1);

        public static readonly Matrix Z180Cw =
            new Matrix(
                -1, 0, 0,
                0, -1, 0,
                0, 0, 1);

        public static readonly Matrix Z270Cw =
            new Matrix(
                0, -1, 0,
                1, 0, 0,
                0, 0, 1);
    }
}
