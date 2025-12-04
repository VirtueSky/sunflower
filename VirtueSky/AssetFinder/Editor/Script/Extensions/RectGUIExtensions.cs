using System;
using UnityEngine;
using UnityEditor;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class RectGUIExtensions
    {
        internal static Rect OnRightClick(this Rect rect, Action onRightClick, float padding = 0f)
        {
            var padRect = rect.Pad(padding, padding);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                if (padRect.Contains(Event.current.mousePosition))
                {
                    onRightClick?.Invoke();
                    Event.current.Use();
                }
            }

            return rect;
        }

        internal static Rect OnLeftClick(this Rect rect, Action onClick, float padding = 0f)
        {
            var padRect = rect.Pad(padding, padding);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (padRect.Contains(Event.current.mousePosition))
                {
                    onClick?.Invoke();
                    Event.current.Use();
                }
            }

            return rect;
        }

        internal static Rect OnDoubleClick(this Rect rect, Action onDoubleClick, float padding = 0f)
        {
            var padRect = rect.Pad(padding, padding);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2)
            {
                if (padRect.Contains(Event.current.mousePosition))
                {
                    onDoubleClick?.Invoke();
                    Event.current.Use();
                }
            }

            return rect;
        }
    }
} 