using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using System.Text;

namespace VirtueSky.Hierarchy.Data
{
    public enum HierarchySetting
    {
        TreeMapShow = 0,
        TreeMapColor = 77,
        TreeMapEnhanced = 78,
        TreeMapTransparentBackground = 60,

        MonoBehaviourIconShow = 4,
        MonoBehaviourIconShowDuringPlayMode = 18,
        MonoBehaviourIconIgnoreUnityMonobehaviour = 45,
        MonoBehaviourIconColor = 82,

        SeparatorShow = 8,
        SeparatorShowRowShading = 50,
        SeparatorColor = 80,
        SeparatorEvenRowShadingColor = 79,
        SeparatorOddRowShadingColor = 81,

        VisibilityShow = 1,
        VisibilityShowDuringPlayMode = 15,

        LockShow = 2,
        LockShowDuringPlayMode = 16,
        LockPreventSelectionOfLockedObjects = 41,

        StaticShow = 12,
        StaticShowDuringPlayMode = 25,

        ErrorShow = 6,
        ErrorShowDuringPlayMode = 20,
        ErrorShowIconOnParent = 27,
        ErrorShowScriptIsMissing = 28,
        ErrorShowReferenceIsNull = 29,
        ErrorShowReferenceIsMissing = 58,
        ErrorShowStringIsEmpty = 30,
        ErrorShowMissingEventMethod = 31,
        ErrorShowWhenTagOrLayerIsUndefined = 32,
        ErrorIgnoreString = 33,
        ErrorShowForDisabledComponents = 44,
        ErrorShowForDisabledGameObjects = 59,

        RendererShow = 7,
        RendererShowDuringPlayMode = 21,

        PrefabShow = 13,
        PrefabShowBreakedPrefabsOnly = 51,

        TagAndLayerShow = 5,
        TagAndLayerShowDuringPlayMode = 19,
        TagAndLayerSizeShowType = 68,
        TagAndLayerType = 34,
        TagAndLayerSizeType = 35,
        TagAndLayerSizeValuePixel = 36,
        TagAndLayerAligment = 37,
        TagAndLayerSizeValueType = 46,
        TagAndLayerSizeValuePercent = 47,
        TagAndLayerLabelSize = 48,
        TagAndLayerTagLabelColor = 66,
        TagAndLayerLayerLabelColor = 67,
        TagAndLayerLabelAlpha = 69,

        ColorShow = 9,
        ColorShowDuringPlayMode = 22,

        GameObjectIconShow = 3,
        GameObjectIconShowDuringPlayMode = 17,
        GameObjectIconSize = 63,

        TagIconShow = 14,
        TagIconShowDuringPlayMode = 26,
        TagIconListFoldout = 84,
        TagIconList = 40,
        TagIconSize = 62,

        LayerIconShow = 85,
        LayerIconShowDuringPlayMode = 86,
        LayerIconListFoldout = 87,
        LayerIconList = 88,
        LayerIconSize = 89,

        ChildrenCountShow = 11,
        ChildrenCountShowDuringPlayMode = 24,
        ChildrenCountLabelSize = 61,
        ChildrenCountLabelColor = 70,

        VerticesAndTrianglesShow = 53,
        VerticesAndTrianglesShowDuringPlayMode = 54,
        VerticesAndTrianglesCalculateTotalCount = 55,
        VerticesAndTrianglesShowTriangles = 56,
        VerticesAndTrianglesShowVertices = 64,
        VerticesAndTrianglesLabelSize = 57,
        VerticesAndTrianglesVerticesLabelColor = 71,
        VerticesAndTrianglesTrianglesLabelColor = 72,

        ComponentsShow = 10,
        ComponentsShowDuringPlayMode = 23,
        ComponentsIconSize = 65,
        ComponentsIgnore = 90,

        ComponentsOrder = 38,

        AdditionalIdentation = 39,
        AdditionalShowHiddenQHierarchyObjectList = 42,
        AdditionalShowModifierWarning = 43,
        AdditionalShowObjectListContent = 49,
        AdditionalHideIconsIfNotFit = 52,
        AdditionalBackgroundColor = 73,
        AdditionalActiveColor = 74,
        AdditionalInactiveColor = 75,
        AdditionalSpecialColor = 76,
    }

    public enum HierarchyTagAndLayerType
    {
        Always = 0,
        OnlyIfNotDefault = 1
    }

