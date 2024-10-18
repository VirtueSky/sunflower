using System;
using System.Collections.Generic;
using VirtueSky.Localization;
using TMPro;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

namespace VirtueSky.LocalizationEditor
{
    public class LocaleTreeView : TreeView
    {
        private enum ColumnType
        {
            Type,
            Name,
            Language,
            Value
        }

        private int _id;
        private float _valueFieldWidth;
        private GUIStyle _textAreaStyle;
        private Rect _cellRect;

        public LocaleTreeView(TreeViewState state)
            : base(state)
        {
        }

        public LocaleTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader)
            : base(state, multiColumnHeader)
        {
            _textAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true };
            rowHeight = 20;
            columnIndexForTreeFoldouts = 1;
            showAlternatingRowBackgrounds = true;
            showBorder = true;

            customFoldoutYOffset = (20 - EditorGUIUtility.singleLineHeight) * 0.5f;
            multiColumnHeader.canSort = false;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            _id = 1;
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var localizedAssets = Locale.FindAllLocalizedAssets();
            var allItems = new List<TreeViewItem>();

            // Add localized assets.
            foreach (var localizedAsset in localizedAssets)
            {
                var assetItem = new AssetTreeViewItem(0, localizedAsset);
                allItems.Add(assetItem);

                // Add locale items.
                var localItems = localizedAsset.LocaleItems;
                for (var i = 0; i < localItems.Length; i++)
                {
                    allItems.Add(new LocaleTreeViewItem(_id++, 1, localItems[i], assetItem));
                }
            }

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                var columnType = (ColumnType)args.GetColumn(i);
                var cellRect = args.GetCellRect(i);
                CellGUI(cellRect, args.item, columnType, ref args);

