using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderAsset
    {
        // ----------------------------- UI DRAWING & USER ACTIONS ---------------------------------------

        internal class AssetFinderAssetDrawConfig
        {
            public bool highlight;
            public bool drawPath = true;
            public bool showFileSize = true;
            public bool showABName = false;
            public bool showAtlasName = false;
            public bool showUsageIcon = true;
            public IWindow window = null;
            public bool drawExtension = true;
            public Action onShowDetails = null;

            public AssetFinderAssetDrawConfig(
            bool highlight,
            bool drawPath = true,
            bool showFileSize = true,
            bool showABName = false,
            bool showAtlasName = false,
            bool showUsageIcon = true,
            IWindow window = null,
            bool drawExtension = true,
                Action onShowDetails = null)
            {
                this.highlight = highlight;
                this.drawPath = drawPath;
                this.showFileSize = showFileSize;
                this.showABName = showABName;
                this.showAtlasName = showAtlasName;
                this.showUsageIcon = showUsageIcon;
                this.window = window;
                this.drawExtension = drawExtension;
                this.onShowDetails = onShowDetails;
            }
        }

        internal float Draw(
            Rect r,
            AssetFinderAssetDrawConfig cfg
        )
        {
            Rect rowRect = new Rect(r.x, r.y, r.width, AssetFinderTheme.Current.TreeItemHeight);
            bool isHover = rowRect.Contains(Event.current.mousePosition);
            bool singleLine = r.height <= 18f;
            float rw = r.width;
            bool selected = AssetFinderBookmark.Contains(guid);

            r.height = AssetFinderTheme.Current.TreeItemHeight;
            bool hasMouse = (Event.current.type == EventType.MouseUp) && r.Contains(Event.current.mousePosition);

            if (hasMouse && (Event.current.button == 1))
            {
                var menu = new GenericMenu();
                if (m_extension == ".prefab") menu.AddItem(AssetFinderGUIContent.FromString("Edit in Scene"), false, EditPrefab);

                menu.AddItem(AssetFinderGUIContent.FromString("Open"), false, Open);
                menu.AddItem(AssetFinderGUIContent.FromString("Ping"), false, Ping);
                #if UNITY_2022_3_OR_NEWER
                menu.AddItem(AssetFinderGUIContent.FromString("Properties..."), false, OpenProperties);
                #endif
                menu.AddItem(AssetFinderGUIContent.FromString(guid), false, CopyGUID);

                //menu.AddItem(AssetFinderGUIContent.FromString("Select in Project Panel"), false, Select);

                menu.AddSeparator(string.Empty);
                menu.AddItem(AssetFinderGUIContent.FromString("Copy path"), false, CopyAssetPath);
                menu.AddItem(AssetFinderGUIContent.FromString("Copy full path"), false, CopyAssetPathFull);

                menu.ShowAsContext();
                Event.current.Use();
            }

            if (IsMissing)
            {
                if (!singleLine) r.y += 16f;

                if (Event.current.type != EventType.Repaint) return 0;

                GUI.Label(r, AssetFinderGUIContent.FromString(guid), EditorStyles.whiteBoldLabel);
                return 0;
            }

            Rect iconRect = GUI2.LeftRect(16f, ref r);
            GUI2.LeftRect(2f, ref r);
            if (Event.current.type == EventType.Repaint)
            {
                Texture icon = AssetDatabase.GetCachedIcon(m_assetPath);
                if (icon != null) GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }

            if ((Event.current.type == EventType.MouseDown) && (Event.current.button == 0))
            {
                Rect pingRect = iconRect; //AssetFinderSetting.PingRow ? new Rect(0, r.y, r.x + r.width, r.height) : 
                if (pingRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.control || Event.current.command)
                    {
                        if (selected)
                        {
                            RemoveFromSelection();
                        } else
                        {
                            AddToSelection();
                        }

                        if (cfg.window != null) cfg.window.Repaint();
                    } else if (Event.current.clickCount == 2)
                    {
                        Open();
                        Event.current.Use();
                    } else
                    {
                        Ping();
                    }
                }
            }
        
            if (isHover)
            {
                if (cfg.onShowDetails != null)
                {
                    r.xMax -= 10f;
                    var (detailRect, flex) = r.ExtractRight(22f);
                    if (GUI.Button(detailRect, new GUIContent("...", "Show Details"), EditorStyles.miniButton))
                    {
                        cfg.onShowDetails?.Invoke();
                    }
                    r = flex;
                }
                
#if UNITY_2022_3_OR_NEWER
                var (propRect, flex1) = r.ExtractRight(22f);
                if (GUI.Button(propRect, new GUIContent("P", "Open Properties"), EditorStyles.miniButton))
                {
                    OpenProperties();
                }
                r = flex1;
#endif
            }

            if (Event.current.type != EventType.Repaint) return 0;
            if ((UsedByMap != null) && (UsedByMap.Count > 0))
            {
                GUIContent str = AssetFinderGUIContent.FromInt(UsedByMap.Count);
                Rect countRect = iconRect;
                countRect.x -= 16f;
                countRect.xMin = -10f;
                GUI.Label(countRect, str, GUI2.miniLabelAlignRight);
            }

            float pathW = cfg.drawPath && !string.IsNullOrEmpty(assetFolder)
                ? EditorStyles.miniLabel.CalcSize(AssetFinderGUIContent.FromString(assetFolder)).x
                : 8f;

            float nameW = cfg.drawPath
                ? EditorStyles.boldLabel.CalcSize(AssetFinderGUIContent.FromString(assetName)).x
                : EditorStyles.label.CalcSize(AssetFinderGUIContent.FromString(assetName)).x;

            float extW = string.IsNullOrEmpty(extension) ? 0f : EditorStyles.miniLabel.CalcSize(AssetFinderGUIContent.FromString(extension)).x;
            Color cc = GUI.skin.settings.selectionColor;

            if (singleLine)
            {
                Rect lbRect = GUI2.LeftRect(pathW + nameW + extW, ref r);

                if (selected)
                {
                    Color c1 = GUI.color;
                    GUI.color = cc;
                    GUI.DrawTexture(lbRect, EditorGUIUtility.whiteTexture);
                    GUI.color = c1;
                }

                if (cfg.drawPath)
                {
                    if (!string.IsNullOrEmpty(assetFolder))
                    {
                        Color c2 = GUI.color;
                        GUI.color = new Color(c2.r, c2.g, c2.b, c2.a * 0.5f);
                        GUI.Label(GUI2.LeftRect(pathW, ref lbRect), AssetFinderGUIContent.FromString(assetFolder), EditorStyles.miniLabel);
                        GUI.color = c2;
                    }

                    GUI.Label(lbRect, AssetFinderGUIContent.FromString(assetName), EditorStyles.boldLabel);
                } else
                {
                    GUI.Label(lbRect, AssetFinderGUIContent.FromString(assetName), EditorStyles.label);
                }

                lbRect.xMin += nameW - 2f;
                lbRect.y += 1f;

                if (!string.IsNullOrEmpty(extension) && cfg.drawExtension)
                {
                    Color c3 = GUI.color;
                    GUI.color = new Color(c3.r, c3.g, c3.b, c3.a * 0.7f);
                    GUI.Label(lbRect, AssetFinderGUIContent.FromString(extension), EditorStyles.miniLabel);
                    GUI.color = c3;
                }
            } else
            {
                if (cfg.drawPath) GUI.Label(new Rect(r.x, r.y + 16f, r.width, r.height), AssetFinderGUIContent.FromString(m_assetFolder), EditorStyles.miniLabel);
                Rect lbRect = GUI2.LeftRect(nameW, ref r);
                if (selected) GUI2.Rect(lbRect, cc);
                GUI.Label(lbRect, AssetFinderGUIContent.FromString(assetName), EditorStyles.boldLabel);
            }

            Rect rr = GUI2.RightRect(10f, ref r);
            if (cfg.highlight)
            {
                rr.xMin += 2f;
                rr.width = 1f;
                GUI2.Rect(rr, GUI2.darkGreen);
            }

            Color c = GUI.color;
            GUI.color = new Color(c.r, c.g, c.b, c.a * 0.5f);

            // (Properties button drawn earlier to receive click events)

            if (cfg.showFileSize)
            {
                Rect fsRect = GUI2.RightRect(40f, ref r);
                if (fileSizeText == null) fileSizeText = AssetFinderGUIContent.FromString(AssetFinderHelper.GetfileSizeString(fileSize));
                GUI.Label(fsRect, fileSizeText, GUI2.miniLabelAlignRight);
            }

            if (!string.IsNullOrEmpty(m_addressable))
            {
                Rect adRect = GUI2.RightRect(100f, ref r);
                GUI.Label(adRect, AssetFinderGUIContent.FromString(m_addressable), GUI2.miniLabelAlignRight);
            }

            if (cfg.showUsageIcon && (HashUsedByClassesIds != null))
            {
                foreach (int item in HashUsedByClassesIds)
                {
                    if (!AssetFinderUnity.HashClassesNormal.ContainsKey(item)) continue;

                    string name = AssetFinderUnity.HashClassesNormal[item];
                    if (!HashClasses.TryGetValue(item, out Type t))
                    {
                        t = AssetFinderUnity.GetType(name);
                        HashClasses.Add(item, t);
                    }

                    bool isExisted = cacheImage.TryGetValue(name, out GUIContent content);
                    if (content == null)
                    {
                        content = t == null ? GUIContent.none : AssetFinderGUIContent.FromType(t, name);
                    }

                    if (!isExisted)
                    {
                        cacheImage.Add(name, content);
                    } else
                    {
                        cacheImage[name] = content;
                    }

                    if (content != null)
                    {
                        try
                        {
                            GUI.Label(GUI2.RightRect(15f, ref r), content, GUI2.miniLabelAlignRight);
                        }
						catch (Exception e)
						{
							AssetFinderLOG.LogWarning(e);
						}
                    }
                }
            }

            if (cfg.showAtlasName)
            {
                GUI2.RightRect(10f, ref r);
                Rect abRect = GUI2.RightRect(120f, ref r);
                if (!string.IsNullOrEmpty(m_atlas)) GUI.Label(abRect, AssetFinderGUIContent.FromString(m_atlas), GUI2.miniLabelAlignRight);
            }

            if (cfg.showABName)
            {
                GUI2.RightRect(10f, ref r);
                Rect abRect = GUI2.RightRect(100f, ref r);
                if (!string.IsNullOrEmpty(m_assetbundle)) GUI.Label(abRect, AssetFinderGUIContent.FromString(m_assetbundle), GUI2.miniLabelAlignRight);
            }

            if (true)
            {
                GUI2.RightRect(10f, ref r);
                Rect abRect = GUI2.RightRect(100f, ref r);
                if (!string.IsNullOrEmpty(m_addressable)) GUI.Label(abRect, AssetFinderGUIContent.FromString(m_addressable), GUI2.miniLabelAlignRight);
            }

            GUI.color = c;

            if (Event.current.type == EventType.Repaint) return rw < pathW + nameW ? 32f : 18f;

            return r.height;
        }

        internal GenericMenu AddArray(
            GenericMenu menu, System.Collections.Generic.List<string> list, string prefix, string title,
            string emptyTitle, bool showAsset, int max = 10)
        {
            menu.AddItem(AssetFinderGUIContent.FromString(emptyTitle), true, null);
            return menu;
        }

        internal void CopyGUID()
        {
            EditorGUIUtility.systemCopyBuffer = guid;
            Debug.Log(guid);
        }

        internal void CopyName()
        {
            EditorGUIUtility.systemCopyBuffer = m_assetName;
            Debug.Log(m_assetName);
        }

        internal void CopyAssetPath()
        {
            EditorGUIUtility.systemCopyBuffer = m_assetPath;
            Debug.Log(m_assetPath);
        }

        internal void CopyAssetPathFull()
        {
            string fullName = new FileInfo(m_assetPath).FullName;
            EditorGUIUtility.systemCopyBuffer = fullName;
            Debug.Log(fullName);
        }


        internal void RemoveFromSelection()
        {
            if (AssetFinderBookmark.Contains(guid)) AssetFinderBookmark.Remove(guid);
        }

        internal void AddToSelection()
        {
            if (!AssetFinderBookmark.Contains(guid)) AssetFinderBookmark.Add(guid);
        }

        internal void Ping()
        {
            if (EditorWindow.focusedWindow is AssetFinderWindowAll fr2Window)
            {
                fr2Window.smartLock.SetPingLockState(AssetFinderSmartLock.PingLockState.Asset);    
            }
            
            EditorApplication.delayCall += () =>
            {
                var asset = AssetDatabase.LoadAssetAtPath(m_assetPath, typeof(UnityObject));
                if (asset != null)
                {
                    EditorGUIUtility.PingObject(asset);
                }
            };

            // Only use event if it exists (not null when called from context menu)
            if (Event.current != null)
            {
                Event.current.Use();
            }
        }

        internal void Open()
        {
            AssetDatabase.OpenAsset(
                AssetDatabase.LoadAssetAtPath(m_assetPath, typeof(UnityObject))
            );
        }

        internal void OpenProperties()
        {
#if UNITY_2022_3_OR_NEWER
            var obj = AssetDatabase.LoadAssetAtPath(m_assetPath, typeof(UnityObject));
            if (obj != null)
            {
                EditorUtility.OpenPropertyEditor(obj);
            }
#endif
        }


        internal void EditPrefab()
        {
            UnityObject prefab = AssetDatabase.LoadAssetAtPath(m_assetPath, typeof(UnityObject));
            UnityObject.Instantiate(prefab);
        }
    }
} 