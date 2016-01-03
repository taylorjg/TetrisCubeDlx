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

            OccupiedSquares = (
                from x in Enumerable.Range(0, Width)
                from y in Enumerable.Range(0, Height)
                from z in Enumerable.Range(0, Depth)
                where initStrings[z][Height - y - 1][x] == 'X'
                select new Coords(x, y, z))
                .ToImmutableList();
        }

        public Colour Colour { get; }
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public IImmutableList<Coords> OccupiedSquares { get; }
    }
}
