using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public abstract class PuzzleBase : IPuzzle
    {
        public abstract int CubeSize { get; }
        public int CubeSizeSquared => CubeSize*CubeSize;
        public int CubeSizeCubed => CubeSizeSquared*CubeSize;
        public IEnumerable<int> AscendingDimensionIndices => Enumerable.Range(0, CubeSize);
        public IEnumerable<int> DescendingDimensionIndices => AscendingDimensionIndices.Reverse();
        public abstract IEnumerable<Piece> Pieces { get; }
    }
}
