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
                GUI.backgroundColor = CustomColor.Red.ToColor();
                if (GUILayout.Button(labelRemove, GUILayout.Width(withButton)))
                {
                    RegistryManager.Remove(packageName);
                    RegistryManager.Resolve();
                }
            }
            else
            {
                GUI.backgroundColor = CustomColor.Green.ToColor();
                if (GUILayout.Button(labelInstall, GUILayout.Width(withButton)))
                {
                    RegistryManager.AddOverrideVersion(packageName,
                        packageVersion);
                }
            }

            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);
            GUILayout.Toggle(isInstall, TextIsInstallPackage(isInstall));
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawButtonAddDefineSymbols(string flagSymbols, float withButton = 400)
        {
            GUILayout.BeginHorizontal();
            bool isAddSymbols = EditorScriptDefineSymbols.IsFlagEnabled(flagSymbols);
            string labelButton = isAddSymbols ? "Remove ---> " : "Add ---> ";
            if (isAddSymbols)
            {
                GUI.backgroundColor = CustomColor.Red.ToColor();
            }
            else
            {
                GUI.backgroundColor = CustomColor.Green.ToColor();
            }

            if (GUILayout.Button(labelButton + flagSymbols, GUILayout.Width(withButton)))
            {
                EditorScriptDefineSymbols.SwitchFlag(flagSymbols);
            }

            GUI.backgroundColor = Color.white;

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

        public static void DrawCustomLine(float with, Vector2 positionPointStart, Vector2 positionPointEnd)
        {
            Handles.DrawAAPolyLine(with, positionPointStart, positionPointEnd);
        }

        public static void DrawLineLastRectY(float with, float posXPointStart, float posXPointEnd, float offsetY = 10)
        {
            Handles.DrawAAPolyLine(with, new Vector3(posXPointStart, GUILayoutUtility.GetLastRect().y + offsetY),
                new Vector3(posXPointEnd, GUILayoutUtility.GetLastRect().y + offsetY));
        }

        public static void DrawLineLastRectX(float with, float posYPointStart, float posYPointEnd, float offsetX = 10)
        {
            Handles.DrawAAPolyLine(with, new Vector3(GUILayoutUtility.GetLastRect().x + offsetX, posYPointStart),
                new Vector3(GUILayoutUtility.GetLastRect().x + offsetX, posYPointEnd));
        }

        public static void DrawToggle(ref bool value, string text, System.Action draw)
        {
            value = GUILayout.Toggle(value, text);
            if (value)
            {
                draw?.Invoke();
            }
        }
    }
}