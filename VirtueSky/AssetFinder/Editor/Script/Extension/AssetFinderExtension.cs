using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static class GUIExtension
    {
        internal static bool drawDebug = false;
        internal static readonly float debugAlpha = 0.02f;
        internal static int debugCount;
        internal static readonly Color[] debugColors =
        {
            Color.cyan.Alpha(debugAlpha),
            Color.blue.Alpha(debugAlpha),
            Color.green.Alpha(debugAlpha),
            Color.yellow.Alpha(debugAlpha),
            Color.magenta.Alpha(debugAlpha),
            Color.red.Alpha(debugAlpha),
            Color.white.Alpha(debugAlpha)
        };

#if AssetFinderDEV
        [MenuItem("Window/FR2/Toggle Draw Debug")]
        internal static void ToggleDrawDebug()
        {
            drawDebug = !drawDebug;
        }
#endif



        internal static bool isRepaint => Event.current.type == EventType.Repaint;
        internal static bool isMouse => Event.current.isMouse;
        internal static bool isLayout => Event.current.type == EventType.Layout;

        public static Rect DrawOverlayDebug(this Rect rect, float alpha = 0.1f)
        {
            Color saved = GUI.color;
            GUI.color = debugColors[debugCount++ % debugColors.Length];
            {
                GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
            }
            GUI.color = saved;
            return rect;
        }

        public static Rect LFoldout(this Rect rect, ref bool isOpen, bool drawCondition)
        {
            var (iconRect, flexRect) = rect.ExtractLeft();
            if (!drawCondition) return flexRect;

            isOpen = EditorGUI.Foldout(iconRect, isOpen, GUIContent.none);
            if (drawDebug) DrawOverlayDebug(iconRect.Move(0, -2f));
            return flexRect;
        }

        private static Rect DrawIcon(Rect rect, Texture icon, float w, bool left, bool drawCondition)
        {
            if (!drawCondition) return rect;
            

            var (iconRect, flexRect) = left ? rect.ExtractLeft(w) : rect.ExtractRight(w);
            if ((icon != null) && isRepaint) GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            if (drawDebug) DrawOverlayDebug(iconRect);
            return flexRect;
        }


        private static Rect DrawLabel(Rect rect, GUIContent content, GUIStyle style, Color? color, bool left, bool drawCondition, float yOffset = 0)
        {
            if (content == null || content == GUIContent.none || !drawCondition) return rect;
            float w = style.CalcSize(content).x;
            
            var (labelRect, flexRect) = left ? rect.ExtractLeft(w) : rect.ExtractRight(w);
            if (drawDebug) DrawOverlayDebug(labelRect);
            if (!isRepaint) return flexRect;

            Color c = GUI.color;
            if (color != null) GUI.color = color.Value;
            {
                GUI.Label(labelRect.Move(0f, yOffset), content, style);
            }
            GUI.color = c;
            return flexRect;
        }

        public static Rect LColumn(this Rect rect, ref float columnW, Func<Rect, Rect> drawer)
        {
            var (lRect, flex) = rect.ExtractLeft(columnW);
            if (drawer == null) return flex;

            Rect padRect = drawer(lRect);
            

            if (padRect.width < 0f) columnW -= padRect.width;
            return flex;
        }

        public static Rect RColumn(this Rect rect, ref float columnW, Func<Rect, Rect> drawer)
        {
            var (lRect, flex) = rect.ExtractRight(columnW);
            if (drawer == null) return flex;

            Rect padRect = drawer(lRect);
            if (padRect.width < 0f) columnW -= padRect.width;
            return flex;
        }



        // TAB
        public static Rect LColumnAlign(this Rect rect, ref float columnX)
        {
            if (isLayout)
            {
                columnX = 0;
                return rect;
            }

            columnX = Mathf.Max(rect.x, columnX);
            rect.xMin = columnX;

            return rect;
        }

        public static Rect RColumnAlign(this Rect rect, ref float columnX, string name)
        {
            if (rect.xMax <= 0)
            {
                columnX = -1;
                return rect;
            }

            if (columnX < 0 || rect.xMax < columnX)
            {
                columnX = rect.xMax;
                
            } else
            {
                rect.xMax = columnX;
            }

            return rect;
        }

        public static Rect LDrawIcon(this Rect rect, Texture icon, float w = 16f, bool drawCondition = true)
        {
            return DrawIcon(rect, icon, w, true, drawCondition);
        }

        public static Rect RDrawIcon(this Rect rect, Texture icon, float w = 16f, bool drawCondition = true)
        {
            return DrawIcon(rect, icon, w, false, drawCondition);
        }

        // Label
        public static Rect LDrawLabel(this Rect rect, string label, Color? color = null, bool drawCondition = true)
        {
            return DrawLabel(rect, AssetFinderGUIContent.FromString(label), EditorStyles.label, color, true, drawCondition);
        }

        public static Rect RDrawLabel(this Rect rect, string label, Color? color = null, bool drawCondition = true)
        {
            return DrawLabel(rect, AssetFinderGUIContent.FromString(label), EditorStyles.label, color, false, drawCondition);
        }


        public static Rect LDrawMiniLabel(this Rect rect, GUIContent label, Color? color = null, bool drawCondition = true)
        {
            return DrawLabel(rect, label, EditorStyles.miniLabel, color, true, drawCondition, 1f);
        }

        public static Rect LDrawMiniLabel(this Rect rect, string label, Color? color = null, bool drawCondition = true)
        {
            return LDrawMiniLabel(rect, AssetFinderGUIContent.FromString(label), color, drawCondition);
        }

        public static Rect RDrawMiniLabel(this Rect rect, GUIContent label, Color? color = null, bool drawCondition = true)
        {
            return DrawLabel(rect, label, EditorStyles.miniLabel, color, false, drawCondition, 1f);
        }

        public static Rect RDrawMiniLabel(this Rect rect, string label, Color? color = null, bool drawCondition = true)
        {
            return RDrawMiniLabel(rect, AssetFinderGUIContent.FromString(label), color, drawCondition);
        }

        public static Rect LDrawLabel(this Rect rect, GUIContent content, GUIStyle style = null, Color? color = null, bool drawCondition = true)
        {
            if (content == null || content == GUIContent.none || !drawCondition) return rect;

            if (style == null) style = EditorStyles.label;
            float w = style.CalcSize(content).x;
            
            var (labelRect, flexRect) = rect.ExtractLeft(w);
            if (drawDebug) DrawOverlayDebug(labelRect);
            if (!isRepaint) return flexRect;

            Color c = GUI.color;
            if (color != null) GUI.color = color.Value;
            {
                GUI.Label(labelRect, content, style);
            }
            GUI.color = c;
            return flexRect;
        }

        public static Rect RDrawLabel(this Rect rect, GUIContent content, GUIStyle style = null, Color? color = null, bool drawCondition = true)
        {
            if (content == null || content == GUIContent.none || !drawCondition) return rect;

            if (style == null) style = EditorStyles.label;
            float w = style.CalcSize(content).x;
            
            var (labelRect, flexRect) = rect.ExtractRight(w);
            if (drawDebug) DrawOverlayDebug(labelRect);
            if (!isRepaint) return flexRect;

            Color c = GUI.color;
            if (color != null) GUI.color = color.Value;
            {
                GUI.Label(labelRect, content, style);
            }
            GUI.color = c;
            return flexRect;
        }


    }




}
