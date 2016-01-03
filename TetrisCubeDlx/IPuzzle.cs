using System.Collections.Generic;

namespace TetrisCubeDlx
{
    public interface IPuzzle
    {
        int CubeSize { get; }
        int CubeSizeSquared { get; }
        int CubeSizeCubed { get; }
        IEnumerable<int> AscendingDimensionIndices { get; }
        IEnumerable<int> DescendingDimensionIndices { get; }
        IEnumerable<Piece> Pieces { get; }
    }
}