    public enum HierarchyTagAndLayerShowType
    {
        TagAndLayer = 0,
        Tag = 1,
        Layer = 2
    }

    public enum HierarchyTagAndLayerAligment
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public enum HierarchyTagAndLayerSizeType
    {
        Pixel = 0,
        Percent = 1
    }

    public enum HierarchyTagAndLayerLabelSize
    {
        Normal = 0,
        Big = 1,
        BigIfSpecifiedOnlyTagOrLayer = 2
    }

    public enum HierarchySize
    {
        Normal = 0,
        Big = 1
    }

    public enum HierarchySizeAll
    {
        Small = 0,
        Normal = 1,
        Big = 2
    }

    public enum HierarchyComponentEnum
    {
        LockComponent = 0,
        VisibilityComponent = 1,
        StaticComponent = 2,
        ColorComponent = 3,
        ErrorComponent = 4,
        RendererComponent = 5,
        PrefabComponent = 6,
        TagAndLayerComponent = 7,
        GameObjectIconComponent = 8,
        TagIconComponent = 9,
        LayerIconComponent = 10,
        ChildrenCountComponent = 11,
        VerticesAndTrianglesCount = 12,
        SeparatorComponent = 1000,
        TreeMapComponent = 1001,
        MonoBehaviourIconComponent = 1002,
        ComponentsComponent = 1003
    }

    public class TagTexture
    {
        public string tag;
        public Texture2D texture;

        public TagTexture(string tag, Texture2D texture)
        {
            this.tag = tag;
            this.texture = texture;
        }

        public static List<TagTexture> loadTagTextureList()
        {
            List<TagTexture> tagTextureList = new List<TagTexture>();
            string customTagIcon = HierarchySettings.getInstance().get<string>(HierarchySetting.TagIconList);
            string[] customTagIconArray = customTagIcon.Split(new char[] { ';' });
            List<string> tags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            for (int i = 0; i < customTagIconArray.Length - 1; i += 2)
            {
                string tag = customTagIconArray[i];
                if (!tags.Contains(tag)) continue;
                string texturePath = customTagIconArray[i + 1];

                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                if (texture != null)
                {
                    TagTexture tagTexture = new TagTexture(tag, texture);
                    tagTextureList.Add(tagTexture);
                }
            }

            return tagTextureList;
        }

        public static void saveTagTextureList(HierarchySetting setting, List<TagTexture> tagTextureList)
        {
            string result = "";
            for (int i = 0; i < tagTextureList.Count; i++)
                result += tagTextureList[i].tag + ";" +
                          AssetDatabase.GetAssetPath(tagTextureList[i].texture.GetInstanceID()) + ";";
            HierarchySettings.getInstance().set(setting, result);
        }
    }

    public class LayerTexture
    {
        public string layer;
        public Texture2D texture;

        public LayerTexture(string layer, Texture2D texture)
        {
            this.layer = layer;
            this.texture = texture;
        }

        public static List<LayerTexture> loadLayerTextureList()
        {
            List<LayerTexture> layerTextureList = new List<LayerTexture>();
            string customTagIcon = HierarchySettings.getInstance().get<string>(HierarchySetting.LayerIconList);
            string[] customLayerIconArray = customTagIcon.Split(new char[] { ';' });
            List<string> layers = new List<string>(UnityEditorInternal.InternalEditorUtility.layers);
            for (int i = 0; i < customLayerIconArray.Length - 1; i += 2)
            {
                string layer = customLayerIconArray[i];
                if (!layers.Contains(layer)) continue;
                string texturePath = customLayerIconArray[i + 1];

                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                if (texture != null)
                {
                    LayerTexture tagTexture = new LayerTexture(layer, texture);
                    layerTextureList.Add(tagTexture);
                }
            }

            return layerTextureList;
        }

        public static void saveLayerTextureList(HierarchySetting setting, List<LayerTexture> layerTextureList)
        {
            string result = "";
            for (int i = 0; i < layerTextureList.Count; i++)
                result += layerTextureList[i].layer + ";" +
                          AssetDatabase.GetAssetPath(layerTextureList[i].texture.GetInstanceID()) + ";";
            HierarchySettings.getInstance().set(setting, result);
        }
    }

    public delegate void SettingChangedHandler();

