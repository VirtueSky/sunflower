using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderObjectDrawer
    {
        private static readonly Dictionary<UnityObject, GUIContent> contentMap = new Dictionary<UnityObject, GUIContent>();
        private static GUIStyle objectFieldStyle;

        public void DrawOnly(Rect rect, UnityObject target)
        {
            GUIContent content;

            if (target == null)
            {
                content = AssetFinderGUIContent.From("(none)", AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)));
            } else if (!contentMap.TryGetValue(target, out content))
            {
                content = AssetFinderGUIContent.From(target.name, AssetPreview.GetMiniTypeThumbnail(target.GetType()));
                contentMap.Add(target, content);
            }

            if (objectFieldStyle == null)
            {
                objectFieldStyle = new GUIStyle(EditorStyles.objectField)
                {
                    margin = new RectOffset(16, 0, 0, 0)
                };
            }

            EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));
            GUI.Label(rect, content, objectFieldStyle);
        }
    }
}
