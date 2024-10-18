using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using VirtueSky.Localization;
using VirtueSky.LocalizationEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPLocalizationDrawer
    {
        private static GoogleTranslator Translator => new(LocaleSettings.GoogleTranslateApiKey);
        private static LocaleTabType localeTabType = LocaleTabType.Setting;
        private static VirtueSky.Localization.LocaleSettings _settings;
        private static UnityEditor.Editor _editor;


        private static LocaleTabType _currentLocaleTab = LocaleTabType.Setting;
        private static TreeViewState _treeViewState;
        public static LocaleTreeView _localeTreeView;
        private static SearchField _localeSearchField;

        private static Rect ToolbarRect(Rect position) =>
            new(ConstantControlPanel.POSITION_X_START_CONTENT + 5, GUILayoutUtility.GetLastRect().y + 10, position.width - ConstantControlPanel.POSITION_X_START_CONTENT - 10,
                GUILayoutUtility.GetLastRect().y + 30);


        private static Rect BodyViewRect(Rect position) =>
            new(ConstantControlPanel.POSITION_X_START_CONTENT + 5, GUILayoutUtility.GetLastRect().y + 30, position.width - ConstantControlPanel.POSITION_X_START_CONTENT - 10, 400);


        private static Rect BottomToolbarRect(Rect position) =>
            new(ConstantControlPanel.POSITION_X_START_CONTENT + 5, position.height - 25, position.width - ConstantControlPanel.POSITION_X_START_CONTENT - 10, 20);

        const float TAB_WIDTH = 50f;

        private static bool _localeInitialized;
        private static MultiColumnHeaderState _multiColumnHeaderState;

        private struct EditorCommands
        {
            public const string DUPLICATE = "Duplicate";
            public const string DELETE = "Delete";
            public const string FRAME_SELECTED = "FrameSelected";
        }

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            _settings = CreateAsset.GetScriptableAsset<LocaleSettings>();
            _editor = UnityEditor.Editor.CreateEditor(_settings);
        }

        public static void OnDrawLocalization(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Localization, "Localization");
            GUILayout.Space(10);
            DrawTab();
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            switch (localeTabType)
            {
                case LocaleTabType.Setting:
                    DrawSetting();
                    break;
                case LocaleTabType.Explore:
                    DrawExplore(position);
                    break;
            }

            GUILayout.EndVertical();
        }

        static void DrawTab()
        {
            EditorGUILayout.BeginHorizontal();
            bool clickSetting = GUILayout.Toggle(localeTabType == LocaleTabType.Setting, "Setting",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickSetting && localeTabType != LocaleTabType.Setting)
            {
                localeTabType = LocaleTabType.Setting;
            }

            bool clickPickup = GUILayout.Toggle(localeTabType == LocaleTabType.Explore, "Explore",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickPickup && localeTabType != LocaleTabType.Explore)
            {
                localeTabType = LocaleTabType.Explore;
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSetting()
        {
            if (_settings == null)
            {
                if (GUILayout.Button("Create LocaleSettings"))
                {
                    _settings = CreateAsset.CreateAndGetScriptableAsset<LocaleSettings>("/Localization/Resources",
                        isPingAsset: false);
                    Init();
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings editor.",
                        MessageType.Error);
                    return;
                }

                _editor.OnInspectorGUI();
            }
        }

        private static void DrawExplore(Rect position)
        {
            InitializeIfNeeded(ref _treeViewState, ref _localeTreeView, ref _multiColumnHeaderState,
                BodyViewRect(position), ref _localeSearchField, ref _localeInitialized);
            HandleEditorCommands(ref _localeTreeView);
            SearchBarView(ref _localeTreeView, ref _localeSearchField, ToolbarRect(position));
            BodyView(ref _localeTreeView, BodyViewRect(position));
            BottomToolbarView(ref _localeTreeView, BottomToolbarRect(position));
        }

        private static void InitializeIfNeeded(
            ref TreeViewState treeViewState,
            ref LocaleTreeView treeView,
            ref MultiColumnHeaderState multiColumnHeaderState,
            Rect bodyViewRect,
            ref SearchField searchField,
            ref bool initialized)
        {
            if (treeViewState == null || treeView == null || searchField == null) initialized = false;

            if (!initialized)
            {
                if (treeViewState == null) treeViewState = new TreeViewState();
                bool firstInit = multiColumnHeaderState == null;
                var headerState = LocaleTreeView.CreateDefaultMultiColumnHeaderState(bodyViewRect.width);
                if (MultiColumnHeaderState.CanOverwriteSerializedFields(multiColumnHeaderState, headerState))
                {
                    MultiColumnHeaderState.OverwriteSerializedFields(multiColumnHeaderState, headerState);
                }

                multiColumnHeaderState = headerState;

                var multiColumnHeader = new MultiColumnHeader(headerState);
                if (firstInit) multiColumnHeader.ResizeToFit();

                treeView = new LocaleTreeView(treeViewState, multiColumnHeader);
                searchField = new SearchField();
                searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;

                initialized = true;
            }
        }

        private static void HandleEditorCommands(ref LocaleTreeView treeView)
        {
            var selectedItems = GetSelectedAssetItems(ref treeView).ToList();
            if (selectedItems.Any())
            {
                var e = Event.current;
                if (e.type == EventType.ValidateCommand &&
                    (e.commandName == EditorCommands.DELETE || e.commandName == EditorCommands.DUPLICATE ||
                     e.commandName == EditorCommands.FRAME_SELECTED))
                {
                    e.Use();
                }

                if (e.type == EventType.ExecuteCommand)
                {
                    switch (e.commandName)
                    {
                        case EditorCommands.DELETE:
                            DeleteAssetItems(selectedItems, ref treeView);
                            break;
                        case EditorCommands.DUPLICATE:
                            DuplicateAssetItems(selectedItems, ref treeView);
                            break;
                        case EditorCommands.FRAME_SELECTED:
                            RevealLocalizedAsset(selectedItems.FirstOrDefault());
                            break;
                    }
                }
            }
        }

        private static void DeleteAssetItems(IEnumerable<AssetTreeViewItem> items, ref LocaleTreeView treeView)
        {
            foreach (var item in items.ToList())
            {
                string assetPath = AssetDatabase.GetAssetPath(item.Asset.GetInstanceID());
                AssetDatabase.MoveAssetToTrash(assetPath);
            }

            // refresh view
            treeView.Reload();
        }

        private static void DuplicateAssetItems(IEnumerable<AssetTreeViewItem> items, ref LocaleTreeView treeView)
        {
            foreach (var item in items)
            {
                string assetPath = AssetDatabase.GetAssetPath(item.Asset.GetInstanceID());
                string newPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
                AssetDatabase.CopyAsset(assetPath, newPath);
            }

            // refresh view
            treeView.Reload();
        }

        private static void RevealLocalizedAsset(AssetTreeViewItem assetTreeViewItem)
        {
            Debug.Assert(assetTreeViewItem != null);
            EditorGUIUtility.PingObject(assetTreeViewItem.Asset);
        }

        private static IEnumerable<AssetTreeViewItem> GetSelectedAssetItems(ref LocaleTreeView treeView)
        {
            return GetSelectedItemsAs<AssetTreeViewItem>(ref treeView);
        }

        private static IEnumerable<T> GetSelectedItemsAs<T>(ref LocaleTreeView treeView) where T : TreeViewItem
        {
            if (treeView == null) return Enumerable.Empty<T>();
            var selection = treeView.GetSelection();
            var items = treeView.GetRows().Where(item => item as T != null && selection.Contains(item.id));
            return items.Cast<T>();
        }

        private static void SearchBarView(ref LocaleTreeView treeView, ref SearchField searchField, Rect rect)
        {
            treeView.searchString = searchField.OnGUI(rect, treeView.searchString);
        }

        private static void BodyView(ref LocaleTreeView treeView, Rect rect)
        {
            treeView.OnGUI(rect);
            OnContextMenu(ref treeView, rect);
        }

        private static void OnContextMenu(ref LocaleTreeView treeView, Rect rect)
        {
            var currentEvent = Event.current;
            var mousePosition = currentEvent.mousePosition;
            if (rect.Contains(mousePosition) && currentEvent.type == EventType.ContextClick)
            {
                TryGetSelectedTreeViewItem(ref treeView, out var assetTreeViewItem, out var localeTreeViewItem);

                if (assetTreeViewItem != null && localeTreeViewItem != null)
                {
                    OnLocaleItemContextMenu(assetTreeViewItem, localeTreeViewItem);
                    currentEvent.Use();
                }
                else
                {
                    OnAssetItemContextMenu(ref assetTreeViewItem, ref mousePosition);
                    currentEvent.Use();
                }
            }
        }

        private static void TryGetSelectedTreeViewItem(ref LocaleTreeView treeView,
            out AssetTreeViewItem assetTreeViewItem, out LocaleTreeViewItem localeTreeViewItem)
        {
            var selectedItem = treeView.GetSelectedItem();
            assetTreeViewItem = selectedItem as AssetTreeViewItem;
            localeTreeViewItem = selectedItem as LocaleTreeViewItem;

            if (assetTreeViewItem == null && selectedItem != null)
            {
                assetTreeViewItem = ((LocaleTreeViewItem)selectedItem).Parent;
            }
        }

        private static void OnAssetItemContextMenu(ref AssetTreeViewItem assetTreeViewItem, ref Vector2 mousePosition)
        {
            const string itemCreate = "Create";
            const string itemRename = "Rename";
            const string itemDelete = "Delete";

            if (Event.current != null)
            {
                mousePosition = Event.current.mousePosition;
            }

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent(itemCreate), false, AssetItemContextMenu_Create, mousePosition);

            if (assetTreeViewItem == null)
            {
                menu.AddDisabledItem(new GUIContent(itemRename));
                menu.AddDisabledItem(new GUIContent(itemDelete));
            }
            else
            {
                menu.AddItem(new GUIContent(itemRename), false, AssetItemContextMenu_Rename);
                menu.AddItem(new GUIContent(itemDelete), false, AssetItemContextMenu_Delete);
            }

            menu.ShowAsContext();
        }

        private static void AssetItemContextMenu_Create(object mousePosition)
        {
            CreateLocalizedAssetPopup((Vector2)mousePosition);
        }

        private static void CreateLocalizedAssetPopup(Vector2 mousePosition)
        {
            var popupPosition = new Rect(mousePosition, Vector2.zero);
            EditorUtility.DisplayPopupMenu(popupPosition, "Assets/Create/Pancake/Localization/", null);
        }

        private static void AssetItemContextMenu_Rename()
        {
            TryGetSelectedTreeViewItem(ref _localeTreeView, out var assetTreeViewItem, out _);
            RenameLocalizedAsset(ref _localeTreeView, assetTreeViewItem);
        }

        private static void RenameLocalizedAsset(ref LocaleTreeView treeView, AssetTreeViewItem assetTreeViewItem)
        {
            Debug.Assert(assetTreeViewItem != null);
            treeView.BeginRename(assetTreeViewItem);
        }

        private static void AssetItemContextMenu_Delete()
        {
            DeleteAssetItems(GetSelectedAssetItems(ref _localeTreeView), ref _localeTreeView);
        }

        private static void OnLocaleItemContextMenu(AssetTreeViewItem assetTreeViewItem,
            LocaleTreeViewItem localeTreeViewItem)
        {
            Debug.Assert(assetTreeViewItem != null);
            Debug.Assert(localeTreeViewItem != null);
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Make default"), false, LocaleItemContextMenu_MakeDefault);
            menu.AddItem(new GUIContent("Remove"), false, LocaleItemContextMenu_Remove);
            menu.ShowAsContext();
        }

        private static void LocaleItemContextMenu_MakeDefault()
        {
            TryGetSelectedTreeViewItem(ref _localeTreeView, out var assetTreeViewItem, out var localeTreeViewItem);
            MakeLocaleDefault(ref _localeTreeView, assetTreeViewItem, localeTreeViewItem);
        }

        private static void LocaleItemContextMenu_Remove()
        {
            TryGetSelectedTreeViewItem(ref _localeTreeView, out var assetTreeViewItem, out var localeTreeViewItem);
            RemoveLocale(ref _localeTreeView, assetTreeViewItem.Asset, localeTreeViewItem.LocaleItem);
        }

        private static void MakeLocaleDefault(ref LocaleTreeView treeView, AssetTreeViewItem assetTreeViewItem,
            LocaleTreeViewItem localeTreeViewItem)
        {
            var localizedAsset = assetTreeViewItem.Asset;
            var language = localeTreeViewItem.LocaleItem.Language;

            var serializedObject = new SerializedObject(localizedAsset);
            serializedObject.Update();
            var elements = serializedObject.FindProperty("items");
            if (elements != null && elements.arraySize > 1)
            {
                int index = Array.FindIndex(localizedAsset.LocaleItems, x => x.Language == language);
                if (index >= 0)
                {
                    language = new Language(language.Name, language.Code, language.Custom);
                    elements.MoveArrayElement(index, 0);
                    serializedObject.ApplyModifiedProperties();
                    treeView.Reload();
                    Debug.Log(localizedAsset.name + ":" + language + " was set as the default language.");
                }
            }
        }

        private static void RemoveLocale(ref LocaleTreeView treeView, ScriptableLocaleBase scriptable,
            LocaleItemBase localeItem)
        {
            if (ScriptableLocaleEditor.RemoveLocale(scriptable, localeItem))
            {
                treeView.Reload();
            }
        }

        private static void BottomToolbarView(ref LocaleTreeView treeView, Rect rect)
        {
            var backgroundStyle = new GUIStyle(GUI.skin.box)
                { normal = { background = CreateTexture(new Color(0.55f, 0.55f, 0.55f, 1f)) } };

            var toolbarStyle = new GUIStyle(EditorStyles.toolbar);
            var padding = toolbarStyle.padding;
            padding.left = 0;
            padding.right = 0;
            toolbarStyle.padding = padding;

            // Toolbar background.
            GUI.Box(new Rect(rect.x, rect.y, rect.width, rect.height - 1), GUIContent.none, backgroundStyle);

            // Toolbar itself.
            GUILayout.BeginArea(new Rect(rect.x, rect.y + 1, rect.width - 1, rect.height));
            using (new EditorGUILayout.HorizontalScope(toolbarStyle))
            {
                TreeViewControls(ref treeView);
                LocalizedAssetControls(ref treeView);
                GUILayout.FlexibleSpace();
                LocaleItemControls(ref treeView);
            }

            GUILayout.EndArea();
        }

        public static Texture2D CreateTexture(Color color)
        {
            var result = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            result.SetPixel(0, 0, color);
            result.Apply();
            return result;
        }

        private static void TreeViewControls(ref LocaleTreeView treeView)
        {
            if (GUILayout.Button(Uniform.IconContent("d_SettingsIcon@2x", "Open settings"), EditorStyles.toolbarButton,
                    GUILayout.Width(25)))
            {
                var settings = LocaleSettings.Instance;
                if (settings) Selection.activeObject = settings;
            }

            if (GUILayout.Button(Uniform.IconContent("refresh", "Refresh the window"), EditorStyles.toolbarButton))
            {
                treeView?.Reload();
            }
        }

        private static void LocalizedAssetControls(ref LocaleTreeView treeView)
        {
            if (GUILayout.Button(new GUIContent("Create", "Create a new localized asset."),
                    EditorStyles.toolbarDropDown))
            {
                var mousePosition = Event.current.mousePosition;
                CreateLocalizedAssetPopup(mousePosition);
            }

            var selectedItem = treeView.GetSelectedItem() as AssetTreeViewItem;
            GUI.enabled = selectedItem != null;
            if (GUILayout.Button(new GUIContent("Rename", "Rename the selected localized asset."),
                    EditorStyles.toolbarButton))
            {
                RenameLocalizedAsset(ref treeView, selectedItem);
            }

            if (GUILayout.Button(new GUIContent("Delete", "Delete the selected localized asset."),
                    EditorStyles.toolbarButton))
            {
                DeleteAssetItems(new[] { selectedItem }, ref treeView);
            }

            GUI.enabled = true;

            if (GUILayout.Button(new GUIContent("Import", "Import text from csv file"), EditorStyles.toolbarButton))
            {
                LocaleEditorUtil.Import();
            }

            if (GUILayout.Button(new GUIContent("Export", "Export text to csv file"), EditorStyles.toolbarButton))
            {
                LocaleEditorUtil.Export();
            }
        }

        private static void LocaleItemControls(ref LocaleTreeView treeView)
        {
            TryGetSelectedTreeViewItem(ref treeView, out var assetTreeViewItem, out var localeTreeViewItem);

            GUI.enabled = assetTreeViewItem != null && assetTreeViewItem.Asset.GetGenericType == typeof(string);
            if (GUILayout.Button(new GUIContent("Translate By", "Translate missing locales."),
                    EditorStyles.toolbarButton))
            {
                TranslateMissingLocalesWithMenu(assetTreeViewItem?.Asset);
            }

            GUI.enabled = !Application.isPlaying;
            if (GUILayout.Button(new GUIContent("Translate All", "Translate all missing locales."),
                    EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Translate All",
                        "Are you sure you wish to translate all missing locale?\nThis action cannot be reversed.",
                        "Yes", "No"))
                {
                    Debug.Log("[Localization] Starting translate all LocaleText!");
                    EditorCoroutine.Start(ExecuteTranslateProcess(treeView));
                }
            }

            if (GUILayout.Button(
                    new GUIContent("Fill All LocaleText",
                        "Fill language same with AvaiableLanguage for all LocaleText."), EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Fill language same AvaiableLanguage for all LocaleText",
                        "Are you sure you wish to fill language same AvaiableLanguage for all LocaleText?\nThis action cannot be reversed.",
                        "Yes",
                        "No"))
                {
                    EditorCoroutine.Start(ExecuteFillMissingLangProcess(treeView));
                }
            }

            // First element is already default.
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(
                    new GUIContent("App Language",
                        Application.isPlaying
                            ? "Set application language"
                            : "Application language can be set in play mode"),
                    EditorStyles.toolbarButton))
            {
                var currentLanguage = Locale.CurrentLanguage;
                var languages = LocaleSettings.AvailableLanguages;

                var menu = new GenericMenu();
                foreach (var language in languages)
                {
                    menu.AddItem(new GUIContent(language.Name), language == currentLanguage, AppLanguageContextMenu,
                        language);
                }

                menu.ShowAsContext();
            }

            GUI.enabled = assetTreeViewItem != null;
            if (GUILayout.Button(Uniform.IconContent("Toolbar Plus", "Add locale for selected asset."),
                    EditorStyles.toolbarButton))
            {
                AddLocale(ref treeView, assetTreeViewItem?.Asset);
            }

            GUI.enabled = localeTreeViewItem != null;

            if (GUILayout.Button(Uniform.IconContent("Toolbar Minus", "Remove selected locale."),
                    EditorStyles.toolbarButton))
            {
                RemoveLocale(ref treeView, assetTreeViewItem?.Asset, localeTreeViewItem?.LocaleItem);
            }

            GUI.enabled = true;

            IEnumerator ExecuteFillMissingLangProcess(LocaleTreeView treeView)
            {
                Debug.Log(
                    "[Localization] Starting fill language same with AvaiableLanguage for LocaleText!"
                );
                var rows = treeView.GetRows();
                foreach (var viewItem in rows.ToList())
                {
                    var assetItem = viewItem as AssetTreeViewItem;
                    FillLanguageSameAvaiableLanguage(assetItem?.Asset);
                    yield return null;
                }

                treeView.Reload();
                Debug.Log("[Localization] End fill language all LocaleText!");
            }

            IEnumerator ExecuteTranslateProcess(LocaleTreeView treeView)
            {
                SessionState.EraseInt("translate_all_locale_text_count");
                var rows = treeView.GetRows();
                foreach (var viewItem in rows.ToList())
                {
                    var assetItem = viewItem as AssetTreeViewItem;
                    TranslateMissingLocales(assetItem?.Asset);
                    yield return new WaitForSeconds(0.15f);
                }

                Debug.Log("[Localization] End translate all LocaleText!");
                Debug.Log(
                    "Total LocaleText Translated is :" + SessionState.GetInt("translate_all_locale_text_count", 0));
                SessionState.EraseInt("translate_all_locale_text_count");
            }
        }

        private static void TranslateMissingLocalesWithMenu(ScriptableLocaleBase asset)
        {
            var localeText = asset as LocaleText;
            var options = new List<GUIContent>();
            if (localeText != null)
            {
                Debug.Log("[Localization] Starting Translate LocaleText: " + localeText.name);
                foreach (var locale in localeText.TypedLocaleItems)
                {
                    if (!string.IsNullOrEmpty(locale.Value)) options.Add(new GUIContent(locale.Language.ToString()));
                }

                var mousePosition = Event.current.mousePosition;
                var popupPosition = new Rect(mousePosition.x, mousePosition.y, 0, 0);
                EditorUtility.DisplayCustomMenu(popupPosition,
                    options.ToArray(),
                    -1,
                    TranslateSelected,
                    localeText);
            }
        }

        /// <summary>
        /// Translate language by first language value
        /// </summary>
        /// <param name="asset"></param>
        private static void TranslateMissingLocales(ScriptableLocaleBase asset)
        {
            var localizedText = asset as LocaleText;
            var options = new List<GUIContent>();
            if (localizedText != null)
            {
                foreach (var locale in localizedText.TypedLocaleItems)
                {
                    if (!string.IsNullOrEmpty(locale.Value)) options.Add(new GUIContent(locale.Language.ToString()));
                }

                TranslateSelected(localizedText, options.Select(c => c.text).ToArray(), 0);
            }
        }

        /// <summary>
        /// Translate language by first language value
        /// </summary>
        /// <param name="asset"></param>
        private static void FillLanguageSameAvaiableLanguage(ScriptableLocaleBase asset)
        {
            var localizedText = asset as LocaleText;
            if (localizedText != null)
            {
                foreach (var item in asset.LocaleItems.ToList())
                {
                    int index = Array.FindIndex(asset.LocaleItems, x => x.Language == item.Language);
                    if (!LocaleSettings.AvailableLanguages.Contains(asset.LocaleItems[index].Language))
                    {
                        asset.LocaleItems.ToList().RemoveAt(index);
                    }
                }

                if (asset.LocaleItems.Length < LocaleSettings.AvailableLanguages.Count)
                {
                    foreach (var lang in LocaleSettings.AvailableLanguages)
                    {
                        int index = Array.FindIndex(asset.LocaleItems, x => x.Language == lang);
                        if (index >= 0) continue;

                        ScriptableLocaleEditor.AddLocale(asset);
                        index = asset.LocaleItems.Length - 1;
                        var localeItem = asset.LocaleItems[index];
                        localeItem.Language = lang;
                        localeItem.ObjectValue = "";
                    }
                }
            }
        }

        private static void TranslateSelected(object userData, string[] options, int selected)
        {
            var localizedText = (LocaleText)userData;

            var selectedLanguage = LocaleSettings.AllLanguages.FirstOrDefault(x => x.Name == options[selected]);
            if (selectedLanguage == null)
            {
                Debug.Assert(false, "Selected language not found in LocaleSettings.AllLanguages.");
                return;
            }

            if (!localizedText.TryGetLocaleValue(selectedLanguage, out string textValue))
            {
                Debug.Assert(false, "Selected language not exist in " + localizedText.name);
                return;
            }

            foreach (var locale in localizedText.TypedLocaleItems)
            {
                if (string.IsNullOrEmpty(locale.Value))
                {
                    var localeItem = locale;
                    Translator.Translate(new GoogleTranslateRequest(selectedLanguage, locale.Language, textValue),
                        e =>
                        {
                            var response = e.Responses.FirstOrDefault();
                            if (response != null)
                            {
                                localeItem.Value = response.translatedText;
                                SessionState.SetInt("translate_all_locale_text_count",
                                    SessionState.GetInt("translate_all_locale_text_count", 0) + 1);
                                Debug.Log("[Localization] Translate Successfull: " +
                                          localizedText.name);
                            }

                            EditorUtility.SetDirty(localizedText);
                        },
                        e => { Debug.LogError("Response (" + e.ResponseCode + "): " + e.Message); });
                }
            }
        }

        private static void AppLanguageContextMenu(object language)
        {
            Locale.CurrentLanguage = (Language)language;
        }

        private static void AddLocale(ref LocaleTreeView localeTreeView, ScriptableLocaleBase localizedAsset)
        {
            if (ScriptableLocaleEditor.AddLocale(localizedAsset)) localeTreeView.Reload();
        }
    }

    public enum LocaleTabType
    {
        Setting,
        Explore
    }

    /// <summary>
    /// Refreshes localization tab wizard if is opened.
    /// </summary>
    public class ScriptableLocalePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (CPLocalizationDrawer._localeTreeView != null) CPLocalizationDrawer._localeTreeView?.Reload();
        }
    }
}