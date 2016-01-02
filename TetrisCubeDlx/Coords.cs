namespace TetrisCubeDlx
{
    public class Coords
    {
        public Coords(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Coords;
            if (other == null) return false;
            return
                X == other.X &&
                Y == other.Y &&
                Z == other.Z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }

        public static Coords operator+(Coords c1, Coords c2)
        {
            return new Coords(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Coords operator-(Coords c1, Coords c2)
        {
            return new Coords(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
