namespace TetrisCubeDlx
{
    public class PlacedPiece
    {
        public PlacedPiece(Piece piece, Orientation orientation)
        {
            Piece = piece;
            Orientation = orientation;
            Width = piece.Width;
            Height = piece.Height;
            Depth = piece.Depth;
        }

        public Piece Piece { get; }
        public Orientation Orientation { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
    }
}
