using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public static class EditorResources
    {
        private const string RELATIVE_PATH = "VirtueSky/Utils/Editor/Icons";

        public static Texture2D BoxContentDark =>
            FileExtension.FindAssetWithPath<Texture2D>("box_content_dark.psd", RELATIVE_PATH);

        public static Texture2D BoxBackgroundDark =>
            FileExtension.FindAssetWithPath<Texture2D>("box_bg_dark.psd", RELATIVE_PATH);

        public static Texture2D EvenBackground =>
            FileExtension.FindAssetWithPath<Texture2D>("even_bg.png", RELATIVE_PATH);

        public static Texture2D EvenBackgroundBlue =>
            FileExtension.FindAssetWithPath<Texture2D>("even_bg_select.png", RELATIVE_PATH);

        public static Texture2D EvenBackgroundDark =>
            FileExtension.FindAssetWithPath<Texture2D>("even_bg_dark.png", RELATIVE_PATH);

        public static Texture2D ScriptableFactory =>
            FileExtension.FindAssetWithPath<Texture2D>("scriptable_factory.png", RELATIVE_PATH);

        public static Texture2D IconAds => FileExtension.FindAssetWithPath<Texture2D>("icon_ads.png", RELATIVE_PATH);
        public static Texture2D IconIap => FileExtension.FindAssetWithPath<Texture2D>("icon_iap.png", RELATIVE_PATH);
        public static Texture2D IconLocale => FileExtension.FindAssetWithPath<Texture2D>("icon_locale.png", RELATIVE_PATH);

        public static Texture2D IconScriptableEvent =>
            FileExtension.FindAssetWithPath<Texture2D>("scriptable_event.png", RELATIVE_PATH);

        public static Texture2D IconScriptableVariable =>
            FileExtension.FindAssetWithPath<Texture2D>("scriptable_variable.png", RELATIVE_PATH);

        public static Texture2D IconAudio =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_audio.png", RELATIVE_PATH);

        public static Texture2D IconFirebase =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_firebase.png", RELATIVE_PATH);

        public static Texture2D IconAdjust =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_adjust.png", RELATIVE_PATH);

        public static Texture2D IconAppsFlyer =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_appsflyer.png", RELATIVE_PATH);


        public static Texture2D IconInAppReview =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_in_app_review.png", RELATIVE_PATH);


        public static Texture2D IconGameService =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_game_service.png", RELATIVE_PATH);

        public static Texture2D IconFolder =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_folder.png", RELATIVE_PATH);

        public static Texture2D IconHierarchy =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_hierarchy.png", RELATIVE_PATH);

        public static Texture2D IconPushNotification =>
            FileExtension.FindAssetWithPath<Texture2D>("script_noti.png", RELATIVE_PATH);

        public static Texture2D IconUnity =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_unity.png", RELATIVE_PATH);

        public static Texture2D IconExtension =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_extension.png", RELATIVE_PATH);

        public static Texture2D IconPackage =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_package.png", RELATIVE_PATH);

        public static Texture2D IconAbout =>
            FileExtension.FindAssetWithPath<Texture2D>("icon_about.png", RELATIVE_PATH);

        public static Texture2D IconVirtueSky =>
            FileExtension.FindAssetWithPath<Texture2D>("virtuesky_removebg.png", RELATIVE_PATH);
    }
}