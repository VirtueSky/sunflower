using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace VirtueSky.Misc
{
    public partial class Common
    {
        public static Color SetAlpha(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color SetRelativeAlpha(this Color color, float a)
        {
            color.a += a;
            return color;
        }

        public static Color SetRed(this Color color, float r)
        {
            color.r = r;
            return color;
        }

        public static Color SetRelativeRed(this Color color, float r)
        {
            color.r += r;
            return color;
        }

        public static Color SetGreen(this Color color, float g)
        {
            color.g = g;
            return color;
        }

        public static Color SetRelativeGreen(this Color color, float g)
        {
            color.g += g;
            return color;
        }

        public static Color SetBlue(this Color color, float b)
        {
            color.b = b;
            return color;
        }

        public static Color SetRelativeBlue(this Color color, float b)
        {
            color.b += b;
            return color;
        }

        public static Graphic SetAlpha(this Graphic graphic, float a)
        {
            var colorCache = graphic.color;
            colorCache.a = a;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetRelativeAlpha(this Graphic graphic, float a)
        {
            var colorCache = graphic.color;
            colorCache.a += a;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetRed(this Graphic graphic, float r)
        {
            var colorCache = graphic.color;
            colorCache.r = r;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetRelativeRed(this Graphic graphic, float r)
        {
            var colorCache = graphic.color;
            colorCache.r += r;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetGreen(this Graphic graphic, float g)
        {
            var colorCache = graphic.color;
            colorCache.g = g;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetRelativeGreen(this Graphic graphic, float g)
        {
            var colorCache = graphic.color;
            colorCache.g += g;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetBlue(this Graphic graphic, float b)
        {
            var colorCache = graphic.color;
            colorCache.b = b;
            graphic.color = colorCache;
            return graphic;
        }

        public static Graphic SetRelativeBlue(this Graphic graphic, float b)
        {
            var colorCache = graphic.color;
            colorCache.b += b;
            graphic.color = colorCache;
            return graphic;
        }

        public static SpriteRenderer SetAlpha(this SpriteRenderer renderer, float a)
        {
            var colorCache = renderer.color;
            colorCache.a = a;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetRelativeAlpha(this SpriteRenderer renderer, float a)
        {
            var colorCache = renderer.color;
            colorCache.a += a;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetRed(this SpriteRenderer renderer, float r)
        {
            var colorCache = renderer.color;
            colorCache.r = r;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetRelativeRed(this SpriteRenderer renderer, float r)
        {
            var colorCache = renderer.color;
            colorCache.r += r;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetGreen(this SpriteRenderer renderer, float g)
        {
            var colorCache = renderer.color;
            colorCache.g = g;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetRelativeGreen(this SpriteRenderer renderer, float g)
        {
            var colorCache = renderer.color;
            colorCache.g += g;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetBlue(this SpriteRenderer renderer, float b)
        {
            var colorCache = renderer.color;
            colorCache.b = b;
            renderer.color = colorCache;
            return renderer;
        }

        public static SpriteRenderer SetRelativeBlue(this SpriteRenderer renderer, float b)
        {
            var colorCache = renderer.color;
            colorCache.b += b;
            renderer.color = colorCache;
            return renderer;
        }


        /// <summary>
        ///   <para>Returns the color as a hexadecimal string in the format "#RRGGBB".</para>
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        /// <returns>
        ///   <para>Hexadecimal string representing the color.</para>
        /// </returns>
        public static string ToHtmlStringRGB(this Color color)
        {
            var color32 = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(color.r * byte.MaxValue), 0, byte.MaxValue),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * byte.MaxValue), 0, byte.MaxValue),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * byte.MaxValue), 0, byte.MaxValue),
                1);

            return "#{0:X2}{1:X2}{2:X2}".Format(color32.r, color32.g, color32.b);
        }


        /// <summary>
        ///   <para>Returns the color as a hexadecimal string in the format "#RRGGBBAA".</para>
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        /// <returns>
        ///   <para>Hexadecimal string representing the color.</para>
        /// </returns>
        // ReSharper disable once InconsistentNaming
        public static string ToHtmlStringRGBA(this Color color)
        {
            var color32 = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(color.r * byte.MaxValue), 0, byte.MaxValue),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * byte.MaxValue), 0, byte.MaxValue),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * byte.MaxValue), 0, byte.MaxValue),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.a * byte.MaxValue), 0, byte.MaxValue));

            return "#{0:X2}{1:X2}{2:X2}{3:X2}".Format(color32.r, color32.g, color32.b, color32.a);
        }


        public static bool TryParseHtmlString(this string htmlString, out Color color)
        {
            string stringColor = htmlString;
            if (!stringColor[0].Equals('#')) stringColor = stringColor.Insert(0, "#");
            return ColorUtility.TryParseHtmlString(stringColor, out color);
        }
    }
}