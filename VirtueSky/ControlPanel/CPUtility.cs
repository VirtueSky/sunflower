using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPUtility
    {
        public static void DrawButtonInstallPackage(string labelInstall, string labelRemove,
            string packageName, string packageVersion, float withButton = 400)
        {
            EditorGUILayout.BeginHorizontal();
            bool isInstall = RegistryManager.IsInstalledPackage(packageName);
            if (isInstall)
            {
                if (GUILayout.Button(labelRemove, GUILayout.Width(withButton)))
                {
                    RegistryManager.Remove(packageName);
                    RegistryManager.Resolve();
                }
            }
            else
            {
                if (GUILayout.Button(labelInstall, GUILayout.Width(withButton)))
                {
                    RegistryManager.AddOverrideVersion(packageName,
                        packageVersion);
                }
            }

            GUILayout.Space(10);
            GUILayout.Toggle(isInstall, TextIsInstallPackage(isInstall));
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawButtonAddDefineSymbols(string flagSymbols, float withButton = 400)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(flagSymbols, GUILayout.Width(withButton)))
            {
                EditorScriptDefineSymbols.SwitchFlag(flagSymbols);
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsFlagEnabled(flagSymbols),
                TextIsEnable(EditorScriptDefineSymbols.IsFlagEnabled(flagSymbols)));
            GUILayout.EndHorizontal();
        }

        public static string TextIsInstallPackage(bool isInstall)
        {
            return isInstall ? "Installed" : "Not installed";
        }

        public static string TextIsEnable(bool condition)
        {
            return condition ? "Enable" : "Disable";
        }
    }
}