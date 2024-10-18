using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VirtueSky.Linq
{
    //Takes a comparer, and creates a reverse comparer, for Descending sorts
    internal sealed class ComparerReverser<T> : IComparer<T>
    {
        private readonly IComparer<T> _wrappedComparer;

        public ComparerReverser(IComparer<T> wrappedComparer)
        {
            this._wrappedComparer = wrappedComparer;
        }
#if !(UNITY_4 || UNITY_5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Compare(T x, T y)
        {
            return _wrappedComparer.Compare(y, x);
        }
    }


    internal static class ComparerExtensions
    {
        // Lets us reverse a comparere with comparer.Reverse();
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return new ComparerReverser<T>(comparer);
        }
    }

    internal sealed class LambdaComparer<T, TU> : IComparer<T>
    {
        IComparer<TU> _comparer;
        Func<T, TU> _selector;

        public LambdaComparer(Func<T, TU> selector, IComparer<TU> comparer)
        {
            this._comparer = comparer;
            this._selector = selector;
        }
#if !(UNITY_4 || UNITY_5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Compare(T x, T y)
        {
            return _comparer.Compare(_selector(x), _selector(y));
        }
    }

    internal sealed class ReverseLambdaComparer<T, TU> : IComparer<T>
    {
        IComparer<TU> _comparer;
        Func<T, TU> _selector;

        public ReverseLambdaComparer(Func<T, TU> selector, IComparer<TU> comparer)
        {
            this._comparer = comparer;
            this._selector = selector;
        }
#if !(UNITY_4 || UNITY_5)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public int Compare(T x, T y)
        {
            return _comparer.Compare(_selector(y), _selector(x));
        }
    }
}