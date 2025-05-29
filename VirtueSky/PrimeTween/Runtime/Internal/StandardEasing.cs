// Credits: http://robertpenner.com/easing/
// Copyright © 2001 Robert Penner
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// ReSharper disable PossibleNullReferenceException
// ReSharper disable CompareOfFloatsByEqualityOperator
using UnityEngine;

namespace PrimeTween {
    internal static class StandardEasing {
        const float halfPi = Mathf.PI / 2f;
        internal const float backEaseConst = 1.70158f;
        internal const float defaultElasticEasePeriod = 0.3f;

        static float InElastic(float t) => 1 - OutElastic(1 - t);

        static float OutElastic(float t) {
            const float decayFactor = 1f;
            float decay = Mathf.Pow(2, -10f * t * decayFactor);
            const float phase = defaultElasticEasePeriod / 4;
            const float twoPi = Mathf.PI * 2f;
            return t > 0.9999f ? 1 : decay * Mathf.Sin((t - phase) * twoPi / defaultElasticEasePeriod) + 1;
        }

        static float OutBounce(float x) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            if (x < 1 / d1) {
                return n1 * x * x;
            }
            if (x < 2 / d1) {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            if (x < 2.5 / d1) {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }

        internal static float Evaluate(float t, Ease ease) {
            switch (ease) {
                case Ease.Linear:
                    return t;
                case Ease.InSine:
                    return 1 - Mathf.Cos(t * halfPi);
                case Ease.OutSine:
                    return Mathf.Sin(t * halfPi);
                case Ease.InOutSine:
                    return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);
                case Ease.InQuad:
                    return t * t;
                case Ease.OutQuad:
                    return -t * (t - 2);
                case Ease.InOutQuad:
                    t *= 2f;
                    if (t < 1) {
                        return 0.5f * t * t;
                    }
                    return -0.5f * (--t * (t - 2) - 1);
                case Ease.InCubic:
                    return t * t * t;
                case Ease.OutCubic:
                    return (t -= 1) * t * t + 1;
                case Ease.InOutCubic:
                    t *= 2f;
                    if (t < 1) {
                        return 0.5f * t * t * t;
                    }
                    return 0.5f * ((t -= 2) * t * t + 2);
                case Ease.InQuart:
                    return t * t * t * t;
                case Ease.OutQuart:
                    return -((t -= 1) * t * t * t - 1);
                case Ease.InOutQuart:
                    t *= 2f;
                    if (t < 1) {
                        return 0.5f * t * t * t * t;
                    }
                    return -0.5f * ((t -= 2) * t * t * t - 2);
                case Ease.InQuint:
                    return t * t * t * t * t;
                case Ease.OutQuint:
                    return (t -= 1) * t * t * t * t + 1;
                case Ease.InOutQuint:
                    t *= 2f;
                    if (t < 1) {
                        return 0.5f * t * t * t * t * t;
                    }
                    return 0.5f * ((t -= 2) * t * t * t * t + 2);
                case Ease.InExpo:
                    return t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));
                case Ease.OutExpo:
                    if (t == 1) {
                        return 1;
                    }
                    return -Mathf.Pow(2, -10 * t) + 1;
                case Ease.InOutExpo:
                    if (t == 0) {
                        return 0;
                    }
                    if (t == 1) {
                        return 1;
                    }
                    t *= 2f;
                    if (t < 1) {
                        return 0.5f * Mathf.Pow(2, 10 * (t - 1));
                    }
                    return 0.5f * (-Mathf.Pow(2, -10 * --t) + 2);
                case Ease.InCirc:
                    return -(Mathf.Sqrt(1 - t * t) - 1);
                case Ease.OutCirc:
                    return Mathf.Sqrt(1 - (t -= 1) * t);
                case Ease.InOutCirc:
                    t *= 2f;
                    if (t < 1) {
                        return -0.5f * (Mathf.Sqrt(1 - t * t) - 1);
                    }
                    return 0.5f * (Mathf.Sqrt(1 - (t -= 2) * t) + 1);
                case Ease.InBack:
                    return t * t * ((backEaseConst + 1) * t - backEaseConst);
                case Ease.OutBack:
                    return (t -= 1) * t * ((backEaseConst + 1) * t + backEaseConst) + 1;
                case Ease.InOutBack:
                    t *= 2f;
                    const float c1 = backEaseConst * 1.525f;
                    if (t < 1) {
                        return 0.5f * (t * t * ((c1 + 1) * t - c1));
                    }
                    return 0.5f * ((t -= 2) * t * ((c1 + 1) * t + c1) + 2);
                case Ease.InElastic:
                    return InElastic(t);
                case Ease.OutElastic:
                    return OutElastic(t);
                case Ease.InOutElastic:
                    if (t < 0.5f) {
                        return InElastic(t * 2) * 0.5f;
                    }
                    return 0.5f + OutElastic((t - 0.5f) * 2f) * 0.5f;
                case Ease.InBounce:
                    return 1 - OutBounce(1 - t);
                case Ease.OutBounce:
                    return OutBounce(t);
                case Ease.InOutBounce:
                    return t < 0.5
                        ? (1 - OutBounce(1 - 2 * t)) / 2
                        : (1 + OutBounce(2 * t - 1)) / 2;
                case Ease.Custom:
                case Ease.Default:
                default:
                    throw new System.Exception($"Invalid ease type: {ease}.");
            }
        }
    }
}
