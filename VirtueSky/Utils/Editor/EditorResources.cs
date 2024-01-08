using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public static class EditorResources
    {
        private const string RELATIVE_PATH = "VirtueSky/Utils/Editor/Icons";
        public static Texture2D BoxContentDark => FileExtension.FindAssetWithPath<Texture2D>("box_content_dark.psd", RELATIVE_PATH);
        public static Texture2D BoxBackgroundDark => FileExtension.FindAssetWithPath<Texture2D>("box_bg_dark.psd", RELATIVE_PATH);
        public static Texture2D EvenBackground => FileExtension.FindAssetWithPath<Texture2D>("even_bg.png", RELATIVE_PATH);
        public static Texture2D EvenBackgroundBlue => FileExtension.FindAssetWithPath<Texture2D>("even_bg_select.png", RELATIVE_PATH);
        public static Texture2D EvenBackgroundDark => FileExtension.FindAssetWithPath<Texture2D>("even_bg_dark.png", RELATIVE_PATH);
        public static Texture2D ScriptableFactory => FileExtension.FindAssetWithPath<Texture2D>("scriptable_factory.png", RELATIVE_PATH);
    }
}