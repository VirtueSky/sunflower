using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
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
            bool isAddSymbols = EditorScriptDefineSymbols.IsFlagEnabled(flagSymbols);
            string labelButton = isAddSymbols ? "Remove Symbols ---> " : "Add Symbols ---> ";
            if (GUILayout.Button(labelButton + flagSymbols, GUILayout.Width(withButton)))
            {
                EditorScriptDefineSymbols.SwitchFlag(flagSymbols);
            }

            GUILayout.Space(10);
            GUILayout.Toggle(isAddSymbols, TextIsEnable(isAddSymbols));
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

        public static void GuiLine(int i_height = 1, CustomColor customColor = CustomColor.Black)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, customColor.ToColor());
        }
    }
}