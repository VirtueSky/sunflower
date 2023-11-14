using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public static class EditorResources
    {
        private const string RELATIVE_PATH = "VirtueSky/Utils/Editor/Icons";
        public static Texture2D BoxContentDark => GetFile.FindAssetWithPath<Texture2D>("box_content_dark.psd", RELATIVE_PATH);
        public static Texture2D BoxBackgroundDark => GetFile.FindAssetWithPath<Texture2D>("box_bg_dark.psd", RELATIVE_PATH);
        public static Texture2D EvenBackground => GetFile.FindAssetWithPath<Texture2D>("even_bg.png", RELATIVE_PATH);
        public static Texture2D EvenBackgroundBlue => GetFile.FindAssetWithPath<Texture2D>("even_bg_select.png", RELATIVE_PATH);
        public static Texture2D EvenBackgroundDark => GetFile.FindAssetWithPath<Texture2D>("even_bg_dark.png", RELATIVE_PATH);
        public static Texture2D ScriptableFactory => GetFile.FindAssetWithPath<Texture2D>("scriptable_factory.png", RELATIVE_PATH);
    }
}