    public class HierarchySettings
    {
        public bool inited = false;
        public Rect lastRect;
        public bool isProSkin;
        public int indentLevel;
        public Texture2D checkBoxChecked;
        public Texture2D checkBoxUnchecked;
        public Texture2D restoreButtonTexture;
        public Vector2 scrollPosition = new Vector2();
        public Color separatorColor;
        public Color yellowColor;
        public float totalWidth;

        public HierarchyComponentsOrderList componentsOrderList;

        // CONST
        private const string PREFS_PREFIX = "QTools.QHierarchy_";
        private const string PREFS_DARK = "Dark_";
        private const string PREFS_LIGHT = "Light_";
        public const string DEFAULT_ORDER = "0;1;2;3;4;5;6;7;8;9;10;11;12";
        public const int DEFAULT_ORDER_COUNT = 13;
        private const string SETTINGS_FILE_NAME = "QSettingsObjectAsset";

        // PRIVATE
        private HierarchySettingsObject settingsObject;
        private Dictionary<int, object> defaultSettings = new Dictionary<int, object>();
        private HashSet<int> skinDependedSettings = new HashSet<int>();

        private Dictionary<int, SettingChangedHandler> settingChangedHandlerList =
            new Dictionary<int, SettingChangedHandler>();

        // SINGLETON
        private static HierarchySettings instance;

        public static HierarchySettings getInstance()
        {
            if (instance == null) instance = new HierarchySettings();
            return instance;
        }

        // CONSTRUCTOR
        private HierarchySettings()
        {
            string[] paths = AssetDatabase.FindAssets(SETTINGS_FILE_NAME);
            for (int i = 0; i < paths.Length; i++)
            {
                settingsObject = (HierarchySettingsObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(paths[i]),
                    typeof(HierarchySettingsObject));
                if (settingsObject != null) break;
            }

            if (settingsObject == null)
            {
                settingsObject = ScriptableObject.CreateInstance<HierarchySettingsObject>();
                string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(settingsObject));
                path = path.Substring(0, path.LastIndexOf("/"));
                AssetDatabase.CreateAsset(settingsObject, path + "/" + SETTINGS_FILE_NAME + ".asset");
                AssetDatabase.SaveAssets();
            }

            initSetting(HierarchySetting.TreeMapShow, true);
            initSetting(HierarchySetting.TreeMapColor, "39FFFFFF", "905D5D5D");
            initSetting(HierarchySetting.TreeMapEnhanced, true);
            initSetting(HierarchySetting.TreeMapTransparentBackground, true);

            initSetting(HierarchySetting.MonoBehaviourIconShow, true);
            initSetting(HierarchySetting.MonoBehaviourIconShowDuringPlayMode, true);
            initSetting(HierarchySetting.MonoBehaviourIconIgnoreUnityMonobehaviour, true);
            initSetting(HierarchySetting.MonoBehaviourIconColor, "A01B6DBB");

            initSetting(HierarchySetting.SeparatorShow, true);
            initSetting(HierarchySetting.SeparatorShowRowShading, true);
            initSetting(HierarchySetting.SeparatorColor, "FF303030", "48666666");
            initSetting(HierarchySetting.SeparatorEvenRowShadingColor, "13000000", "08000000");
            initSetting(HierarchySetting.SeparatorOddRowShadingColor, "00000000", "00FFFFFF");

            initSetting(HierarchySetting.VisibilityShow, true);
            initSetting(HierarchySetting.VisibilityShowDuringPlayMode, true);

            initSetting(HierarchySetting.LockShow, true);
            initSetting(HierarchySetting.LockShowDuringPlayMode, false);
            initSetting(HierarchySetting.LockPreventSelectionOfLockedObjects, false);

            initSetting(HierarchySetting.StaticShow, true);
            initSetting(HierarchySetting.StaticShowDuringPlayMode, false);

            initSetting(HierarchySetting.ErrorShow, true);
            initSetting(HierarchySetting.ErrorShowDuringPlayMode, false);
            initSetting(HierarchySetting.ErrorShowIconOnParent, false);
            initSetting(HierarchySetting.ErrorShowScriptIsMissing, true);
            initSetting(HierarchySetting.ErrorShowReferenceIsNull, false);
            initSetting(HierarchySetting.ErrorShowReferenceIsMissing, true);
            initSetting(HierarchySetting.ErrorShowStringIsEmpty, false);
            initSetting(HierarchySetting.ErrorShowMissingEventMethod, true);
            initSetting(HierarchySetting.ErrorShowWhenTagOrLayerIsUndefined, true);
            initSetting(HierarchySetting.ErrorIgnoreString, "");
            initSetting(HierarchySetting.ErrorShowForDisabledComponents, true);
            initSetting(HierarchySetting.ErrorShowForDisabledGameObjects, true);

