using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class Pieces
    {
        public static List<Piece> MakePieces()
        {
            var pieces = new List<Piece>();
            pieces.AddRange(MakePieces(Shape1, Colour.Orange, "A"));
            pieces.AddRange(MakePieces(Shape1, Colour.Cerise, "B", "C"));
            pieces.AddRange(MakePieces(Shape2, Colour.Magenta, "D", "E", "F", "G"));
            pieces.AddRange(MakePieces(Shape3, Colour.Red, "H", "I"));
            pieces.AddRange(MakePieces(Shape3, Colour.Green, "J", "K"));
            pieces.AddRange(MakePieces(Shape4, Colour.Yellow, "L", "M", "N"));
            pieces.AddRange(MakePieces(Shape5, Colour.Blue, "O", "P"));
            return pieces;
        }

        private static IEnumerable<Piece> MakePieces(IReadOnlyList<string[]> shape, Colour colour, params string[] names)
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

        // 4 magenta
        private static readonly string[][] Shape2 =
        {
            new[]
            {
                "X ",
                "XX",
                "X "
            }
        };

        // 2 red
        // 3 green
        private static readonly string[][] Shape3 =
        {
            new[]
            {
                " X",
                "XX",
                "X "
            }
        };

        // 3 yellow
        private static readonly string[][] Shape4 =
        {
            new[]
            {
                "XX",
                "XX"
            }
        };

        // 2 blue
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
