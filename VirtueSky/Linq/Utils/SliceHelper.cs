using System;

namespace VirtueSky.Linq
{
    public static class SliceHelper
    {
#if UNITY_2021_3_OR_NEWER
        public static Span<T> Slice<T>(this T[] array, int start, int len)
        {
            return array.AsSpan().Slice(start, len);
        }
#endif
    }
}