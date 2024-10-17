using VirtueSky.Localization;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.LocalizationEditor
{
    public static class EditorMenu
    {
        private const string MENU_NAME = "Tools/Pancake/Localization/Change Locale/";

        [MenuItem("Tools/Pancake/Localization/Import CSV", priority = 10000)]
        private static void ImportCsv()
        {
            LocaleEditorUtil.Import();
        }

        [MenuItem("Tools/Pancake/Localization/Export CSV", priority = 10001)]
        private static void ExportCsv()
        {
            LocaleEditorUtil.Export();
        }

        private static void SetLanguage(Language currentLanguage)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Setting language only available when application is playing.");
                return;
            }

            var previousLanguage = Locale.CurrentLanguage;
            Locale.CurrentLanguage = currentLanguage;

            Menu.SetChecked(GetMenuName(previousLanguage), false);
            Menu.SetChecked(GetMenuName(currentLanguage), true);
        }

        [MenuItem(MENU_NAME + "/Afrikaans", priority = 10002)]
        private static void ChangeToAfrikaans()
        {
            SetLanguage(Language.Afrikaans);
        }

        [MenuItem(MENU_NAME + "/Arabic", priority = 10003)]
        private static void ChangeToArabic()
        {
            SetLanguage(Language.Arabic);
        }

        [MenuItem(MENU_NAME + "/Basque", priority = 10003)]
        private static void ChangeToBasque()
        {
            SetLanguage(Language.Basque);
        }

        [MenuItem(MENU_NAME + "/Belarusian", priority = 10004)]
        private static void ChangeToBelarusian()
        {
            SetLanguage(Language.Belarusian);
        }

        [MenuItem(MENU_NAME + "/Bulgarian", priority = 10005)]
        private static void ChangeToBulgarian()
        {
            SetLanguage(Language.Bulgarian);
        }

        [MenuItem(MENU_NAME + "/Catalan", priority = 10006)]
        private static void ChangeToCatalan()
        {
            SetLanguage(Language.Catalan);
        }

        [MenuItem(MENU_NAME + "/Chinese", priority = 10007)]
        private static void ChangeToChinese()
        {
            SetLanguage(Language.Chinese);
        }

        [MenuItem(MENU_NAME + "/Czech", priority = 10008)]
        private static void ChangeToCzech()
        {
            SetLanguage(Language.Czech);
        }

        [MenuItem(MENU_NAME + "/Danish", priority = 10009)]
        private static void ChangeToDanish()
        {
            SetLanguage(Language.Danish);
        }

        [MenuItem(MENU_NAME + "/Dutch", priority = 10010)]
        private static void ChangeToDutch()
        {
            SetLanguage(Language.Dutch);
        }

        [MenuItem(MENU_NAME + "/English", priority = 10011)]
        private static void ChangeToEnglish()
        {
            SetLanguage(Language.English);
        }

        [MenuItem(MENU_NAME + "/Estonian", priority = 10012)]
        private static void ChangeToEstonian()
        {
            SetLanguage(Language.Estonian);
        }

        [MenuItem(MENU_NAME + "/Faroese", priority = 10013)]
        private static void ChangeToFaroese()
        {
            SetLanguage(Language.Faroese);
        }

        [MenuItem(MENU_NAME + "/Finnish", priority = 10014)]
        private static void ChangeToFinnish()
        {
            SetLanguage(Language.Finnish);
        }

        [MenuItem(MENU_NAME + "/French", priority = 10015)]
        private static void ChangeToFrench()
        {
            SetLanguage(Language.French);
        }

        [MenuItem(MENU_NAME + "/German", priority = 10016)]
        private static void ChangeToGerman()
        {
            SetLanguage(Language.German);
        }

        [MenuItem(MENU_NAME + "/Greek", priority = 10017)]
        private static void ChangeToGreek()
        {
            SetLanguage(Language.Greek);
        }

        [MenuItem(MENU_NAME + "/Hebrew", priority = 10018)]
        private static void ChangeToHebrew()
        {
            SetLanguage(Language.Hebrew);
        }

        [MenuItem(MENU_NAME + "/Hungarian", priority = 10019)]
        private static void ChangeToHungarian()
        {
            SetLanguage(Language.Hungarian);
        }

        [MenuItem(MENU_NAME + "/Icelandic", priority = 10020)]
        private static void ChangeToIcelandic()
        {
            SetLanguage(Language.Icelandic);
        }

        [MenuItem(MENU_NAME + "/Indonesian", priority = 10021)]
        private static void ChangeToIndonesian()
        {
            SetLanguage(Language.Indonesian);
        }

        [MenuItem(MENU_NAME + "/Italian", priority = 10022)]
        private static void ChangeToItalian()
        {
            SetLanguage(Language.Italian);
        }

        [MenuItem(MENU_NAME + "/Japanese", priority = 10023)]
        private static void ChangeToJapanese()
        {
            SetLanguage(Language.Japanese);
        }

        [MenuItem(MENU_NAME + "/Korean", priority = 10024)]
        private static void ChangeToKorean()
        {
            SetLanguage(Language.Korean);
        }

        [MenuItem(MENU_NAME + "/Latvian", priority = 10025)]
        private static void ChangeToLatvian()
        {
            SetLanguage(Language.Latvian);
        }

        [MenuItem(MENU_NAME + "/Lithuanian", priority = 10026)]
        private static void ChangeToLithuanian()
        {
            SetLanguage(Language.Lithuanian);
        }

        [MenuItem(MENU_NAME + "/Norwegian", priority = 10027)]
        private static void ChangeToNorwegian()
        {
            SetLanguage(Language.Norwegian);
        }

        [MenuItem(MENU_NAME + "/Polish", priority = 10028)]
        private static void ChangeToPolish()
        {
            SetLanguage(Language.Polish);
        }

        [MenuItem(MENU_NAME + "/Portuguese", priority = 10029)]
        private static void ChangeToPortuguese()
        {
            SetLanguage(Language.Portuguese);
        }

        [MenuItem(MENU_NAME + "/Romanian", priority = 10030)]
        private static void ChangeToRomanian()
        {
            SetLanguage(Language.Romanian);
        }

        [MenuItem(MENU_NAME + "/Russian", priority = 10031)]
        private static void ChangeToRussian()
        {
            SetLanguage(Language.Russian);
        }

        [MenuItem(MENU_NAME + "/SerboCroatian", priority = 10032)]
        private static void ChangeToSerboCroatian()
        {
            SetLanguage(Language.SerboCroatian);
        }

        [MenuItem(MENU_NAME + "/Slovak", priority = 10033)]
        private static void ChangeToSlovak()
        {
            SetLanguage(Language.Slovak);
        }

        [MenuItem(MENU_NAME + "/Slovenian", priority = 10034)]
        private static void ChangeToSlovenian()
        {
            SetLanguage(Language.Slovenian);
        }

        [MenuItem(MENU_NAME + "/Spanish", priority = 10035)]
        private static void ChangeToSpanish()
        {
            SetLanguage(Language.Spanish);
        }

        [MenuItem(MENU_NAME + "/Swedish", priority = 10036)]
        private static void ChangeToSwedish()
        {
            SetLanguage(Language.Swedish);
        }

        [MenuItem(MENU_NAME + "/Thai", priority = 10037)]
        private static void ChangeToThai()
        {
            SetLanguage(Language.Thai);
        }

        [MenuItem(MENU_NAME + "/Turkish", priority = 10038)]
        private static void ChangeToTurkish()
        {
            SetLanguage(Language.Turkish);
        }

        [MenuItem(MENU_NAME + "/Ukrainian", priority = 10039)]
        private static void ChangeToUkrainian()
        {
            SetLanguage(Language.Ukrainian);
        }

        [MenuItem(MENU_NAME + "/Vietnamese", priority = 10040)]
        private static void ChangeToVietnamese()
        {
            SetLanguage(Language.Vietnamese);
        }

        private static string GetMenuName(Language language)
        {
            return $"{MENU_NAME}{language}";
        }
    }
}