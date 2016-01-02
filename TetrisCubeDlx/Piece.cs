using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public class Piece
    {
        public Piece(IReadOnlyList<string[]> initStrings, Colour colour = Colour.Orange, string name = "")
        {
            // TODO: validate initStrings...
            // cannot be null
            // no lengths can be zero
            // all strings must have the same length (= piece width)
            // all slices (inner arrays) must have the same length (= piece height)
            // every char (string element) must be a space or 'X'

            Colour = colour;
            Name = name;
            Width = initStrings[0][0].Length;
            Height = initStrings[0].Length;
            Depth = initStrings.Count;

            var allSquares = AllSquares;
            var flags = new bool[Width, Height, Depth];

            foreach (var coords in allSquares)
            {
                var invertedHeight = Height - coords.Y - 1;
                flags[coords.X, coords.Y, coords.Z] = initStrings[coords.Z][invertedHeight][coords.X] == 'X';
            }

            OccupiedSquares = allSquares.Where(coords => FlagsIsSet(flags, coords)).ToImmutableList();
        }

        public Colour Colour { get; }
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public IImmutableList<Coords> OccupiedSquares { get; }

        private IImmutableList<Coords> AllSquares => (
            from x in Enumerable.Range(0, Width)
            from y in Enumerable.Range(0, Height)
            from z in Enumerable.Range(0, Depth)
            select new Coords(x, y, z))
            .ToImmutableList();

        private static bool FlagsIsSet(bool[,,] flags, Coords coords)
        {
            return flags[coords.X, coords.Y, coords.Z];
        }
    }
}
