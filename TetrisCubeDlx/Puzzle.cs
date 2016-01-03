using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class Puzzle
    {
        public static int CubeSize => 4;
        public static int CubeSizeSquared => CubeSize*CubeSize;
        public static int CubeSizeCubed => CubeSize*CubeSize*CubeSize;
        public static IImmutableList<Piece> Pieces => LazyPieces.Value;

        private static readonly Lazy<IImmutableList<Piece>> LazyPieces =
            new Lazy<IImmutableList<Piece>>(MakeAllPieces);

        private static IImmutableList<Piece> MakeAllPieces()
        {
            var pieces = new List<Piece>();
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape1, Colour.Orange, "A"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape1, Colour.Cerise, "B", "C"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape2, Colour.Magenta, "D", "E", "F", "G"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape3, Colour.Red, "H", "I"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape3, Colour.Green, "J", "K"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape4, Colour.Yellow, "L", "M", "N"));
            pieces.AddRange(MakePiecesWithShapeAndColour(Shape5, Colour.Blue, "O", "P"));
            return pieces.ToImmutableList();
        }

        private static IEnumerable<Piece> MakePiecesWithShapeAndColour(
            IReadOnlyList<string[]> shape,
            Colour colour,
            params string[] names)
        {
            return names.Select(name => new Piece(shape, colour, name));
        }

        private static readonly string[][] Shape1 =
        {
            new[]
            {
                "X ",
                "X ",
                "XX"
            }
        };

        private static readonly string[][] Shape2 =
        {
            new[]
            {
                "X ",
                "XX",
                "X "
            }
        };

        private static readonly string[][] Shape3 =
        {
            new[]
            {
                " X",
                "XX",
                "X "
            }
        };

        private static readonly string[][] Shape4 =
        {
            new[]
            {
                "XX",
                "XX"
            }
        };

        private static readonly string[][] Shape5 =
        {
            new[]
            {
                "X",
                "X",
                "X",
                "X"
            }
        };
    }
}
