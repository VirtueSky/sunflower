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
    }
}