            initSetting(HierarchySetting.RendererShow, false);
            initSetting(HierarchySetting.RendererShowDuringPlayMode, false);

            initSetting(HierarchySetting.PrefabShow, false);
            initSetting(HierarchySetting.PrefabShowBreakedPrefabsOnly, true);

            initSetting(HierarchySetting.TagAndLayerShow, true);
            initSetting(HierarchySetting.TagAndLayerShowDuringPlayMode, true);
            initSetting(HierarchySetting.TagAndLayerSizeShowType, (int)HierarchyTagAndLayerShowType.TagAndLayer);
            initSetting(HierarchySetting.TagAndLayerType, (int)HierarchyTagAndLayerType.OnlyIfNotDefault);
            initSetting(HierarchySetting.TagAndLayerAligment, (int)HierarchyTagAndLayerAligment.Left);
            initSetting(HierarchySetting.TagAndLayerSizeValueType, (int)HierarchyTagAndLayerSizeType.Pixel);
            initSetting(HierarchySetting.TagAndLayerSizeValuePercent, 0.25f);
            initSetting(HierarchySetting.TagAndLayerSizeValuePixel, 75);
            initSetting(HierarchySetting.TagAndLayerLabelSize, (int)HierarchyTagAndLayerLabelSize.Normal);
            initSetting(HierarchySetting.TagAndLayerTagLabelColor, "FFCCCCCC", "FF333333");
            initSetting(HierarchySetting.TagAndLayerLayerLabelColor, "FFCCCCCC", "FF333333");
            initSetting(HierarchySetting.TagAndLayerLabelAlpha, 0.35f);

            initSetting(HierarchySetting.ColorShow, true);
            initSetting(HierarchySetting.ColorShowDuringPlayMode, true);

            initSetting(HierarchySetting.GameObjectIconShow, false);
            initSetting(HierarchySetting.GameObjectIconShowDuringPlayMode, true);
            initSetting(HierarchySetting.GameObjectIconSize, (int)HierarchySizeAll.Small);

            initSetting(HierarchySetting.TagIconShow, false);
            initSetting(HierarchySetting.TagIconShowDuringPlayMode, true);
            initSetting(HierarchySetting.TagIconListFoldout, false);
            initSetting(HierarchySetting.TagIconList, "");
            initSetting(HierarchySetting.TagIconSize, (int)HierarchySizeAll.Small);

            initSetting(HierarchySetting.LayerIconShow, false);
            initSetting(HierarchySetting.LayerIconShowDuringPlayMode, true);
            initSetting(HierarchySetting.LayerIconListFoldout, false);
            initSetting(HierarchySetting.LayerIconList, "");
            initSetting(HierarchySetting.LayerIconSize, (int)HierarchySizeAll.Small);

            initSetting(HierarchySetting.ChildrenCountShow, false);
            initSetting(HierarchySetting.ChildrenCountShowDuringPlayMode, true);
            initSetting(HierarchySetting.ChildrenCountLabelSize, (int)HierarchySize.Normal);
            initSetting(HierarchySetting.ChildrenCountLabelColor, "FFCCCCCC", "FF333333");

            initSetting(HierarchySetting.VerticesAndTrianglesShow, false);
            initSetting(HierarchySetting.VerticesAndTrianglesShowDuringPlayMode, false);
            initSetting(HierarchySetting.VerticesAndTrianglesCalculateTotalCount, false);
            initSetting(HierarchySetting.VerticesAndTrianglesShowTriangles, false);
            initSetting(HierarchySetting.VerticesAndTrianglesShowVertices, true);
            initSetting(HierarchySetting.VerticesAndTrianglesLabelSize, (int)HierarchySize.Normal);
            initSetting(HierarchySetting.VerticesAndTrianglesVerticesLabelColor, "FFCCCCCC", "FF333333");
            initSetting(HierarchySetting.VerticesAndTrianglesTrianglesLabelColor, "FFCCCCCC", "FF333333");

