using UnityEditor;
using UnityEngine;
using VirtueSky.Rating;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPInAppReviewDrawer
    {
        public static void OnDrawInAppReview(Rect position, ref StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP REVIEW", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create In App Review"))
            {
                RatingWindowEditor.CreateInAppReview();
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("INSTALL PACKAGE IN APP REVIEW", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Add Package In App Review And Dependency"))
            {
                RegistryManager.Add(ConstantPackage.PackageNameGGPlayReview,
                    ConstantPackage.MaxVersionGGPlayReview);
                RegistryManager.Add(ConstantPackage.PackageNameGGPlayCore,
                    ConstantPackage.MaxVersionGGPlayCore);
                RegistryManager.Add(ConstantPackage.PackageNameGGPlayCommon,
                    ConstantPackage.MaxVersionGGPlayCommon);
                RegistryManager.Add(ConstantPackage.PackageNameGGAndroidAppBundle,
                    ConstantPackage.MaxVersionAndroidAppBundle);
                RegistryManager.Add(ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
                RegistryManager.Resolve();
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                "Add scripting define symbols \"VIRTUESKY_RATING\" to use IAP",
                MessageType.Info);
            if (GUILayout.Button("Open Scripting Define Symbols tab to add"))
            {
                statePanelControl = StatePanelControl.ScriptDefineSymbols;
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("REMOVE PACKAGE IN APP REVIEW", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Remove Package In App Review And Dependency"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayReview);
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayCore);
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayCommon);
                RegistryManager.Remove(ConstantPackage.PackageNameGGAndroidAppBundle);
                RegistryManager.Remove(ConstantPackage.PackageNameGGExternalDependencyManager);
                RegistryManager.Resolve();
            }

            GUILayout.EndVertical();
        }
    }
}