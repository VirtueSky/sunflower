using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public struct Uniform
    {
        #region field

        private static GUIStyle contentList;
        private static GUIStyle contentListBlue;
        private static GUIStyle contentListDark;
        private static GUIStyle contentBox;
        private static GUIStyle box;
        private static GUIStyle foldoutButton;
        private static GUIStyle foldoutIcon;
        private static GUIStyle installedIcon;
        private static GUIStyle headerLabel;

        private static readonly Dictionary<string, GUIContent> CachedIconContent =
            new Dictionary<string, GUIContent>();

        private static readonly UniformFoldoutState FoldoutSettings = new UniformFoldoutState();

        #endregion


        #region prop

        public static GUIStyle ContentList
        {
            get
            {
                if (contentList != null) return contentList;
                contentList = new GUIStyle
                {
                    border = new RectOffset(2, 2, 2, 2),
                    normal = { background = EditorResources.EvenBackground }
                };
                return contentList;
            }
        }

        public static GUIStyle ContentListBlue
        {
            get
            {
                if (contentListBlue != null) return contentListBlue;
                contentListBlue = new GUIStyle
                {
                    border = new RectOffset(2, 2, 2, 2),
                    normal = { background = EditorResources.EvenBackgroundBlue }
                };
                return contentListBlue;
            }
        }

        public static GUIStyle ContentListDark
        {
            get
            {
                if (contentListDark != null) return contentListDark;
                contentListDark = new GUIStyle
                {
                    border = new RectOffset(2, 2, 2, 2),
                    normal = { background = EditorResources.EvenBackgroundDark }
                };
                return contentListDark;
            }
        }


        public static GUIStyle BoxContent
        {
            get
            {
                if (contentBox != null) return contentBox;
                contentBox = new GUIStyle
                {
                    border = new RectOffset(2, 2, 2, 2),
                    normal =
                    {
                        background = EditorResources.BoxContentDark,
                        scaledBackgrounds = new[] { EditorResources.BoxContentDark }
                    }
                };
                return contentBox;
            }
        }

        public static GUIStyle Box
        {
            get
            {
                if (box != null) return box;
                box = new GUIStyle
                {
                    border = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(2, 2, 2, 2),
                    normal =
                    {
                        background = EditorResources.BoxBackgroundDark,
                        scaledBackgrounds = new[] { EditorResources.BoxBackgroundDark }
                    }
                };
                return box;
            }
        }

        public static GUIStyle FoldoutButton
        {
            get
            {
                if (foldoutButton != null) return foldoutButton;

                foldoutButton = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.white },
                    margin = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(0, 0, 2, 3),
                    stretchWidth = true,
                    richText = true,
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };

                return foldoutButton;
            }
        }

        public static GUIStyle FoldoutIcon
        {
            get
            {
                if (foldoutIcon != null) return foldoutIcon;

                foldoutIcon = new GUIStyle { padding = new RectOffset(0, 0, 5, 0) };

                return foldoutIcon;
            }
        }

        public static GUIStyle InstalledIcon
        {
            get
            {
                if (installedIcon != null) return installedIcon;

                installedIcon = new GUIStyle
                    { padding = new RectOffset(0, 0, 3, 0), fixedWidth = 30, fixedHeight = 30 };

                return installedIcon;
            }
        }

        public static GUIStyle HeaderLabel
        {
            get
            {
                if (headerLabel != null) return headerLabel;
                headerLabel = new GUIStyle(EditorStyles.label)
                    { fontSize = 13, fontStyle = FontStyle.Bold };
                return headerLabel;
            }
        }

        #endregion


        #region color

        public static readonly Color Green = new(0.31f, 0.98f, 0.48f, 0.66f);
        public static readonly Color Orange = new(1f, 0.72f, 0.42f, 0.66f);
        public static readonly Color Blue = new(0f, 1f, 0.97f, 0.27f);
        public static readonly Color Purple = new(0.74f, 0.58f, 0.98f, 0.39f);
        public static readonly Color Red = new(1f, 0.16f, 0.16f, 0.66f);
        public static readonly Color Pink = new(1f, 0.47f, 0.78f, 0.66f);
        public static readonly Color RichBlack = new(0.04f, 0.05f, 0.11f);
        public static readonly Color FieryRose = new(0.97f, 0.33f, 0.41f);
        public static readonly Color DeepCarminePink = new(1f, 0.2f, 0.2f);
        public static readonly Color FluorescentBlue = new(0.2f, 1f, 1f);

        #endregion


        #region draw

        public static void DrawBox(Rect position, GUIStyle style, bool isHover = false,
            bool isActive = false, bool on = false, bool hasKeyboardFocus = false)
        {
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position,
                    GUIContent.none,
                    isHover,
                    isActive,
                    on,
                    hasKeyboardFocus);
            }
        }

        /// <summary>
        /// Icon content
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        public static GUIContent IconContent(string name, string tooltip = "")
        {
            if (CachedIconContent.TryGetValue(name, out var result))
                return result ?? GUIContent.none;
            var builtinIcon = EditorGUIUtility.IconContent(name) ??
                              new GUIContent(Texture2D.whiteTexture);
            result = new GUIContent(builtinIcon.image, tooltip);
            CachedIconContent.Add(name, result);
            return result;
        }

        /// <summary>
        /// Draw header with title
        /// </summary>
        /// <param name="title"></param>
        public static void DrawHeader(string title)
        {
            var bgStyle = new GUIStyle(GUIStyle.none)
                { normal = { background = CreateTexture(RichBlack) } };
            GUILayout.BeginVertical(bgStyle, GUILayout.Height(60));
            GUILayout.BeginHorizontal();

            var iconStyle = new GUIStyle(GUIStyle.none) { padding = new RectOffset(2, 0, 2, 2) };
            // GUILayout.Box(EditorResources.Dreamblale, iconStyle, GUILayout.Width(60), GUILayout.Height(60));

            var titleStyle = new GUIStyle(GUIStyle.none)
            {
                margin = new RectOffset(4, 4, 4, 4),
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = false,
                richText = true,
                imagePosition = ImagePosition.ImageLeft,
                fixedHeight = 50,
                stretchHeight = true,
                stretchWidth = true,
                normal = { textColor = Color.white }
            };

            EditorGUILayout.LabelField(title, titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }


        // public static Texture2D Dreamblale => ProjectDatabase.FindAssetWithPath<Texture2D>("dreamblade.png", RELATIVE_PATH);

        public static Texture2D CreateTexture(Color color)
        {
            var result = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            result.SetPixel(0, 0, color);
            result.Apply();
            return result;
        }

        /// <summary>
        /// Draw only the property specified.
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="isReadOnly"></param>
        public static void DrawOnlyField(SerializedObject serializedObject, string fieldName,
            bool isReadOnly)
        {
            serializedObject.Update();
            var prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (prop.name != fieldName) continue;

                    GUI.enabled = !isReadOnly;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                    GUI.enabled = true;
                } while (prop.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws a line in the inspector.
        /// </summary>
        /// <param name="height"></param>
        public static void DrawLine(int height = 1)
        {
            var rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        /// <summary>
        /// Draw a selectable object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="labels"></param>
        public static void DrawSelectableObject(Object obj, string[] labels)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(labels[0], GUILayout.MaxWidth(300)))
                EditorGUIUtility.PingObject(obj);

            if (GUILayout.Button(labels[1], GUILayout.MaxWidth(75)))
            {
                EditorWindow.FocusWindowIfItsOpen(typeof(SceneView));
                Selection.activeObject = obj;
                SceneView.FrameLastActiveSceneView();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }

        /// <summary>
        /// Draws all properties like base.OnInspectorGUI() but excludes a field by name.
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="fieldToSkip">The name of the field that should be excluded. Example: "m_Script" will skip the default Script field.</param>
        public static void DrawInspectorExcept(SerializedObject serializedObject,
            string fieldToSkip)
        {
            Uniform.DrawInspectorExcept(serializedObject, new[] { fieldToSkip });
        }

        /// <summary>
        /// Draws all properties like base.OnInspectorGUI() but excludes the specified fields by name.
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="fieldsToSkip">
        /// An array of names that should be excluded.
        /// Example: new string[] { "m_Script" , "myInt" } will skip the default Script field and the Integer field myInt.
        /// </param>
        public static void DrawInspectorExcept(SerializedObject serializedObject,
            string[] fieldsToSkip)
        {
            serializedObject.Update();
            var prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (fieldsToSkip.Any(prop.name.Contains)) continue;

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                } while (prop.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw group selection with header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sectionName"></param>
        /// <param name="drawer"></param>
        /// <param name="defaultFoldout"></param>
        /// <param name="isShowContent"></param>
        public static float DrawGroupFoldout(string key, string sectionName, System.Action drawer,
            bool defaultFoldout = true, bool isShowContent = true)
        {
            bool foldout = GetFoldoutState(key, defaultFoldout);

            var rect = EditorGUILayout.BeginVertical(Box,
                GUILayout.MinHeight(foldout && isShowContent ? 30 : 0));

            if (!isShowContent)
            {
                EditorGUILayout.LabelField(sectionName);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                // Header label (and button).
                if (GUILayout.Button($"    {sectionName}", FoldoutButton))
                    SetFoldoutState(key, !foldout);

                // The expand/collapse icon.
                var buttonRect = GUILayoutUtility.GetLastRect();
                var iconRect = new Rect(buttonRect.x, buttonRect.y, 10, buttonRect.height);
                GUI.Label(iconRect,
                    foldout ? IconContent("d_IN_foldout_act_on") : IconContent("d_IN_foldout"),
                    FoldoutIcon);

                EditorGUILayout.EndHorizontal();

                // Draw the section content.
                if (foldout) GUILayout.Space(5);
                if (foldout && drawer != null) drawer();
            }

            float height = rect.height;
            EditorGUILayout.EndVertical();
            height += 4;
            return height;
        }

        /// <summary>
        /// Draw group selection with header
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sectionName"></param>
        /// <param name="drawer"></param>
        /// <param name="actionRightClick"></param>
        /// <param name="defaultFoldout"></param>
        /// <param name="isShowContent"></param>
        public static float DrawGroupFoldoutWithRightClick(
            string key,
            string sectionName,
            System.Action drawer,
            System.Action actionRightClick,
            bool defaultFoldout = true,
            bool isShowContent = true)
        {
            bool foldout = GetFoldoutState(key, defaultFoldout);

            var rect = EditorGUILayout.BeginVertical(Box,
                GUILayout.MinHeight(foldout && isShowContent ? 30 : 0));

            if (!isShowContent)
            {
                EditorGUILayout.LabelField(sectionName);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                // Header label (and button).
                if (GUILayout.Button($"    {sectionName}", FoldoutButton))
                {
                    if (Event.current.button == 1)
                    {
                        actionRightClick?.Invoke();
                        return rect.height;
                    }

                    SetFoldoutState(key, !foldout);
                }

                // The expand/collapse icon.
                var buttonRect = GUILayoutUtility.GetLastRect();
                var iconRect = new Rect(buttonRect.x, buttonRect.y, 10, buttonRect.height);
                GUI.Label(iconRect,
                    foldout ? IconContent("d_IN_foldout_act_on") : IconContent("d_IN_foldout"),
                    FoldoutIcon);

                EditorGUILayout.EndHorizontal();

                // Draw the section content.
                if (foldout) GUILayout.Space(5);
                if (foldout && drawer != null) drawer();
            }

            float height = rect.height;
            EditorGUILayout.EndVertical();
            height += 4;
            return height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldTitle"></param>
        /// <param name="text"></param>
        /// <param name="labelWidth"></param>
        /// <param name="textFieldWidthOption"></param>
        /// <returns></returns>
        public static string DrawTextField(string fieldTitle, string text,
            GUILayoutOption labelWidth, GUILayoutOption textFieldWidthOption = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            EditorGUILayout.LabelField(new GUIContent(fieldTitle), labelWidth);
            GUILayout.Space(4);
            text = textFieldWidthOption == null
                ? GUILayout.TextField(text)
                : GUILayout.TextField(text, textFieldWidthOption);
            GUILayout.Space(4);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            return text;
        }

        /// <summary>
        /// Centers a rect inside another window.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Rect CenterInWindow(Rect window, Rect origin)
        {
            var pos = window;
            float w = (origin.width - pos.width) * 0.5f;
            float h = (origin.height - pos.height) * 0.5f;
            pos.x = origin.x + w;
            pos.y = origin.y + h;
            return pos;
        }

        /// <summary>
        /// Draw label installed and icon
        /// </summary>
        public static void DrawInstalled(string version)
        {
            EditorGUILayout.BeginHorizontal();
            var label = $"Installed {version}";
            GUILayout.Label(label);
            var lastRect = GUILayoutUtility.GetLastRect();
            var iconRect = new Rect(lastRect.x + label.Length * 6f, lastRect.y, 10,
                lastRect.height);
            GUI.Label(iconRect, Uniform.IconContent("CollabNew"), InstalledIcon);
            EditorGUILayout.EndHorizontal();
        }

        #endregion


        #region foldout state

        public static bool GetFoldoutState(string key, bool defaultFoldout = true)
        {
            if (!FoldoutSettings.ContainsKey(key)) FoldoutSettings.Add(key, defaultFoldout);
            return FoldoutSettings[key];
        }

        public static void SetFoldoutState(string key, bool state)
        {
            if (!FoldoutSettings.ContainsKey(key))
            {
                FoldoutSettings.Add(key, state);
            }
            else
            {
                FoldoutSettings[key] = state;
            }
        }

        [System.Serializable]
        public class FoldoutState
        {
            public string key;
            public bool state;
        }

        [System.Serializable]
        public class UniformFoldoutState
        {
            public List<FoldoutState> uniformFoldouts;

            public UniformFoldoutState()
            {
                uniformFoldouts = new List<FoldoutState>();
            }

            public bool ContainsKey(string key)
            {
                return uniformFoldouts.Any(foldoutState => foldoutState.key.Equals(key));
            }

            public void Add(string key, bool value)
            {
                uniformFoldouts.Add(new FoldoutState { key = key, state = value });
            }

            public bool this[string key]
            {
                get
                {
                    foreach (var foldoutState in uniformFoldouts)
                    {
                        if (foldoutState.key.Equals(key))
                        {
                            return foldoutState.state;
                        }
                    }

                    return false;
                }
                set
                {
                    foreach (var foldoutState in uniformFoldouts)
                    {
                        if (foldoutState.key.Equals(key))
                        {
                            foldoutState.state = value;
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}