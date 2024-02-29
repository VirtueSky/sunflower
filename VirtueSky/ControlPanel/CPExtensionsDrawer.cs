using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;
using VirtueSky.UtilsEditor;


namespace VirtueSky.ControlPanel.Editor
{
    public static class CPExtensionsDrawer
    {
        public static void OnDrawExtensions(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
#if UNITY_ANDROID
            GUILayout.Label("ANDROID EXTERNAL TOOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open Sdk"))
            {
                OpenSdkPath();
            }

            if (GUILayout.Button("Open Jdk"))
            {
                OpenJdkPath();
            }

            if (GUILayout.Button("Open Ndk"))
            {
                OpenNdkPath();
            }

            if (GUILayout.Button("Open Gradle"))
            {
                OpenGradlePath();
            }

            if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            {
                GUILayout.Space(10);
                CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Monitor only works on java sdk 8", MessageType.Warning);
                if (GUILayout.Button("Open Monitor"))
                {
                    OpenMonitor();
                }
            }
#endif
            GUILayout.EndVertical();
        }

        static void OpenSdkPath()
        {
            var path = $"{AndroidExternalToolsSettings.sdkRootPath}/";
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    FileExtension.OpenFolderInExplorer(path);
                    break;
                case OperatingSystemFamily.MacOSX:
                    FileExtension.OpenFolderInFinder(path);
                    break;
            }
        }

        static void OpenJdkPath()
        {
            var path = $"{AndroidExternalToolsSettings.jdkRootPath}/";
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    FileExtension.OpenFolderInExplorer(path);
                    break;
                case OperatingSystemFamily.MacOSX:
                    FileExtension.OpenFolderInFinder(path);
                    break;
            }
        }

        static void OpenNdkPath()
        {
            var path = $"{AndroidExternalToolsSettings.ndkRootPath}/";
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    FileExtension.OpenFolderInExplorer(path);
                    break;
                case OperatingSystemFamily.MacOSX:
                    FileExtension.OpenFolderInFinder(path);
                    break;
            }
        }

        static void OpenGradlePath()
        {
            var path = $"{AndroidExternalToolsSettings.gradlePath}/";
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    FileExtension.OpenFolderInExplorer(path);
                    break;
                case OperatingSystemFamily.MacOSX:
                    FileExtension.OpenFolderInFinder(path);
                    break;
            }
        }

        static void OpenMonitor()
        {
            string path = $"{AndroidExternalToolsSettings.sdkRootPath}/tools/monitor.bat";
            if (File.Exists(path))
            {
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.StartInfo.Verb = "runas";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
            }
        }
    }
}