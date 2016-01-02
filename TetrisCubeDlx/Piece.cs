﻿using System.Collections.Generic;
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

            _squares = new bool[Width, Height, Depth];

            foreach (var coords in AllSquares)
            {
                var invertedHeight = Height - coords.Y - 1;
                _squares[coords.X, coords.Y, coords.Z] = initStrings[coords.Z][invertedHeight][coords.X] == 'X';
            }
        }

        private readonly bool[,,] _squares;

        public Colour Colour { get; }
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public IEnumerable<Coords> OccupiedSquares =>
            AllSquares.Where(IsSquareOccupied);

        private IEnumerable<Coords> AllSquares =>
            from x in Enumerable.Range(0, Width)
            from y in Enumerable.Range(0, Height)
            from z in Enumerable.Range(0, Depth)
            select new Coords(x, y, z);

        private bool IsSquareOccupied(Coords coords)
        {
            return _squares[coords.X, coords.Y, coords.Z];
        }
    }
}
