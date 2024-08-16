using System.Runtime.CompilerServices;
using UnityEngine;

namespace VirtueSky.Misc
{
    public partial class Common
    {
        #region Constants

        /// <summary>The circle constant. Defined as the circumference of a circle divided by its radius. Equivalent to 2*pi</summary>
        public const float TWO_PI = 6.28318530717959f;

        /// <summary>An obscure circle constant. Defined as the circumference of a circle divided by its diameter. Equivalent to 0.5*tau</summary>
        public const float PI = 3.14159265358979f;

        // PI / 2 OR 90 deg
        public const float PI_2 = 1.5707963267949f;

        // PI / 3 OR 60 deg
        public const float PI_3 = 1.04719755119659666667f;

        // PI / 4 OR 45 deg
        public const float PI_4 = 0.785398163397448f;

        // PI / 8 OR 22.5 deg
        public const float PI_8 = 0.392699081698724f;

        // PI / 16 OR 11.25 deg
        public const float PI_16 = 0.196349540849362f;

        // 3 * PI_2 OR 270 deg
        public const float THREE_PI_2 = 4.71238898038469f;

        /// <summary>Euler's number. The base of the natural logarithm. f(x)=e^x is equal to its own derivative</summary>
        public const float E = 2.71828182845905f;

        /// <summary>The golden ratio. It is the value of a/b where a/b = (a+b)/a. It's the positive root of x^2-x-1</summary>
        public const float GOLDEN_RATIO = 1.61803398875f;

        /// <summary>The square root of two. The length of the vector (1,1)</summary>
        public const float SQRT2 = 1.4142135623731f;

        /// <summary>The reciprocal of the square root of two. The components of the vector (1,1)</summary>
        public const float RSQRT2 = 1f / SQRT2;

        /// <summary>Multiply an angle in degrees by this, to convert it to radians (2pi/360)</summary>
        public const float DEG_TO_RAD = 0.0174532925199433f;

        /// <summary>Multiply an angle in radians by this, to convert it to degrees (360/2pi)</summary>
        public const float RAD_TO_DEG = 57.2957795130823f;

        public const double DBL_EPSILON = 9.99999943962493E-11;

        #endregion

        #region Math operations

        /// <summary>Returns the square root of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(this float value) => (float)System.Math.Sqrt(value);

        /// <summary>Returns the square root of each component</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Sqrt(this Vector2 v) => new Vector2(Sqrt(v.x), Sqrt(v.y));

        /// <inheritdoc cref="Math.Sqrt(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Sqrt(this Vector3 v) => new Vector3(Sqrt(v.x), Sqrt(v.y), Sqrt(v.z));

        /// <inheritdoc cref="Math.Sqrt(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Sqrt(this Vector4 v) => new Vector4(Sqrt(v.x), Sqrt(v.y), Sqrt(v.z), Sqrt(v.w));

        /// <summary>Returns <c>value</c> raised to the power of <c>exponent</c></summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(this float value, float exponent) => (float)System.Math.Pow(value, exponent);

        /// <summary>Returns e to the power of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(this float power) => (float)System.Math.Exp(power);

        /// <summary>Returns the logarithm of a value, with the given base</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this float value, float @base) => (float)System.Math.Log(value, @base);

        /// <summary>Returns the natural logarithm of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this float value) => (float)System.Math.Log(value);

        /// <summary>Returns the base 10 logarithm of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(this float value) => (float)System.Math.Log10(value);

        #endregion

        #region Floating Point

        /// <summary>A very small value, used for various floating point inaccuracy thresholds</summary>
        public static readonly float Epsilon = UnityEngineInternal.MathfInternal.IsFlushToZeroEnabled
            ? UnityEngineInternal.MathfInternal.FloatMinNormal
            : UnityEngineInternal.MathfInternal.FloatMinDenormal;

        /// <summary>float.PositiveInfinity</summary>
        public const float Infinity = float.PositiveInfinity;

        /// <summary>float.NegativeInfinity</summary>
        public const float NegativeInfinity = float.NegativeInfinity;

