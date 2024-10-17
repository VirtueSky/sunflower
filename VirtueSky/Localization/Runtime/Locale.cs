using System;
using System.Linq;
using UnityEngine;

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
    }
}