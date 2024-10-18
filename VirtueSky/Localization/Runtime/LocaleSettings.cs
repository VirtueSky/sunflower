using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Linq;
using VirtueSky.Utils;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_scriptable")]
    public sealed class LocaleSettings : ScriptableSettings<LocaleSettings>
    {
        [SerializeField] private List<Language> availableLanguages = new(1) { Language.English };
        [SerializeField] private bool detectDeviceLanguage;
        [SerializeField] private string importLocation = "Assets/_Sunflower/Scriptable/Localization";
        [SerializeField] private string googleTranslateApiKey;
        [SerializeField] private string spreadsheetKey;
        [SerializeField, TextArea] private string serviceAccountCredential;

        public static bool DetectDeviceLanguage => Instance.detectDeviceLanguage;
        public static string ImportLocation => Instance.importLocation;
        public static List<Language> AvailableLanguages => Instance.availableLanguages;
        public static string GoogleTranslateApiKey => Instance.googleTranslateApiKey;
        public static string SpreadsheetKey => Instance.spreadsheetKey;
        public static string ServiceAccountCredential => Instance.serviceAccountCredential;

        public static List<Language> AllLanguages
        {
            get
            {
                var languages = new List<Language>();
                languages.AddRange(Language.BuiltInLanguages);
                languages.AddRange(AvailableLanguages.Filter(l => l.Custom));
                return languages;
            }
        }
    }
}