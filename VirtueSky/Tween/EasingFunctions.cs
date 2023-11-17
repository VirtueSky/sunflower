/*
TERMS OF USE - EASING EQUATIONS
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace VirtueSky.Tween
{
    using UnityEngine;
    using System;

    /// <summary>
    /// The Easing Types to choose From
    /// </summary>
    public enum EasingTypes
    {
        Linear,
        QuadraticIn,
        QuadraticOut,
        QuadraticInOut,
        QuarticIn,
        QuarticOut,
        QuarticInOut,
        QuinticIn,
        QuinticOut,
        QuinticInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        ExponentialIn,
        ExponentialOut,
        ExponentialInOut,
        CircularIn,
        CircularOut,
        CircularInOut,
        SinusoidalIn,
        SinusoidalOut,
        SinusoidalInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        BackIn,
        BackOut,
        BackInOut
    }

    /// <summary>
    /// Easing Functions
    /// </summary>
    public static class EasingFunctions
    {
        #region Easing Functions

        public static float Linear(float from, float to, float t)
        {
            return Mathf.Lerp(from, to, t);
        }

        public static float QuadraticIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * t * t;
        }

        public static float QuadraticOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * t * (2f - t);
        }

        public static float QuadraticInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2;
            if ((t *= 2f) < 1f)
                value2 = 0.5f * t * t;
            else
                value2 = -0.5f * (--t * (t - 2f) - 1f);
            return from + value * value2;
        }

        public static float QuarticIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * t * t * t * t;
        }

        public static float QuarticOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = 1f - (--t * t * t * t);
            return from + value * value2;
        }

        public static float QuarticInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            if ((t *= 2f) < 1f)
                return from + value * 0.5f * t * t * t * t;
            return from + value * -0.5f * ((t -= 2f) * t * t * t - 2f);
        }

        public static float QuinticIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * t * t * t * t * t;
        }

        public static float QuinticOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = --t * t * t * t * t + 1f;
            return from + value * value2;
        }

        public static float QuinticInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            if ((t *= 2f) < 1)
                return from + value * 0.5f * t * t * t * t * t;
            return from + value * 0.5f * ((t -= 2f) * t * t * t * t + 2f);
        }

        public static float CubicIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * t * t * t;
        }

        public static float CubicOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = --t * t * t + 1f;
            return from + value * value2;
        }

        public static float CubicInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to) * 0.5f;
            float value2;
            if ((t *= 2f) < 1f)
                value2 = t * t * t;
            else value2 = ((t -= 2f) * t * t + 2f);
            return from + value * value2;
        }

        public static float SinusoidalIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = 1f - Mathf.Cos(t * Mathf.PI / 2f);
            return from + value * value2;
        }

        public static float SinusoidalOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = Mathf.Sin(t * Mathf.PI / 2f);
            return from + value * value2;
        }

        public static float SinusoidalInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = 0.5f * (1f - Mathf.Cos(Mathf.PI * t));
            return from + value * value2;
        }

        public static float ExponentialIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = Mathf.Approximately(0f, t) ? 0f : Mathf.Pow(1024f, t - 1f);
            return from + value * value2;
        }

        public static float ExponentialOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = Mathf.Approximately(1f, t) ? 1f : 1f - Mathf.Pow(2f, -10f * t);
            return from + value * value2;
        }

        public static float ExponentialInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            if (Mathf.Approximately(0f, t))
                return from;
            if (Mathf.Approximately(1f, t))
                return from + value;
            if ((t *= 2f) < 1f)
                return from + value * 0.5f * Mathf.Pow(1024f, t - 1f);
            return from + value * 0.5f * (-Mathf.Pow(2f, -10f * (t - 1f)) + 2f);
        }

        public static float CircularIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = 1f - Mathf.Sqrt(1f - t * t);
            return from + value * value2;
        }

        public static float CircularOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            return from + value * Mathf.Sqrt(1f - (--t * t));
        }

        public static float CircularInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            if ((t *= 2f) < 1f)
                return from + value * -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
            return from + value * 0.5f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f);
        }

        public static float ElasticIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s, a = 0.1f, p = 0.4f;
            if (Mathf.Approximately(0, t))
                return from;
            if (Mathf.Approximately(1f, t))
                return from + value;
            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
                s = p * Mathf.Asin(1f / a) / (2f * Mathf.PI);

            return from + value * -(a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p));
        }

        public static float ElasticOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s, a = 0.1f, p = 0.4f;
            if (Mathf.Approximately(0, t))
                return from;
            if (Mathf.Approximately(1f, t))
                return from + value;
            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
                s = p * Mathf.Asin(1f / a) / (2f * Mathf.PI);

            return from + value * (a * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) + 1f);
        }

        public static float ElasticInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s, a = 0.1f, p = 0.4f;
            if (Mathf.Approximately(0, t))
                return from;
            if (Mathf.Approximately(1f, t))
                return from + value;
            if (a < 1f)
            {
                a = 1f;
                s = p / 4f;
            }
            else
                s = p * Mathf.Asin(1f / a) / (2f * Mathf.PI);

            float value2;
            if ((t *= 2f) < 1f)
                value2 = -0.5f * (a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p));
            else
                value2 = a * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t - s) * (2f * Mathf.PI) / p) * 0.5f + 1f;
            return from + value * value2;
        }

        public static float BounceIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2 = 1f - BounceOut(0f, 1f, 1f - t);
            return from + value * value2;
        }

        public static float BounceOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2;
            if (t < (1f / 2.75f))
            {
                value2 = 7.5625f * t * t;
            }
            else if (t < (2f / 2.75f))
            {
                value2 = 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
            }
            else if (t < (2.5f / 2.75f))
            {
                value2 = 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
            }
            else
            {
                value2 = 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
            }

            return from + value * value2;
        }

        public static float BounceInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float value2;
            if (t < 0.5f)
                value2 = BounceIn(0f, 1f, t * 2f) * 0.5f;
            else value2 = BounceOut(0f, 1f, t * 2f - 1f) * 0.5f + 0.5f;
            return from + value * value2;
        }

        public static float BackIn(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s = 1.70158f;
            return from + value * t * t * ((s + 1f) * t - s);
        }

        public static float BackOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s = 1.70158f;
            float value2 = --t * t * ((s + 1f) * t + s) + 1f;
            return from + value * value2;
        }

        public static float BackInOut(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            float value = Difference(from, to);
            float s = 1.70158f * 1.525f;
            if ((t *= 2f) < 1f)
                return from + value * 0.5f * (t * t * ((s + 1) * t - s));
            return from + value * 0.5f * ((t -= 2f) * t * ((s + 1f) * t + s) + 2f);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Function returns an easing Function according to the given enum EasingTypes
        /// </summary>
        /// <param name="type">Type.</param>
        public static Func<float, float, float, float> Function(EasingTypes type)
        {
            switch (type)
            {
                case EasingTypes.Linear:
                    return Linear;

                case EasingTypes.QuadraticIn:
                    return QuadraticIn;

                case EasingTypes.QuadraticOut:
                    return QuadraticOut;

                case EasingTypes.QuadraticInOut:
                    return QuadraticInOut;

                case EasingTypes.QuarticIn:
                    return QuarticIn;

                case EasingTypes.QuarticOut:
                    return QuarticOut;

                case EasingTypes.QuarticInOut:
                    return QuarticInOut;

                case EasingTypes.QuinticIn:
                    return QuinticIn;

                case EasingTypes.QuinticOut:
                    return QuinticOut;

                case EasingTypes.QuinticInOut:
                    return QuinticInOut;

                case EasingTypes.CubicIn:
                    return CubicIn;

                case EasingTypes.CubicOut:
                    return CubicOut;

                case EasingTypes.CubicInOut:
                    return CubicInOut;

                case EasingTypes.ExponentialIn:
                    return ExponentialIn;

                case EasingTypes.ExponentialOut:
                    return ExponentialOut;

                case EasingTypes.ExponentialInOut:
                    return ExponentialInOut;

                case EasingTypes.CircularIn:
                    return CircularIn;

                case EasingTypes.CircularOut:
                    return CircularOut;

                case EasingTypes.CircularInOut:
                    return CircularInOut;

                case EasingTypes.SinusoidalIn:
                    return SinusoidalIn;

                case EasingTypes.SinusoidalOut:
                    return SinusoidalOut;

                case EasingTypes.SinusoidalInOut:
                    return SinusoidalInOut;

                case EasingTypes.ElasticIn:
                    return ElasticIn;

                case EasingTypes.ElasticOut:
                    return ElasticOut;
                case EasingTypes.ElasticInOut:
                    return ElasticInOut;

                case EasingTypes.BounceIn:
                    return BounceIn;

                case EasingTypes.BounceOut:
                    return BounceOut;

                case EasingTypes.BounceInOut:
                    return BounceInOut;

                case EasingTypes.BackIn:
                    return BackIn;

                case EasingTypes.BackOut:
                    return BackOut;

                case EasingTypes.BackInOut:
                    return BackInOut;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static float Difference(float a, float b)
        {
            if (a > b)
                return -Mathf.Abs(a - b);
            return Mathf.Abs(a - b);
        }

        #endregion
    }
}