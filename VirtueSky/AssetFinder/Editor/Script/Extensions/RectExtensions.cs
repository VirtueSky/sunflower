using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class RectExtensions
    {
        internal static (Rect, Rect) HzSplit(this Rect r, float space = 8f, float ratio = 0.5f)
        {
            var w = (r.width - space) * ratio;
            var r1 = new Rect(r.x, r.y, w, r.height);
            var r2 = new Rect(r.x + w + space, r.y, r.width - w - space, r.height);
            return (r1, r2);
        }

        internal static (Rect left, Rect flex) ExtractLeft(this Rect r, float leftWidth = 16f, float space = 0f)
        {
            var left = new Rect(r.x, r.y, leftWidth, r.height);
            var flex = new Rect(r.x + leftWidth + space, r.y, r.width - leftWidth - space, r.height);
            return (left, flex);
        }

        internal static (Rect right, Rect flex) ExtractRight(this Rect r, float rightWidth = 16f, float space = 0f)
        {
            var right = new Rect(r.x + r.width - rightWidth, r.y, rightWidth, r.height);
            var flex = new Rect(r.x, r.y, r.width - rightWidth - space, r.height);
            return (right, flex);
        }

        internal static Rect LPad(this Rect r, float padding = 8f, bool padCondition = true)
        {
            return padCondition ? new Rect(r.x + padding, r.y, r.width - padding, r.height) : r;
        }

        internal static Rect RPad(this Rect r, float padding = 8f, bool padCondition = true)
        {
            return padCondition ? new Rect(r.x, r.y, r.width - padding, r.height) : r;
        }

        internal static Rect Pad(this Rect r, float padLeft = 8f, float padRight = 8f, bool padCondition = true)
        {
            return padCondition ? new Rect(r.x + padLeft, r.y, r.width - padLeft - padRight, r.height) : r;
        }

        internal static Rect Move(this Rect r, float dx = 0f, float dy = 0f)
        {
            return new Rect(r.x + dx, r.y + dy, r.width, r.height);
        }

        internal static Rect SetHeight(this Rect r, float h)
        {
            return new Rect(r.x, r.y, r.width, h);
        }

        internal static Rect SetWidth(this Rect r, float w)
        {
            return new Rect(r.x, r.y, w, r.height);
        }
    }
} 