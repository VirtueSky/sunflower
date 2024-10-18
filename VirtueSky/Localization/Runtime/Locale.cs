using System;
using System.Linq;
using UnityEngine;
using VirtueSky.DataStorage;

namespace VirtueSky.Localization
{
    public sealed class Locale
    {
        private static Locale instance;
        private Language _currentLanguage = Language.English;

        /// <summary>
        /// Raised when <see cref="CurrentLanguage"/> has been changed. 
        /// </summary>
        private event EventHandler<LocaleChangedEventArgs> OnLocaleChangedEvent;

        private static Locale Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Debug.LogError("Locale only avaiable when application playing!");
                    return null;
                }
#endif

                if (instance == null)
                {
                    instance = new Locale();
                    instance.SetDefaultLanguage();
                }

                return instance;
            }
        }

        public static Language CurrentLanguage
        {
            get => Instance._currentLanguage;
            set
            {
                if (Instance._currentLanguage != value)
                {
                    Instance._currentLanguage = value;
                    SetCurrentLanguageCode(value);
                    var oldValue = Instance._currentLanguage;
                    Instance._currentLanguage = value;
                    Instance.OnLanguageChanged(new LocaleChangedEventArgs(oldValue, value));
                }
            }
        }

        public static EventHandler<LocaleChangedEventArgs> LocaleChangedEvent
        {
            get => Instance.OnLocaleChangedEvent;
            set => Instance.OnLocaleChangedEvent = value;
        }

        private void OnLanguageChanged(LocaleChangedEventArgs e)
        {
            OnLocaleChangedEvent?.Invoke(this, e);
        }

        /// <summary>
        /// Sets the <see cref="CurrentLanguage"/> as <see cref="Application.systemLanguage"/>.
        /// </summary>
        public void SetSystemLanguage()
        {
            CurrentLanguage = Application.systemLanguage;
        }

        /// <summary>
        /// Sets the <see cref="CurrentLanguage"/> to default language defined in <see cref="LocaleSettings"/>.
        /// </summary>
        public void SetDefaultLanguage()
        {
            CurrentLanguage = LocaleSettings.AvailableLanguages.FirstOrDefault();
        }

        /// <summary>
        /// Finds all localized assets with type given. Finds all assets in the project if in Editor; otherwise,
        /// finds only that loaded in memory.
        /// </summary>
        /// <returns>Array of specified localized assets.</returns>
        public static T[] FindAllLocalizedAssets<T>() where T : ScriptableLocaleBase
        {
#if UNITY_EDITOR
            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T)}");
            var assets = new T[guids.Length];
            for (var i = 0; i < guids.Length; ++i)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                Debug.Assert(assets[i]);
            }

            return assets;
#else
            return Resources.FindObjectsOfTypeAll<T>();
#endif
        }

        /// <summary>
        /// Finds all localized assets.
        /// </summary>
        /// <seealso cref="FindAllLocalizedAssets{T}"/>
        /// <returns>Array of localized assets.</returns>
        public static ScriptableLocaleBase[] FindAllLocalizedAssets()
        {
            return FindAllLocalizedAssets<ScriptableLocaleBase>();
        }

        const string KEY_LANGUAGE = "KEY_LANGUAGE";
        public static string GetCurrentLanguageCode() => GameData.Get(KEY_LANGUAGE, "");
        public static void SetCurrentLanguageCode(Language language) => GameData.Set(KEY_LANGUAGE, language.Code);
        public static void SetCurrentLanguageCode(string languageCode) => GameData.Set(KEY_LANGUAGE, languageCode);

        public static void LoadLanguageSetting()
        {
            var list = LocaleSettings.AvailableLanguages;
            string lang = GetCurrentLanguageCode();
            // for first time when user not choose lang to display
            // use system language, if you don't use detect system language use first language in list available laguages
            if (string.IsNullOrEmpty(lang))
            {
                var index = 0;
                if (LocaleSettings.DetectDeviceLanguage)
                {
                    var nameSystemLang = UnityEngine.Application.systemLanguage.ToString();
                    index = list.FindIndex(x => x.Name == nameSystemLang);
                    if (index < 0) index = 0;
                }

                lang = list[index].Code;
                SetCurrentLanguageCode(lang);
            }

            int i = list.FindIndex(x => x.Code == lang);
            Locale.CurrentLanguage = list[i];
        }
    }
}