        /// <summary>Returns whether or not two values are approximately equal.
        /// They are considered equal if they are within a <c>M.Epsilon*8</c> or <c>max(a,b)*0.000001f</c> range of each other</summary>
        /// <param name="a">The first value to compare</param>
        /// <param name="b">The second value to compare</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this float a, float b) =>
            Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), Epsilon * 8);

        /// <inheritdoc cref="Approximately(float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this Vector2 a, Vector2 b) =>
            Approximately(a.x, b.x) && Approximately(a.y, b.y);

        /// <inheritdoc cref="Approximately(float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this Vector3 a, Vector3 b) =>
            Approximately(a.x, b.x) && Approximately(a.y, b.y) && Approximately(a.z, b.z);

        /// <inheritdoc cref="Approximately(float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this Vector4 a, Vector4 b) =>
            Approximately(a.x, b.x) && Approximately(a.y, b.y) && Approximately(a.z, b.z) && Approximately(a.w, b.w);

        /// <inheritdoc cref="Approximately(float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(this Color a, Color b) =>
            Approximately(a.r, b.r) && Approximately(a.g, b.g) && Approximately(a.b, b.b) && Approximately(a.a, b.a);

        #endregion

        #region Trigonometry

        /// <summary>Returns the cosine of the given angle. Equivalent to the x-component of a unit vector with the same angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(this float angRad) => (float)System.Math.Cos(angRad);

        /// <summary>Returns the sine of the given angle. Equivalent to the y-component of a unit vector with the same angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(this float angRad) => (float)System.Math.Sin(angRad);

        /// <summary>Returns the tangent of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(this float angRad) => (float)System.Math.Tan(angRad);

        /// <summary>Returns the arc cosine of the given value, in radians</summary>
        /// <param name="value">A value between -1 and 1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(this float value) => (float)System.Math.Acos(value);

        /// <summary>Returns the arc sine of the given value, in radians</summary>
        /// <param name="value">A value between -1 and 1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(this float value) => (float)System.Math.Asin(value);

        /// <summary>Returns the arc tangent of the given value, in radians</summary>
        /// <param name="value">A value between -1 and 1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(this float value) => (float)System.Math.Atan(value);

        /// <summary>Returns the angle of a vector. I don't recommend using this function, it's confusing~ Use M.DirToAng instead</summary>
        /// <param name="y">The y component of the vector. They're flipped yeah I know but this is how everyone implements if for some godforsaken reason</param>
        /// <param name="x">The x component of the vector. They're flipped yeah I know but this is how everyone implements if for some godforsaken reason</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan2(this float y, float x) => (float)System.Math.Atan2(y, x);

        /// <summary>Returns the cosecant of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Csc(this float angRad) => 1f / (float)System.Math.Sin(angRad);

        /// <summary>Returns the secant of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sec(this float angRad) => 1f / (float)System.Math.Cos(angRad);

        /// <summary>Returns the cotangent of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cot(this float angRad) => 1f / (float)System.Math.Tan(angRad);

        /// <summary>Returns the versine of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ver(this float angRad) => 1 - (float)System.Math.Cos(angRad);

        /// <summary>Returns the coversine of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cvs(this float angRad) => 1 - (float)System.Math.Sin(angRad);

        /// <summary>Returns the chord of the given angle</summary>
        /// <param name="angRad">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Crd(this float angRad) => 2 * (float)System.Math.Sin(angRad / 2);

        #endregion

        #region Hyperbolic Trigonometry

        /// <summary>Returns the hyperbolic cosine of the given hyperbolic angle</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cosh(this float x) => (float)System.Math.Cosh(x);

        /// <summary>Returns the hyperbolic sine of the given hyperbolic angle</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sinh(this float x) => (float)System.Math.Sinh(x);

        /// <summary>Returns the hyperbolic tangent of the given hyperbolic angle</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tanh(this float x) => (float)System.Math.Tanh(x);

        /// <summary>Returns the hyperbolic arc cosine of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acosh(this float x) => (float)System.Math.Log(x + Mathf.Sqrt(x * x - 1));

        /// <summary>Returns the hyperbolic arc sine of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asinh(this float x) => (float)System.Math.Log(x + Mathf.Sqrt(x * x + 1));

        /// <summary>Returns the hyperbolic arc tangent of the given value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atanh(this float x) => (float)(0.5 * System.Math.Log((1 + x) / (1 - x)));

        #endregion

        #region Absolute Values

        /// <summary>Returns the absolute value. Basically makes negative numbers positive</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(this float value) => System.Math.Abs(value);

        /// <inheritdoc cref="Math.Abs(float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Abs(this int value) => System.Math.Abs(value);

        /// <summary>Returns the absolute value, per component. Basically makes negative numbers positive</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Abs(this Vector2 v) => new Vector2(Abs(v.x), Abs(v.y));

        /// <inheritdoc cref="Math.Abs(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Abs(this Vector3 v) => new Vector3(Abs(v.x), Abs(v.y), Abs(v.z));

        /// <inheritdoc cref="Math.Abs(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Abs(this Vector4 v) => new Vector4(Abs(v.x), Abs(v.y), Abs(v.z), Abs(v.w));

        #endregion

        #region Clamping

        /// <summary>Returns the value clamped between <c>min</c> and <c>max</c></summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public static float Clamp(this float value, float min, float max) =>
            value < min ? min : value > max ? max : value;

        /// <summary>Clamps each component between <c>min</c> and <c>max</c></summary>
        public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max) =>
            new Vector2(v.x < min.x ? min.x : v.x > max.x ? max.x : v.x,
                v.y < min.y ? min.y : v.y > max.y ? max.y : v.y);

        /// <inheritdoc cref="Math.Clamp(Vector2,Vector2,Vector2)"/>
        public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max) =>
            new Vector3(v.x < min.x ? min.x : v.x > max.x ? max.x : v.x,
                v.y < min.y ? min.y : v.y > max.y ? max.y : v.y,
                v.z < min.z ? min.z : v.z > max.z ? max.z : v.z);

        /// <inheritdoc cref="Math.Clamp(Vector2,Vector2,Vector2)"/>
        public static Vector4 Clamp(this Vector4 v, Vector4 min, Vector4 max) =>
            new Vector4(v.x < min.x ? min.x : v.x > max.x ? max.x : v.x,
                v.y < min.y ? min.y : v.y > max.y ? max.y : v.y,
                v.z < min.z ? min.z : v.z > max.z ? max.z : v.z,
                v.w < min.w ? min.w : v.w > max.w ? max.w : v.w);

        /// <inheritdoc cref="Math.Clamp(float,float,float)"/>
        public static int Clamp(this int value, int min, int max) => value < min ? min : value > max ? max : value;

        /// <summary>Returns the value clamped between 0 and 1</summary>
        public static float Clamp01(this float value) => value < 0f ? 0f : value > 1f ? 1f : value;

        /// <summary>Clamps each component between 0 and 1</summary>
        public static Vector2 Clamp01(this Vector2 v) =>
            new Vector2(v.x < 0f ? 0f : v.x > 1f ? 1f : v.x, v.y < 0f ? 0f : v.y > 1f ? 1f : v.y);

        /// <inheritdoc cref="Math.Clamp01(Vector2)"/>
        public static Vector3 Clamp01(this Vector3 v) =>
            new Vector3(v.x < 0f ? 0f : v.x > 1f ? 1f : v.x, v.y < 0f ? 0f : v.y > 1f ? 1f : v.y,
                v.z < 0f ? 0f : v.z > 1f ? 1f : v.z);

        /// <inheritdoc cref="Math.Clamp01(Vector2)"/>
        public static Vector4 Clamp01(this Vector4 v) =>
            new Vector4(v.x < 0f ? 0f : v.x > 1f ? 1f : v.x,
                v.y < 0f ? 0f : v.y > 1f ? 1f : v.y,
                v.z < 0f ? 0f : v.z > 1f ? 1f : v.z,
                v.w < 0f ? 0f : v.w > 1f ? 1f : v.w);

        /// <summary>Clamps the value between -1 and 1</summary>
        public static float ClampNeg1To1(this float value) => value < -1f ? -1f : value > 1f ? 1f : value;

        /// <summary>Clamps each component between -1 and 1</summary>
        public static Vector2 ClampNeg1To1(this Vector2 v) => new Vector2(v.x < -1f ? -1f : v.x > 1f ? 1f : v.x,
            v.y < -1f ? -1f : v.y > 1f ? 1f : v.y);

        /// <summary>Clamps each component between -1 and 1</summary>
        public static Vector3 ClampNeg1To1(this Vector3 v) =>
            new Vector3(v.x < -1f ? -1f : v.x > 1f ? 1f : v.x, v.y < -1f ? -1f : v.y > 1f ? 1f : v.y,
                v.z < -1f ? -1f : v.z > 1f ? 1f : v.z);

        /// <summary>Clamps each component between -1 and 1</summary>
        public static Vector4 ClampNeg1To1(this Vector4 v) =>
            new Vector4(v.x < -1f ? -1f : v.x > 1f ? 1f : v.x,
                v.y < -1f ? -1f : v.y > 1f ? 1f : v.y,
                v.z < -1f ? -1f : v.z > 1f ? 1f : v.z,
                v.w < -1f ? -1f : v.w > 1f ? 1f : v.w);

        #endregion

        #region Min & Max

        /// <summary>Returns the smallest of the two values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float a, float b) => a < b ? a : b;

        /// <summary>Returns the smallest of the three values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float a, float b, float c) => Min(Min(a, b), c);

        /// <summary>Returns the smallest of the four values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float a, float b, float c, float d) => Min(Min(a, b), Min(c, d));

        /// <summary>Returns the largest of the two values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float a, float b) => a > b ? a : b;

        /// <summary>Returns the largest of the three values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float a, float b, float c) => Max(Max(a, b), c);

        /// <summary>Returns the largest of the four values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float a, float b, float c, float d) => Max(Max(a, b), Max(c, d));

        /// <summary>Returns the smallest of the two values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(this int a, int b) => a < b ? a : b;

        /// <summary>Returns the smallest of the three values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(this int a, int b, int c) => Min(Min(a, b), c);

        /// <summary>Returns the smallest of the four values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(this int a, int b, int c, int d) => Min(Min(a, b), Min(c, d));

        /// <summary>Returns the largest of the two values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(this int a, int b) => a > b ? a : b;

        /// <summary>Returns the largest of the three values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(this int a, int b, int c) => Max(Max(a, b), c);

        /// <summary>Returns the largest of the four values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(this int a, int b, int c, int d) => Max(Max(a, b), Max(c, d));

        private static float MinCopyLinq(float[] source)
        {
            float r = float.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
                else if (float.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        private static int MinCopyLinq(int[] source)
        {
            int r = int.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        private static int MaxCopyLinq(int[] source)
        {
            int r = int.MinValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] > r) r = source[i];
            }

            return r;
        }

        private static float MaxCopyLinq(float[] source)
        {
            float r = source[0];
            int startIndex = 0;
            for (; startIndex < source.Length; startIndex++)
            {
                if (!float.IsNaN(source[startIndex]))
                {
                    r = source[startIndex];
                    break;
                }
            }

            for (int i = startIndex; i < source.Length; i++)
            {
                if (source[i] > r) r = source[i];
            }

            return r;
        }

        /// <summary>Returns the smallest of the given values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(params float[] values) => MinCopyLinq(values);

        /// <summary>Returns the largest of the given values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(params float[] values) => MaxCopyLinq(values);

        /// <summary>Returns the smallest of the given values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(params int[] values) => MinCopyLinq(values);

        /// <summary>Returns the largest of the given values</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(params int[] values) => MaxCopyLinq(values);

        /// <summary>Returns the minimum value of all components in the vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this Vector2 v) => Min(v.x, v.y);

        /// <inheritdoc cref="Math.Min(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this Vector3 v) => Min(v.x, v.y, v.z);

        /// <inheritdoc cref="Math.Min(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this Vector4 v) => Min(v.x, v.y, v.z, v.w);

        /// <summary>Returns the maximum value of all components in the vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this Vector2 v) => Max(v.x, v.y);

        /// <inheritdoc cref="Math.Max(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this Vector3 v) => Max(v.x, v.y, v.z);

        /// <inheritdoc cref="Math.Max(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this Vector4 v) => Max(v.x, v.y, v.z, v.w);

        #endregion

        #region Rounding

        /// <summary>Rounds the value down to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(this float value) => (float)System.Math.Floor(value);

        /// <summary>Rounds the vector components down to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Floor(this Vector2 value) =>
            new Vector2((float)System.Math.Floor(value.x), (float)System.Math.Floor(value.y));

        /// <inheritdoc cref="Math.Floor(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Floor(this Vector3 value) =>
            new Vector3((float)System.Math.Floor(value.x), (float)System.Math.Floor(value.y),
                (float)System.Math.Floor(value.z));

        /// <inheritdoc cref="Math.Floor(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Floor(this Vector4 value) =>
            new Vector4((float)System.Math.Floor(value.x), (float)System.Math.Floor(value.y),
                (float)System.Math.Floor(value.z), (float)System.Math.Floor(value.w));

        /// <summary>Rounds the value down to the nearest integer, returning an int value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this float value) => (int)System.Math.Floor(value);

        /// <summary>Rounds the vector components down to the nearest integer, returning an integer vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int FloorToInt(this Vector2 value) =>
            new Vector2Int((int)System.Math.Floor(value.x), (int)System.Math.Floor(value.y));

        /// <inheritdoc cref="Math.FloorToInt(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int FloorToInt(this Vector3 value) =>
            new Vector3Int((int)System.Math.Floor(value.x), (int)System.Math.Floor(value.y),
                (int)System.Math.Floor(value.z));

        /// <summary>Rounds the value up to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceil(this float value) => (float)System.Math.Ceiling(value);

        /// <summary>Rounds the vector components up to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Ceil(this Vector2 value) =>
            new Vector2((float)System.Math.Ceiling(value.x), (float)System.Math.Ceiling(value.y));

        /// <inheritdoc cref="Math.Ceil(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Ceil(this Vector3 value) =>
            new Vector3((float)System.Math.Ceiling(value.x), (float)System.Math.Ceiling(value.y),
                (float)System.Math.Ceiling(value.z));

        /// <inheritdoc cref="Math.Ceil(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Ceil(this Vector4 value) =>
            new Vector4((float)System.Math.Ceiling(value.x),
                (float)System.Math.Ceiling(value.y),
                (float)System.Math.Ceiling(value.z),
                (float)System.Math.Ceiling(value.w));

        /// <summary>Rounds the value up to the nearest integer, returning an int value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(this float value) => (int)System.Math.Ceiling(value);

        /// <summary>Rounds the vector components up to the nearest integer, returning an integer vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CeilToInt(this Vector2 value) =>
            new Vector2Int((int)System.Math.Ceiling(value.x), (int)System.Math.Ceiling(value.y));

        /// <inheritdoc cref="Math.CeilToInt(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CeilToInt(this Vector3 value) =>
            new Vector3Int((int)System.Math.Ceiling(value.x), (int)System.Math.Ceiling(value.y),
                (int)System.Math.Ceiling(value.z));

        /// <summary>Rounds the value to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(this float value) => (float)System.Math.Round(value);

        /// <summary>Rounds the vector components to the nearest integer</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Round(this Vector2 value) =>
            new Vector2((float)System.Math.Round(value.x), (float)System.Math.Round(value.y));

        /// <inheritdoc cref="Math.Round(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Round(this Vector3 value) =>
            new Vector3((float)System.Math.Round(value.x), (float)System.Math.Round(value.y),
                (float)System.Math.Round(value.z));

        /// <inheritdoc cref="Math.Round(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Round(this Vector4 value) =>
            new Vector4((float)System.Math.Round(value.x), (float)System.Math.Round(value.y),
                (float)System.Math.Round(value.z), (float)System.Math.Round(value.w));

        /// <summary>Rounds the value to the nearest value, snapped to the given interval size</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(this float value, float snapInterval) =>
            Mathf.Round(value / snapInterval) * snapInterval;

        /// <summary>Rounds the vector components to the nearest value, snapped to the given interval size</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Round(this Vector2 value, float snapInterval) =>
            new Vector2(Round(value.x, snapInterval), Round(value.y, snapInterval));

        /// <inheritdoc cref="Math.Round(Vector2,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Round(this Vector3 value, float snapInterval) =>
            new Vector3(Round(value.x, snapInterval), Round(value.y, snapInterval), Round(value.z, snapInterval));

        /// <inheritdoc cref="Math.Round(Vector2,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Round(this Vector4 value, float snapInterval) =>
            new Vector4(Round(value.x, snapInterval), Round(value.y, snapInterval), Round(value.z, snapInterval),
                Round(value.w, snapInterval));

        /// <summary>Rounds the value to the nearest integer, returning an int value</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this float value) => (int)System.Math.Round(value);

        /// <summary>Rounds the vector components to the nearest integer, returning an integer vector</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int RoundToInt(this Vector2 value) =>
            new Vector2Int((int)System.Math.Round(value.x), (int)System.Math.Round(value.y));

        /// <inheritdoc cref="Math.RoundToInt(Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int RoundToInt(this Vector3 value) =>
            new Vector3Int((int)System.Math.Round(value.x), (int)System.Math.Round(value.y),
                (int)System.Math.Round(value.z));

        #endregion

        #region Repeat

        /// <summary>Repeats the given value in the interval specified by length</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(this float value, float length) =>
            Clamp(value - Floor(value / length) * length, 0.0f, length);

        /// <summary>Repeats a value within a range, going back and forth</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PingPong(float t, float length) => length - Abs(Repeat(t, length * 2f) - length);

        #endregion

        #region Smoothing

        /// <summary>Applies cubic smoothing to the 0-1 interval, also known as the smoothstep function. Similar to an EaseInOut operation</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Smooth01(float x) => x * x * (3 - 2 * x);

        /// <summary>Applies quintic smoothing to the 0-1 interval, also known as the smootherstep function. Similar to an EaseInOut operation</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Smoother01(float x) => x * x * x * (x * (x * 6 - 15) + 10);

        /// <summary>Applies trigonometric smoothing to the 0-1 interval. Similar to an EaseInOut operation</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SmoothCos01(float x) => Cos(x * PI) * -0.5f + 0.5f;

        /// <summary>Applies a gamma curve or something idk I've never used this function before but it was part of Unity's original M.cs and it's undocumented</summary>
        public static float Gamma(float value, float absmax, float gamma)
        {
            bool negative = value < 0F;
            float absval = Abs(value);
            if (absval > absmax)
                return negative ? -absval : absval;

            float result = Pow(absval / absmax, gamma) * absmax;
            return negative ? -result : result;
        }

        /// <summary>Gradually changes a value towards a desired goal over time.
        /// The value is smoothed by some spring-damper like function, which will never overshoot.
        /// The function can be used to smooth any kind of value, positions, colors, scalars</summary>
        /// <param name="current">The current position</param>
        /// <param name="target">The position we are trying to reach</param>
        /// <param name="currentVelocity">The current velocity, this value is modified by the function every time you call it</param>
        /// <param name="smoothTime">Approximately the time it will take to reach the target. A smaller value will reach the target faster</param>
        /// <param name="maxSpeed">	Optionally allows you to clamp the maximum speed</param>
        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime,
            float maxSpeed = Infinity)
        {
            float deltaTime = Time.deltaTime;
            return SmoothDamp(current,
                target,
                ref currentVelocity,
                smoothTime,
                maxSpeed,
                deltaTime);
        }

        /// <summary>Gradually changes a value towards a desired goal over time.
        /// The value is smoothed by some spring-damper like function, which will never overshoot.
        /// The function can be used to smooth any kind of value, positions, colors, scalars</summary>
        /// <param name="current">The current position</param>
        /// <param name="target">The position we are trying to reach</param>
        /// <param name="currentVelocity">The current velocity, this value is modified by the function every time you call it</param>
        /// <param name="smoothTime">Approximately the time it will take to reach the target. A smaller value will reach the target faster</param>
        /// <param name="maxSpeed">	Optionally allows you to clamp the maximum speed</param>
        /// <param name="deltaTime">The time since the last call to this function. By default Time.deltaTime</param>
        public static float SmoothDamp(
            float current,
            float target,
            ref float currentVelocity,
            float smoothTime,
            [UnityEngine.Internal.DefaultValue("Mathf.Infinity")]
            float maxSpeed,
            [UnityEngine.Internal.DefaultValue("Time.deltaTime")]
            float deltaTime)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Mathf.Max(0.0001F, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * deltaTime;
            float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
            float change = current - target;
            float originalTo = target;

            // Clamp maximum speed
            float maxChange = maxSpeed * smoothTime;
            change = Mathf.Clamp(change, -maxChange, maxChange);
            target = current - change;

            float temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            float output = target + (change + temp) * exp;

            // Prevent overshooting
            if (originalTo - current > 0.0F == output > originalTo)
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }

        #endregion

        #region Value & Vector interpolation

        /// <summary>Blends between a and b, based on the t-value. When t = 0 it returns a, when t = 1 it returns b, and any values between are blended linearly </summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the lerp</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t) => (1f - t) * a + t * b;

        /// <summary>Blends between a and b of each component, based on the t-value of each component in the t-vector. When t = 0 it returns a, when t = 1 it returns b, and any values between are blended linearly </summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-values from 0 to 1 representing position along the lerp</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t) =>
            new Vector2(Lerp(a.x, b.x, t.x), Lerp(a.y, b.y, t.y));

        /// <inheritdoc cref="Math.Lerp(Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 a, Vector3 b, Vector3 t) =>
            new Vector3(Lerp(a.x, b.x, t.x), Lerp(a.y, b.y, t.y), Lerp(a.z, b.z, t.z));

        /// <inheritdoc cref="Math.Lerp(Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Lerp(Vector4 a, Vector4 b, Vector4 t) => new Vector4(Lerp(a.x, b.x, t.x),
            Lerp(a.y, b.y, t.y), Lerp(a.z, b.z, t.z), Lerp(a.w, b.w, t.w));

        /// <summary>Linearly blends between two rectangles, moving and resizing from the center. Note: this lerp is unclamped</summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-values from 0 to 1 representing position along the lerp</param>
        public static Rect Lerp(Rect a, Rect b, float t)
        {
            Vector2 center = Vector2.LerpUnclamped(a.center, b.center, t);
            Vector2 size = Vector2.LerpUnclamped(a.size, b.size, t);
            return new Rect(default, size) { center = center };
        }

        /// <summary>Blends between a and b, based on the t-value. When t = 0 it returns a, when t = 1 it returns b, and any values between are blended linearly</summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the lerp, clamped between 0 and 1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpClamped(float a, float b, float t) => Lerp(a, b, Clamp01(t));

        /// <summary>Lerps between a and b, applying cubic smoothing to the t-value</summary>
        /// <param name="a">The start value, when t is 0</param>
        /// <param name="b">The start value, when t is 1</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the lerp, clamped between 0 and 1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpSmooth(float a, float b, float t) => Lerp(a, b, Smooth01(Clamp01(t)));

        /// <summary>Given a value between a and b, returns its normalized location in that range, as a t-value (interpolant) from 0 to 1</summary>
        /// <param name="a">The start of the range, where it would return 0</param>
        /// <param name="b">The end of the range, where it would return 1</param>
        /// <param name="value">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InverseLerp(float a, float b, float value) => (value - a) / (b - a);

        /// <summary>Given a value between a and b, returns its normalized location in that range, as a t-value (interpolant) from 0 to 1.
        /// This safe version returns 0 if a == b, instead of a division by zero</summary>
        /// <param name="a">The start of the range, where it would return 0</param>
        /// <param name="b">The end of the range, where it would return 1</param>
        /// <param name="value">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InverseLerpSafe(float a, float b, float value)
        {
            float den = b - a;
            if (den == 0)
                return 0;
            return (value - a) / den;
        }

        /// <summary>Given values between a and b in each component, returns their normalized locations in the given ranges, as t-values (interpolants) from 0 to 1</summary>
        /// <param name="a">The start of the ranges, where it would return 0</param>
        /// <param name="b">The end of the ranges, where it would return 1</param>
        /// <param name="v">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 v) =>
            new Vector2((v.x - a.x) / (b.x - a.x), (v.y - a.y) / (b.y - a.y));

        /// <inheritdoc cref="Math.InverseLerp(Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InverseLerp(Vector3 a, Vector3 b, Vector3 v) =>
            new Vector3((v.x - a.x) / (b.x - a.x), (v.y - a.y) / (b.y - a.y), (v.z - a.z) / (b.z - a.z));

        /// <inheritdoc cref="Math.InverseLerp(Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 InverseLerp(Vector4 a, Vector4 b, Vector4 v) =>
            new Vector4((v.x - a.x) / (b.x - a.x), (v.y - a.y) / (b.y - a.y), (v.z - a.z) / (b.z - a.z),
                (v.w - a.w) / (b.w - a.w));

        /// <summary>Given a value between a and b, returns its normalized location in that range, as a t-value (interpolant) clamped between 0 and 1</summary>
        /// <param name="a">The start of the range, where it would return 0</param>
        /// <param name="b">The end of the range, where it would return 1</param>
        /// <param name="value">A value between a and b</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InverseLerpClamped(float a, float b, float value) => Clamp01((value - a) / (b - a));

        /// <summary>Given a value between a and b, returns its normalized location in that range, as a t-value (interpolant) from 0 to 1, with cubic smoothing applied.
        /// Equivalent to "smoothstep" in shader code</summary>
        /// <param name="a">The start of the range, where it would return 0</param>
        /// <param name="b">The end of the range, where it would return 1</param>
        /// <param name="value">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InverseLerpSmooth(float a, float b, float value) =>
            Smooth01(Clamp01((value - a) / (b - a)));

        /// <summary>Remaps a value from the input range [iMin to iMax] into the output range [oMin to oMax].
        /// Equivalent to Lerp(oMin,oMax,InverseLerp(iMin,iMax,value))</summary>
        /// <param name="iMin">The start value of the input range</param>
        /// <param name="iMax">The end value of the input range</param>
        /// <param name="oMin">The start value of the output range</param>
        /// <param name="oMax">The end value of the output range</param>
        /// <param name="value">The value to remap</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(float iMin, float iMax, float oMin, float oMax, float value) =>
            Lerp(oMin, oMax, InverseLerp(iMin, iMax, value));

        /// <inheritdoc cref="Math.Remap(float,float,float,float,float)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Remap(float iMin, float iMax, float oMin, float oMax, int value) =>
            Lerp(oMin, oMax, InverseLerp(iMin, iMax, value));

        /// <summary>Remaps values from the input range [iMin to iMax] into the output range [oMin to oMax] on a per-component basis.
        /// Equivalent to Lerp(oMin,oMax,InverseLerp(iMin,iMax,value))</summary>
        /// <param name="iMin">The start values of the input ranges</param>
        /// <param name="iMax">The end values of the input ranges</param>
        /// <param name="oMin">The start values of the output ranges</param>
        /// <param name="oMax">The end values of the output ranges</param>
        /// <param name="value">The values to remap</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value) =>
            Lerp(oMin, oMax, InverseLerp(iMin, iMax, value));

        /// <inheritdoc cref="Math.Remap(Vector2,Vector2,Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Remap(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 value) =>
            Lerp(oMin, oMax, InverseLerp(iMin, iMax, value));

        /// <inheritdoc cref="Math.Remap(Vector2,Vector2,Vector2,Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Remap(Vector4 iMin, Vector4 iMax, Vector4 oMin, Vector4 oMax, Vector4 value) =>
            Lerp(oMin, oMax, InverseLerp(iMin, iMax, value));

        /// <summary>Remaps a value from the input range [iMin to iMax] into the output range [oMin to oMax], clamping to make sure it does not extrapolate.
        /// Equivalent to Lerp(oMin,oMax,InverseLerpClamped(iMin,iMax,value))</summary>
        /// <param name="iMin">The start value of the input range</param>
        /// <param name="iMax">The end value of the input range</param>
        /// <param name="oMin">The start value of the output range</param>
        /// <param name="oMax">The end value of the output range</param>
        /// <param name="value">The value to remap</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RemapClamped(float iMin, float iMax, float oMin, float oMax, float value) =>
            Lerp(oMin, oMax, InverseLerpClamped(iMin, iMax, value));

        /// <summary>Remaps a value from the input Rect to the output Rect</summary>
        /// <param name="iRect">The input Rect</param>
        /// <param name="oRect">The output Rect</param>
        /// <param name="iPos">The input position in the input Rect space</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Remap(Rect iRect, Rect oRect, Vector2 iPos) =>
            Remap(iRect.min,
                iRect.max,
                oRect.min,
                oRect.max,
                iPos);

        /// <summary>Remaps a value from the input Bounds to the output Bounds</summary>
        /// <param name="iBounds">The input Bounds</param>
        /// <param name="oBounds">The output Bounds</param>
        /// <param name="iPos">The input position in the input Bounds space</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Remap(Bounds iBounds, Bounds oBounds, Vector3 iPos) =>
            Remap(iBounds.min,
                iBounds.max,
                oBounds.min,
                oBounds.max,
                iPos);

        /// <summary>Exponential interpolation, the multiplicative version of lerp, useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Eerp(float a, float b, float t) => Mathf.Pow(a, 1 - t) * Mathf.Pow(b, t);

        /// <summary>Inverse exponential interpolation, the multiplicative version of InverseLerp, useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="v">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InverseEerp(float a, float b, float v) => Mathf.Log(a / v) / Mathf.Log(a / b);

        #endregion

        #region Angle

        /// <summary>Returns the angle of this vector, in radians</summary>
        /// <param name="v">The vector to get the angle of. It does not have to be normalized</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(this Vector2 v) => Mathf.Atan2(v.y, v.x);

        /// <summary>Get the angle in degrees off the forward defined by x.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(this float x, float y) => Mathf.Atan2(y, x);

        public static Vector2 Reflect(this Vector2 v, Vector2 normal)
        {
            var dp = 2 * Vector2.Dot(v, normal);
            return new Vector2(v.x - normal.x * dp, v.y - normal.y * dp);
        }

        public static void Mirror(this ref Vector2 v, Vector2 axis)
        {
            v = (2 * (Vector2.Dot(v, axis) / Vector2.Dot(axis, axis)) * axis) - v;
        }

        public static Vector2 Mirror(this Vector2 v, Vector2 axis)
        {
            return (2 * (Vector2.Dot(v, axis) / Vector2.Dot(axis, axis)) * axis) - v;
        }

        /// <summary>
        /// Angular interpolates between two vectors.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns>The vectors are 2 dimensional, so technically this is not a spherical linear interpolation. The name Slerp is kept for consistency. 
        /// The result would be if you Slerped between 2 Vector3's that had a z value of 0. The direction interpolates at an angular rate, where as the 
        /// magnitude interpolates at a linear rate.</returns>
        public static Vector2 Slerp(this Vector2 from, Vector2 to, float t)
        {
            var a = NormalizeAngle(Lerp(Atan2(from.y, from.x), Atan2(to.y, to.x), t), true);
            var l = Mathf.LerpUnclamped(from.magnitude, to.magnitude, t);
            return new Vector2(Cos(a) * l, Sin(a) * l);
        }

        /// <summary>
        /// Angular interpolates between two vectors.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns>The vectors are 2 dimensional, so technically this is not a spherical linear interpolation. The name Slerp is kept for consistency. 
        /// The result would be if you Slerped between 2 Vector3's that had a z value of 0. The direction interpolates at an angular rate, where as the 
        /// magnitude interpolates at a linear rate.</returns>
        public static Vector2 SlerpClamped(this Vector2 from, Vector2 to, float t)
        {
            var a = NormalizeAngle(Lerp(Atan2(from.y, from.x), Atan2(to.y, to.x), t), true);
            var l = Lerp(from.magnitude, to.magnitude, t);
            return new Vector2(Cos(a) * l, Sin(a) * l);
        }

        /// <summary>Returns the shortest angle between <c>a</c> and <c>b</c>, in the range 0 to tau/2 (0 to pi)</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleBetween(this Vector2 a, Vector2 b) =>
            Mathf.Acos(Vector2.Dot(a.normalized, b.normalized).ClampNeg1To1());

        /// <inheritdoc cref="AngleBetween(Vector2,Vector2)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleBetween(this Vector3 a, Vector3 b) =>
            Mathf.Acos(Vector3.Dot(a.normalized, b.normalized).ClampNeg1To1());

        /// <summary>
        /// set an angle with in the bounds of -PI to PI
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="useRadians"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static float NormalizeAngle(this float angle, bool useRadians = true)
        {
            float rd = (useRadians ? PI : 180);
            return Wrap(angle, rd, -rd);
        }

        public static float ClampIn180(this float a, bool bUseRadians = false)
        {
            return NormalizeAngle(a, bUseRadians);
        }

        public static float ClampIn360(this float a, bool bUseRadians = false)
        {
            a = NormalizeAngle(a, bUseRadians);
            if (a < 0f) a += (bUseRadians) ? TWO_PI : 360.0f;
            return a;
        }

        /// <summary>Blends between the <c>aRad</c> and <c>bRad</c> angles, based on the input t-value between 0 and 1</summary>
        /// <param name="aRad">The start value, in radians</param>
        /// <param name="bRad">The end value, in radians</param>
        /// <param name="t">The t-value between 0 and 1</param>
        public static float LerpAngle(this float aRad, float bRad, float t)
        {
            float delta = Repeat((bRad - aRad), TWO_PI);
            if (delta > PI)
                delta -= TWO_PI;
            return aRad + delta * Clamp01(t);
        }

        /// <summary>Returns the shortest angle between the two input angles, in radians</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DeltaAngle(this float a, float b) => (b - a + PI).Repeat(TWO_PI) - PI;

        /// <summary>Given an angle between a and b, returns its normalized location in that range, as a t-value (interpolant) from 0 to 1</summary>
        /// <param name="a">The start angle of the range (in radians), where it would return 0</param>
        /// <param name="b">The end angle of the range (in radians), where it would return 1</param>
        /// <param name="v">An angle between a and b</param>
        public static float InverseLerpAngle(this float a, float b, float v)
        {
            float angBetween = DeltaAngle(a, b);
            b = a + angBetween; // removes any a->b discontinuity
            float h = a + angBetween * 0.5f; // halfway angle
            v = h + DeltaAngle(h, v); // get offset from h, and offset by h
            return InverseLerpClamped(a, b, v);
        }

        /// <summary>Gradually changes an angle given in radians towards a desired goal angle over time.
        /// The value is smoothed by some spring-damper like function.
        /// The function can be used to smooth any kind of value, positions, colors, scalars. The most common use is for smoothing a follow camera.</summary>
        /// <param name="current">The current angle</param>
        /// <param name="target">The angle we are trying to reach</param>
        /// <param name="currentVelocity">The current angular velocity, this value is modified by the function every time you call it</param>
        /// <param name="smoothTime">Approximately the time it will take to reach the target. A smaller value will reach the target faster</param>
        /// <param name="maxSpeed">Optionally allows you to clamp the maximum speed</param>
        public static float SmoothDampAngle(this float current, float target, ref float currentVelocity,
            float smoothTime, float maxSpeed = Infinity)
        {
            float deltaTime = Time.deltaTime;
            return SmoothDampAngle(current,
                target,
                ref currentVelocity,
                smoothTime,
                maxSpeed,
                deltaTime);
        }

        /// <summary>Gradually changes an angle given in radians towards a desired goal angle over time.
        /// The value is smoothed by some spring-damper like function.
        /// The function can be used to smooth any kind of value, positions, colors, scalars. The most common use is for smoothing a follow camera.</summary>
        /// <param name="current">The current angle</param>
        /// <param name="target">The angle we are trying to reach</param>
        /// <param name="currentVelocity">The current angular velocity, this value is modified by the function every time you call it</param>
        /// <param name="smoothTime">Approximately the time it will take to reach the target. A smaller value will reach the target faster</param>
        /// <param name="maxSpeed">Optionally allows you to clamp the maximum speed</param>
        /// <param name="deltaTime">The time since the last call to this function. By default Time.deltaTime</param>
        public static float SmoothDampAngle(
            this float current,
            float target,
            ref float currentVelocity,
            float smoothTime,
            [UnityEngine.Internal.DefaultValue("Mathf.Infinity")]
            float maxSpeed,
            [UnityEngine.Internal.DefaultValue("Time.deltaTime")]
            float deltaTime)
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current,
                target,
                ref currentVelocity,
                smoothTime,
                maxSpeed,
                deltaTime);
        }

        #endregion

        #region Wrap

        public enum WrapMode
        {
            Oblivion = 0,
            Clamp = 1,
            Loop = 2,
            PingPong = 3
        }

        /// <summary>
        /// Wraps a value around some significant range.
        /// 
        /// Similar to C# modulo, but works like a true module, in a unary direction over any range (including negative values).
        /// 
        /// ex:
        /// Wrap(8,6,2) == 4
        /// Wrap(4,2,0) == 0
        /// Wrap(4,2,-2) == 0
        /// </summary>
        /// <param name="value">value to wrap</param>
        /// <param name="max">max in range</param>
        /// <param name="min">min in range</param>
        /// <returns>A value wrapped around min to max</returns>
        /// <remarks></remarks>
        public static int Wrap(this int value, int max, int min)
        {
            max -= min;
            if (max == 0)
                return min;

            var result = (value - min) % max;
            return result < 0 ? result + max + min : result + min;
        }

        public static int Wrap(this int value, int max)
        {
            var result = value % max;
            return result < 0 ? result + max : result;
        }

        public static long Wrap(this long value, long max, long min)
        {
            max -= min;
            if (max == 0)
                return min;

            var result = (value - min) % max;
            return result < 0 ? result + max + min : result + min;
        }

        public static long Wrap(long value, long max)
        {
            var result = value % max;
            return result < 0 ? result + max : result;
        }

        public static float Wrap(this float value, float max, float min)
        {
            max -= min;
            if (max == 0)
                return min;

            var result = (value - min) % max;
            return result < 0 ? result + max + min : result + min;
        }

        public static float Wrap(this float value, float max)
        {
            var result = value % max;
            return result < 0 ? result + max : result;
        }

        public static double Wrap(this double value, double max, double min)
        {
            max -= min;
            if (max == 0)
                return min;

            var result = (value - min) % max;
            return result < 0 ? result + max + min : result + min;
        }

        public static double Wrap(this double value, double max)
        {
            var result = value % max;
            return result < 0 ? result + max : result;
        }

        /// <summary>
        /// Wrap an index by some mode
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int WrapIndex(WrapMode mode, int value, int max)
        {
            switch (mode)
            {
                case WrapMode.Clamp:
                    return Clamp(value, 0, max - 1);
                case WrapMode.Loop:
                    return Wrap(value, max);
                case WrapMode.PingPong:
                    return (int)PingPong(value, max - 1);
                default:
                    return value;
            }
        }

        public static float Wrap(WrapMode mode, int value, int max, int min = 0)
        {
            switch (mode)
            {
                case WrapMode.Clamp:
                    return Clamp(value, min, max);
                case WrapMode.Loop:
                    return Wrap(value, max, min);
                case WrapMode.PingPong:
                    return (int)PingPong(value - min, max - min) + min;
                default:
                    return value;
            }
        }

        public static float Wrap(WrapMode mode, float value, float max, float min = 0)
        {
            switch (mode)
            {
                case WrapMode.Clamp:
                    return Clamp(value, min, max - 1);
                case WrapMode.Loop:
                    return Wrap(value, max, min);
                case WrapMode.PingPong:
                    return (int)PingPong(value - min, max - min - 1) + min;
                default:
                    return value;
            }
        }

        #endregion
    }
}