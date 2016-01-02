namespace TetrisCubeDlx
{
    public class DlxMatrixRow
    {
        public DlxMatrixRow(int[] bits, InternalRow internalRow)
        {
            Bits = bits;
            InternalRow = internalRow;
        }

        public int[] Bits { get; }
        public InternalRow InternalRow { get; }
    }
}
