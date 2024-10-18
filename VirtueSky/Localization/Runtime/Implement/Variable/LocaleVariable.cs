using System;
using System.Linq;
using UnityEngine;

namespace VirtueSky.Localization
{
    [Serializable]
    public abstract class LocaleVariable<T> : ScriptableLocaleBase where T : class
    {
        /// <summary>
        /// Gets the defined locale items of the localized asset with concrete type.
        /// </summary>
        public LocaleItem<T>[] TypedLocaleItems => (LocaleItem<T>[])LocaleItems;

        /// <summary>
        /// Gets localized asset value regarding to <see cref="Locale.CurrentLanguage"/> if available.
        /// Gets first value of the asset if application is not playing.
        /// </summary>
        /// <seealso cref="Application.isPlaying"/>
        public T Value
        {
            get
            {
                var value = default(T);
                var isValueSet = false;
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
#endif
                    isValueSet = TryGetLocaleValue(Locale.CurrentLanguage, out value);
#if UNITY_EDITOR
                }
                else
                {
                    // Get default language from settings if is not in Play mode.
                    if (LocaleSettings.AvailableLanguages.Any())
                    {
                        isValueSet = TryGetLocaleValue(LocaleSettings.AvailableLanguages.First(), out value);
                    }
                }
#endif

                return isValueSet ? value : FirstValue;
            }
        }

        /// <summary>
        /// Gets the first locale value of the asset.
        /// </summary>
        public T FirstValue
        {
            get
            {
                var localeItem = TypedLocaleItems.FirstOrDefault();
                return localeItem?.Value;
            }
        }

        /// <summary>
        /// Returns the language given is whether exist or not.
        /// </summary>
        public bool HasLocale(Language language)
        {
            return LocaleItems.Any(x => x.Language == language);
        }

        /// <summary>
        /// Gets localized value if exist regarding to given language.
        /// </summary>
        /// <returns>True if exist; otherwise False</returns>
        public bool TryGetLocaleValue(Language language, out T value)
        {
            int index = Array.FindIndex(TypedLocaleItems, x => x.Language == language);
            if (index >= 0)
            {
                value = TypedLocaleItems[index].Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Returns LocalizedAsset value.
        /// </summary>
        /// <param name="asset">LocalizedAsset</param>
        public static implicit operator T(LocaleVariable<T> asset)
        {
            return asset ? asset.Value : default;
        }

        public override Type GetGenericType => typeof(T);
    }
}