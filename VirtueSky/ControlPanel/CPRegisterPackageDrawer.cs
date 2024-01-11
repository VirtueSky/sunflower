using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPRegisterPackageDrawer
    {
        private static string inputPackageFullNameAdd = "";
        private static string inputPackageFullNameRemove = "";
        private static Vector2 scrollPositionFileManifest = Vector2.zero;

        public static void OnDrawImportPackageByManifest(Rect position, EditorWindow editorWindow)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("REGISTER PACKAGE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label("Add Package", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            inputPackageFullNameAdd = EditorGUILayout.TextField(inputPackageFullNameAdd);
            if (GUILayout.Button("Add", GUILayout.Width(70)))
            {
                if (inputPackageFullNameAdd == "")
                {
                    Debug.LogError("The input add package field is null");
                    return;
                }

                inputPackageFullNameAdd = inputPackageFullNameAdd.Trim();
                string inputPackageName = inputPackageFullNameAdd.Split(':').First();
                (string packageName, string packageVersion) =
                    RegistryManager.GetPackageInManifestByPackageName(inputPackageName);
                if (packageName == inputPackageName)
                {
                    RegistryManager.RemovePackageInManifest(packageName + packageVersion);
                }

                RegistryManager.AddPackageInManifest(inputPackageFullNameAdd);
                RegistryManager.Resolve();
                //clear text Field
                inputPackageFullNameAdd = string.Empty;
                GUIUtility.keyboardControl = 0;
                editorWindow.Repaint();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("Remove Package", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            inputPackageFullNameRemove = EditorGUILayout.TextField(inputPackageFullNameRemove);
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                if (inputPackageFullNameRemove == "")
                {
                    Debug.LogError("The input remove package field is null");
                    return;
                }

                inputPackageFullNameRemove = inputPackageFullNameRemove.Trim();
                string inputPackageName = inputPackageFullNameRemove.Split(':').First();
                (string packageName, string packageVersion) =
                    RegistryManager.GetPackageInManifestByPackageName(inputPackageName);
                if (packageName + packageVersion == inputPackageFullNameRemove)
                {
                    RegistryManager.RemovePackageInManifest(inputPackageFullNameRemove);
                }
                else if (packageName == inputPackageName)
                {
                    Debug.LogError("Input package version is not available");
                }
                else
                {
                    Debug.LogError("Input package is not available");
                }

                RegistryManager.Resolve();
                //clear text Field
                inputPackageFullNameRemove = string.Empty;
                GUIUtility.keyboardControl = 0;
                editorWindow.Repaint();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("Manifest.json", EditorStyles.boldLabel);
            GUILayout.Space(10);
            scrollPositionFileManifest =
                EditorGUILayout.BeginScrollView(scrollPositionFileManifest,
                    GUILayout.Height(250));
            string manifestContent = EditorGUILayout.TextArea(
                System.IO.File.ReadAllText(FileExtension.ManifestPath),
                GUILayout.ExpandHeight(true));
            RegistryManager.WriteAllManifestContent(manifestContent);
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Resolve Package"))
            {
                RegistryManager.Resolve();
            }

            GUILayout.EndVertical();
        }
    }
}