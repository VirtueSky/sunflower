using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPGameServiceDrawer
    {
        public static void OnDrawGameService()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("GAME SERVICE", EditorStyles.boldLabel);
            GUILayout.Space(10);

            CPUtility.DrawButtonInstallPackage("Install Apple Sign In", "Remove Apple Sign In",
                ConstantPackage.PackageNameAppleSignIn, ConstantPackage.MaxVersionAppleSignIn);

            if (GUILayout.Button("Install Google Play Game Service", GUILayout.Width(400)))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/google-play-game.unitypackage"), false);
            }

            GUILayout.Space(10);
            CPUtility.GuiLine();
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPLE_AUTH);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_GPGS);
            GUILayout.EndVertical();
        }
    }
}