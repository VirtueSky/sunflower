using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderEnumDrawer
    {

        [NonSerialized] internal EnumInfo AssetFinderenum;
        public int index;
        public string tooltip;

        public bool DrawLayout<T>(ref T enumValue, params GUILayoutOption[] options)
        {
            if (AssetFinderenum == null)
            {
                Type enumType = enumValue.GetType();
                AssetFinderenum = EnumInfo.Get(enumType);
                index = AssetFinderenum.IndexOf(enumValue);
            }

            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout)
            {
                GUILayout.Label(AssetFinderenum.contents[index], EditorStyles.toolbarPopup, options);
                return false;
            }

            int nIndex = EditorGUILayout.Popup(index, AssetFinderenum.contents, EditorStyles.toolbarPopup, options);
            if (nIndex == index)
            {
                // Debug.LogWarning($"Same index: {nIndex} | {index}");
                return false;
            }
            index = nIndex;
            enumValue = (T)AssetFinderenum.ValueAt(index);
            return true;
        }

        public bool Draw<T>(Rect rect, ref T enumValue)
        {
            if (AssetFinderenum == null)
            {
                Type enumType = enumValue.GetType();
                AssetFinderenum = EnumInfo.Get(enumType);
                index = AssetFinderenum.IndexOf(enumValue);
            }

            if (Event.current.type == EventType.Layout) return false;
            if (Event.current.type == EventType.Repaint)
            {
                GUIContent content = AssetFinderenum.contents[index];
                if (!string.IsNullOrEmpty(tooltip)) content.tooltip = tooltip;
                GUI.Label(rect, content, EditorStyles.toolbarPopup);
                return false;
            }

            int nIndex = EditorGUI.Popup(rect, index, AssetFinderenum.contents, EditorStyles.toolbarPopup); //, options
            if (nIndex != index)
            {
                index = nIndex;
                enumValue = (T)AssetFinderenum.ValueAt(index);
                return true;
            }

            return false;
        }
        internal class EnumInfo
        {
            public static readonly Dictionary<Type, EnumInfo> cache = new Dictionary<Type, EnumInfo>();
            public readonly GUIContent[] contents;
            public readonly Array values;
            public EnumInfo(Type enumType)
            {
                string[] names = Enum.GetNames(enumType);

                values = Enum.GetValues(enumType);
                contents = new GUIContent[names.Length];
                for (var i = 0; i < names.Length; i++)
                {
                    contents[i] = AssetFinderGUIContent.FromString(names[i]);
                }
            }

            public EnumInfo(params object[] enumValues)
            {
                values = enumValues;
                contents = new GUIContent[values.Length];
                for (var i = 0; i < values.Length; i++)
                {
                    contents[i] = AssetFinderGUIContent.FromString(enumValues[i].ToString());
                }
            }
            public static EnumInfo Get(Type type)
            {
                if (cache.TryGetValue(type, out EnumInfo result))
                {
                    return result;
                }

                result = new EnumInfo(type);
                cache.Add(type, result);
                return result;
            }

            public int IndexOf(object enumValue)
            {
                return Array.IndexOf(values, enumValue);
            }

            public object ValueAt(int index)
            {
                return values.GetValue(index);
            }
        }
    }
}