                // Refresh row heights if the Value column width is changed.
                if (columnType == ColumnType.Value && cellRect.width != _valueFieldWidth)
                {
                    _valueFieldWidth = cellRect.width;
                    RefreshCustomRowHeights();
                }
            }
        }

        /// <summary>
        /// Make TextArea as expandable as possible.
        /// </summary>
        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            float h = base.GetCustomRowHeight(row, item);
            if (item is LocaleTreeViewItem localeItem)
            {
                var assetItem = localeItem.Parent;
                if (assetItem.Asset.GetGenericType == typeof(string))
                {
                    var column = multiColumnHeader.GetColumn(3);
                    if (column != null)
                    {
                        var stringValue = (string)localeItem.LocaleItem.ObjectValue;
                        float calculatedRowHeight = _textAreaStyle.CalcHeight(new GUIContent(stringValue), column.width) + 4;
                        h = Mathf.Clamp(calculatedRowHeight, h, 100);
                    }
                }
            }

            return h;
        }

        void CellGUI(Rect cellRect, TreeViewItem item, ColumnType column, ref RowGUIArgs args)
        {
            switch (column)
            {
                case ColumnType.Type:
                    DrawTypeCell(cellRect, item);
                    break;
                case ColumnType.Name:
                    DrawNameCell(cellRect, item, ref args);
                    break;
                case ColumnType.Language:
                    DrawLanguageCell(cellRect, item);
                    break;
                case ColumnType.Value:
                    DrawValueCell(cellRect, item);
                    break;
            }
        }

        private void DrawTypeCell(Rect cellRect, TreeViewItem item)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            var treeViewItem = item as AssetTreeViewItem;
            if (treeViewItem != null)
            {
                Texture icon;

                // Set icon by localized asset value type.
                var valueType = treeViewItem.Asset.GetGenericType;
                if (valueType == typeof(string))
                {
                    icon = EditorGUIUtility.ObjectContent(null, typeof(TextAsset)).image;
                }
                else if (valueType == typeof(TMP_FontAsset))
                {
                    icon = EditorGUIUtility.ObjectContent(null, typeof(Font)).image;
                }
                else
                {
                    icon = EditorGUIUtility.ObjectContent(null, valueType).image;
                }

                // Set default icon if not exist.
                if (!icon)
                {
                    icon = EditorGUIUtility.ObjectContent(null, typeof(ScriptableObject)).image;
                }

                if (icon)
                {
                    GUI.DrawTexture(cellRect, icon, ScaleMode.ScaleToFit);
                }
            }
        }

        private void DrawNameCell(Rect cellRect, TreeViewItem item, ref RowGUIArgs args)
        {
            ValidateMissingLocales(item);

            CenterRectUsingSingleLineHeight(ref cellRect);
            args.rowRect = cellRect;
            base.RowGUI(args);
            GUI.contentColor = Color.white;
        }

        private void ValidateMissingLocales(TreeViewItem item)
        {
            var assetTreeViewItem = item as AssetTreeViewItem;
            var assetItem = assetTreeViewItem?.Asset;
            var localizedText = assetItem as LocaleText;
            if (localizedText == null) return;
            foreach (var typedLocaleItem in localizedText.TypedLocaleItems)
            {
                GUI.contentColor = string.IsNullOrEmpty(typedLocaleItem.Value) ? new Color(0.97f, 0.33f, 0.41f) : Color.white;
            }
        }


        private void DrawLanguageCell(Rect cellRect, TreeViewItem item)
        {
            cellRect.y += 2;
            cellRect.height -= 4;
            if (item is LocaleTreeViewItem localeItem)
            {
                LocaleEditorUtil.LocaleDrawLanguageField(cellRect, ref localeItem);
            }
        }

        private void DrawValueCell(Rect cellRect, TreeViewItem item)
        {
            cellRect.y += 2;
            cellRect.height -= 4;
            if (item is LocaleTreeViewItem treeViewItem)
            {
                var localeItem = treeViewItem.LocaleItem;
                var valueType = treeViewItem.Parent.Asset.GetGenericType;

                EditorGUI.BeginChangeCheck();
                if (valueType.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    if (valueType == typeof(TMP_FontAsset) && localeItem.ObjectValue == null)
                    {
                        localeItem.ObjectValue = EditorGUI.ObjectField(cellRect, null, typeof(TMP_FontAsset), false);
                    }
                    else
                    {
                        localeItem.ObjectValue = EditorGUI.ObjectField(cellRect, (UnityEngine.Object)localeItem.ObjectValue, localeItem.ObjectValue.GetType(), false);
                    }
                }
                else if (valueType == typeof(string))
                {
                    EditorGUI.BeginChangeCheck();
                    localeItem.ObjectValue = EditorGUI.TextArea(cellRect, (string)localeItem.ObjectValue, _textAreaStyle);
                    if (EditorGUI.EndChangeCheck())
                    {
                        RefreshCustomRowHeights();
                    }
                }
                else
                {
                    EditorGUI.LabelField(cellRect, valueType + " value type not supported.");
                }

                if (EditorGUI.EndChangeCheck())
                {
                    treeViewItem.Parent.IsDirty = true;
                    EditorUtility.SetDirty(treeViewItem.Parent.Asset);
                }
            }
        }

        protected override bool CanRename(TreeViewItem item)
        {
            if (item is AssetTreeViewItem)
            {
                // Only allow rename if we can show the rename overlay with a certain width (label might be clipped
                // by other columns).
                var renameRect = GetRenameRect(treeViewRect, 0, item);
                return renameRect.width > 30;
            }

            return false;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            // Set the backend name and reload the tree to reflect the new model
            if (args.acceptedRename)
            {
                var item = FindItem(args.itemID, rootItem) as AssetTreeViewItem;
                if (item != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(item.Asset.GetInstanceID());
                    AssetDatabase.RenameAsset(assetPath, args.newName);
                    AssetDatabase.SaveAssets();
                    Reload();
                }
            }
        }

        public TreeViewItem GetSelectedItem()
        {
            var selection = GetSelection();
            if (selection.Count > 0)
            {
                return FindItem(selection[0], rootItem);
            }

            return null;
        }

        protected override Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
        {
            var cellRect = GetCellRectForTreeFoldouts(rowRect);
            CenterRectUsingSingleLineHeight(ref cellRect);
            return base.GetRenameRect(cellRect, row, item);
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
        {
            var typeColumnWidth = 30;
            var nameColumnWidth = 150;
            var languageColumnWidth = 110;
            float valueColumnWidth = treeViewWidth - typeColumnWidth - nameColumnWidth - languageColumnWidth;
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType"), "Localized asset type."),
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = typeColumnWidth,
                    minWidth = 30,
                    maxWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name", "Localized asset name."),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = nameColumnWidth,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Language", "Locale language."),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = languageColumnWidth,
                    minWidth = 35,
                    autoResize = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Value", "Locale value."),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = valueColumnWidth,
                    minWidth = 60,
                    autoResize = true
                }
            };

            Assert.AreEqual(columns.Length,
                Enum.GetValues(typeof(ColumnType)).Length,
                "Number of columns should match number of enum values: You probably forgot to update one of them.");

            var state = new MultiColumnHeaderState(columns);
            return state;
        }
    }
}