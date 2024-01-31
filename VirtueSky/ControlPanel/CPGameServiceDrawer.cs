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
            EditorGUILayout.BeginHorizontal();
            if (RegistryManager.IsInstalledPackage(ConstantPackage.PackageNameAppleSignIn))
            {
                if (GUILayout.Button("Remove Apple Sign In", GUILayout.Width(400)))
                {
                    RegistryManager.Remove(ConstantPackage.PackageNameAppleSignIn);
                    RegistryManager.Resolve();
                }
            }
            else
            {
                if (GUILayout.Button("Install Apple Sign In", GUILayout.Width(400)))
                {
                    RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameAppleSignIn,
                        ConstantPackage.MaxVersionAppleSignIn);
                }
            }

            GUILayout.Space(10);
            GUILayout.Toggle(RegistryManager.IsInstalledPackage(ConstantPackage.PackageNameAppleSignIn),
                TextIsInstall(RegistryManager.IsInstalledPackage(ConstantPackage.PackageNameAppleSignIn)));
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Install Google Play Game Service"))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/google-play-game.unitypackage"), false);
            }

            GUILayout.EndVertical();
        }

        static string TextIsInstall(bool isInstall)
        {
            if (isInstall)
            {
                return "Installed";
            }
            else
            {
                return "Not installed yet";
            }
        }
    }
}