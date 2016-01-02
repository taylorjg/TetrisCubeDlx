using System.Collections.Generic;
using System.Linq;

namespace TetrisCubeDlx
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}
