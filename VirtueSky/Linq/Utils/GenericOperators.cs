using System;
using System.Runtime.CompilerServices;

/*

    C# has no way to constrain a generic to only primitive types.
    You can only constrain them to value types via:
        where T : struct
    All primitives are value types but not all value types are
    primitives. Thus, when you try to do operations like + > < ==
    on generic value types, you get a compiler error.  These methods
    work around that by checking the type, casting, and performing the
    operation.  The JIT elides all of the non relevant bits of the If statement
    for each generic type, so this is remarkably still fast.  Since these are
    only used for the SIMD specific operations, and the SIMD library
    works exactly this way as well, it does not add any problems that
    don't already exist when you use .NET SIMD.  Which is, if you create a
    Vector<T> where T is a non primitive value type, you will get a runtime error.
 */
namespace VirtueSky.Linq
{
    internal static class GenericOperators
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(T a, T b)
        {
            if (typeof(T) == typeof(byte))
            {
                return (T)(object)((byte)(object)a + (byte)(object)b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return (T)(object)((sbyte)(object)a + (sbyte)(object)b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return (T)(object)((ushort)(object)a + (ushort)(object)b);
            }

            if (typeof(T) == typeof(short))
            {
                return (T)(object)((short)(object)a + (short)(object)b);
            }

            if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)a + (uint)(object)b);
            }

            if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)a + (int)(object)b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return (T)(object)((ulong)(object)a + (ulong)(object)b);
            }

            if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)a + (long)(object)b);
            }

            if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)a + (float)(object)b);
            }

            if (typeof(T) == typeof(double))
            {
                return (T)(object)((double)(object)a + (double)(object)b);
            }

            throw new NotSupportedException("Nope");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan<T>(T a, T b)
        {
            if (typeof(T) == typeof(byte))
            {
                return ((byte)(object)a > (byte)(object)b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return ((sbyte)(object)a > (sbyte)(object)b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return ((ushort)(object)a > (ushort)(object)b);
            }

            if (typeof(T) == typeof(short))
            {
                return ((short)(object)a > (short)(object)b);
            }

            if (typeof(T) == typeof(uint))
            {
                return ((uint)(object)a > (uint)(object)b);
            }

            if (typeof(T) == typeof(int))
            {
                return ((int)(object)a > (int)(object)b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return ((ulong)(object)a > (ulong)(object)b);
            }

            if (typeof(T) == typeof(long))
            {
                return ((long)(object)a > (long)(object)b);
            }

            if (typeof(T) == typeof(float))
            {
                return ((float)(object)a > (float)(object)b);
            }

            if (typeof(T) == typeof(double))
            {
                return ((double)(object)a > (double)(object)b);
            }

            throw new NotSupportedException("Nope");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T a, T b)
        {
            if (typeof(T) == typeof(byte))
            {
                return ((byte)(object)a == (byte)(object)b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return ((sbyte)(object)a == (sbyte)(object)b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return ((ushort)(object)a == (ushort)(object)b);
            }

            if (typeof(T) == typeof(short))
            {
                return ((short)(object)a == (short)(object)b);
            }

            if (typeof(T) == typeof(uint))
            {
                return ((uint)(object)a == (uint)(object)b);
            }

            if (typeof(T) == typeof(int))
            {
                return ((int)(object)a == (int)(object)b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return ((ulong)(object)a == (ulong)(object)b);
            }

            if (typeof(T) == typeof(long))
            {
                return ((long)(object)a == (long)(object)b);
            }

            if (typeof(T) == typeof(float))
            {
                return ((float)(object)a == (float)(object)b);
            }

            if (typeof(T) == typeof(double))
            {
                return ((double)(object)a == (double)(object)b);
            }

            throw new NotSupportedException("Nope");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan<T>(T a, T b)
        {
            if (typeof(T) == typeof(byte))
            {
                return ((byte)(object)a < (byte)(object)b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return ((sbyte)(object)a < (sbyte)(object)b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return ((ushort)(object)a < (ushort)(object)b);
            }

            if (typeof(T) == typeof(short))
            {
                return ((short)(object)a < (short)(object)b);
            }

            if (typeof(T) == typeof(uint))
            {
                return ((uint)(object)a < (uint)(object)b);
            }

            if (typeof(T) == typeof(int))
            {
                return ((int)(object)a < (int)(object)b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return ((ulong)(object)a < (ulong)(object)b);
            }

            if (typeof(T) == typeof(long))
            {
                return ((long)(object)a < (long)(object)b);
            }

            if (typeof(T) == typeof(float))
            {
                return ((float)(object)a < (float)(object)b);
            }

            if (typeof(T) == typeof(double))
            {
                return ((double)(object)a < (double)(object)b);
            }

            throw new NotSupportedException("Nope");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Divide<T>(T a, double b)
        {
            if (typeof(T) == typeof(byte))
            {
                return (double)(object)((byte)(object)a / b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return (double)(object)((sbyte)(object)a / b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return (double)(object)((ushort)(object)a / b);
            }

            if (typeof(T) == typeof(short))
            {
                return (double)(object)((short)(object)a / b);
            }

            if (typeof(T) == typeof(uint))
            {
                return (double)(object)((uint)(object)a / b);
            }

            if (typeof(T) == typeof(int))
            {
                return (double)(object)((int)(object)a / b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return (double)(object)((ulong)(object)a / b);
            }

            if (typeof(T) == typeof(long))
            {
                return (double)(object)((long)(object)a / b);
            }

            if (typeof(T) == typeof(float))
            {
                return (double)(object)((float)(object)a / b);
            }

            if (typeof(T) == typeof(double))
            {
                return (double)(object)((double)(object)a / b);
            }

            throw new NotSupportedException("Nope");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DivideFloat<T>(T a, float b)
        {
            if (typeof(T) == typeof(byte))
            {
                return (float)(object)((byte)(object)a / b);
            }

            if (typeof(T) == typeof(sbyte))
            {
                return (float)(object)((sbyte)(object)a / b);
            }

            if (typeof(T) == typeof(ushort))
            {
                return (float)(object)((ushort)(object)a / b);
            }

            if (typeof(T) == typeof(short))
            {
                return (float)(object)((short)(object)a / b);
            }

            if (typeof(T) == typeof(uint))
            {
                return (float)(object)((uint)(object)a / b);
            }

            if (typeof(T) == typeof(int))
            {
                return (float)(object)((int)(object)a / b);
            }

            if (typeof(T) == typeof(ulong))
            {
                return (float)(object)((ulong)(object)a / b);
            }

            if (typeof(T) == typeof(long))
            {
                return (float)(object)((long)(object)a / b);
            }

            if (typeof(T) == typeof(float))
            {
                return (float)(object)((float)(object)a / b);
            }

            if (typeof(T) == typeof(double))
            {
                return (float)(object)((double)(object)a / b);
            }

            throw new NotSupportedException("Nope");
        }
    }
}