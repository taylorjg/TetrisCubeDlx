using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public class Piece
    {
        public Piece(IReadOnlyList<string[]> initStrings)
        {
            // TODO: validate initStrings...
            // cannot be null
            // no lengths can be zero
            // all strings must have the same length (= piece width)
            // all slices (inner arrays) must have the same length (= piece height)
            // every char (string element) must be a space or 'X'

            Width = initStrings[0][0].Length;
            Height = initStrings[0].Length;
            Depth = initStrings.Count;

            _squares = new bool[Width, Height, Depth];

            foreach (var coords in AllSquares)
            {
                var invertedHeight = Height - coords.Y - 1;
                _squares[coords.X, coords.Y, coords.Z] = initStrings[coords.Z][invertedHeight][coords.X] == 'X';
            }
        }

        private readonly bool[,,] _squares;

        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public IEnumerable<Coords> AllSquares =>
            from x in Enumerable.Range(0, Width)
            from y in Enumerable.Range(0, Height)
            from z in Enumerable.Range(0, Depth)
            select new Coords(x, y, z);

        public bool IsSquareOccupied(Coords coords)
        {
            return _squares[coords.X, coords.Y, coords.Z];
        }

        public IEnumerable<Coords> OccupiedSquares()
        {
            return AllSquares.Where(IsSquareOccupied);
        }
    }
}
