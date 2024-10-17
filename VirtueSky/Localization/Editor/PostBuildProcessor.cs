using System.Collections.Generic;
using VirtueSky.Localization;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using System.IO;
using UnityEngine;
using UnityEditor.iOS.Xcode;
#endif

namespace MyNamespace
{
    public static class PostBuildProcessor
    {
        [PostProcessBuild(9999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                // Continue if any localization info exists.
                var localizations = GetLocalizations();
                if (localizations.Count == 0)
                {
                    return;
                }

                // Get plist.
                var plistPath = pathToBuiltProject + "/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get root of plist/dict.
                var rootDict = plist.root;
                var plistLocalizations = rootDict.CreateArray("CFBundleLocalizations");

                // Add localizations.
                foreach (string locale in localizations)
                {
                    plistLocalizations.AddString(locale);
                    Debug.Log("[LocalizationBuildPostprocessor] Localization added: " + locale);
                }

                // Save all changes.
                File.WriteAllText(plistPath, plist.WriteToString());
            }
#endif
        }

        private static List<string> GetLocalizations()
        {
            var localizations = new List<string>();
            if (LocaleSettings.Instance != null)
            {
                foreach (var language in LocaleSettings.AvailableLanguages)
                {
                    localizations.Add(language.Code);
                }
            }

            return localizations;
        }
    }
}