            initSetting(HierarchySetting.ComponentsShow, false);
            initSetting(HierarchySetting.ComponentsShowDuringPlayMode, false);
            initSetting(HierarchySetting.ComponentsIconSize, (int)HierarchySizeAll.Small);
            initSetting(HierarchySetting.ComponentsIgnore, "");

            initSetting(HierarchySetting.ComponentsOrder, DEFAULT_ORDER);

            initSetting(HierarchySetting.AdditionalShowObjectListContent, false);
            initSetting(HierarchySetting.AdditionalShowHiddenQHierarchyObjectList, true);
            initSetting(HierarchySetting.AdditionalHideIconsIfNotFit, true);
            initSetting(HierarchySetting.AdditionalIdentation, 0);
            initSetting(HierarchySetting.AdditionalShowModifierWarning, true);

#if UNITY_2019_1_OR_NEWER
            initSetting(HierarchySetting.AdditionalBackgroundColor, "00383838", "00CFCFCF");
#else
            initSetting(QSetting.AdditionalBackgroundColor                  , "00383838", "00C2C2C2");
#endif
            initSetting(HierarchySetting.AdditionalActiveColor, "FFFFFF80", "CF363636");
            initSetting(HierarchySetting.AdditionalInactiveColor, "FF4F4F4F", "1E000000");
            initSetting(HierarchySetting.AdditionalSpecialColor, "FF2CA8CA", "FF1D78D5");
        }

        // DESTRUCTOR
        public void OnDestroy()
        {
            skinDependedSettings = null;
            defaultSettings = null;
            settingsObject = null;
            settingChangedHandlerList = null;
            instance = null;
        }

        // PUBLIC
        public T get<T>(HierarchySetting setting)
        {
            return (T)settingsObject.get<T>(getSettingName(setting));
        }

        public Color getColor(HierarchySetting setting)
        {
            string stringColor = (string)settingsObject.get<string>(getSettingName(setting));
            return HierarchyColorUtils.fromString(stringColor);
        }

        public void setColor(HierarchySetting setting, Color color)
        {
            string stringColor = HierarchyColorUtils.toString(color);
            set(setting, stringColor);
        }

        public void set<T>(HierarchySetting setting, T value, bool invokeChanger = true)
        {
            int settingId = (int)setting;
            settingsObject.set(getSettingName(setting), value);

            if (invokeChanger && settingChangedHandlerList.ContainsKey(settingId) &&
                settingChangedHandlerList[settingId] != null)
                settingChangedHandlerList[settingId].Invoke();

            EditorApplication.RepaintHierarchyWindow();
        }

        public void addEventListener(HierarchySetting setting, SettingChangedHandler handler)
        {
            int settingId = (int)setting;

            if (!settingChangedHandlerList.ContainsKey(settingId))
                settingChangedHandlerList.Add(settingId, null);

            if (settingChangedHandlerList[settingId] == null)
                settingChangedHandlerList[settingId] = handler;
            else
                settingChangedHandlerList[settingId] += handler;
        }

        public void removeEventListener(HierarchySetting setting, SettingChangedHandler handler)
        {
            int settingId = (int)setting;

            if (settingChangedHandlerList.ContainsKey(settingId) && settingChangedHandlerList[settingId] != null)
                settingChangedHandlerList[settingId] -= handler;
        }

        public void restore(HierarchySetting setting)
        {
            set(setting, defaultSettings[(int)setting]);
        }

        // PRIVATE
        private void initSetting(HierarchySetting setting, object defaultValueDark, object defaultValueLight)
        {
            skinDependedSettings.Add((int)setting);
            initSetting(setting, EditorGUIUtility.isProSkin ? defaultValueDark : defaultValueLight);
        }

        private void initSetting(HierarchySetting setting, object defaultValue)
        {
            string settingName = getSettingName(setting);
            defaultSettings.Add((int)setting, defaultValue);
            object value = settingsObject.get(settingName, defaultValue);
            if (value == null || value.GetType() != defaultValue.GetType())
            {
                settingsObject.set(settingName, defaultValue);
            }
        }

        private string getSettingName(HierarchySetting setting)
        {
            int settingId = (int)setting;
            string settingName = PREFS_PREFIX;
            if (skinDependedSettings.Contains(settingId))
                settingName += EditorGUIUtility.isProSkin ? PREFS_DARK : PREFS_LIGHT;
            settingName += setting.ToString("G");
            return settingName.ToString();
        }
    }
}