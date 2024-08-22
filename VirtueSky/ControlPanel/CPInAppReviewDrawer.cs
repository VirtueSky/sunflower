using UnityEditor;
using UnityEngine;
using VirtueSky.Rating;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPInAppReviewDrawer
    {
        public static void OnDrawInAppReview(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.InAppReview, "In App Review");
            GUILayout.Space(10);
            if (GUILayout.Button("Create Scriptable In App Review"))
            {
                RatingWindowEditor.CreateInAppReview();
            }

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Install Sdk");
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install Google Play Review", "Remove Google Play Review",
                ConstantPackage.PackageNameGGPlayReview, ConstantPackage.MaxVersionGGPlayReview);
            CPUtility.DrawButtonInstallPackage("Install Google Play Core", "Remove Google Play Core",
                ConstantPackage.PackageNameGGPlayCore, ConstantPackage.MaxVersionGGPlayCore);
            CPUtility.DrawButtonInstallPackage("Install Google Play Common", "Remove Google Play Common",
                ConstantPackage.PackageNameGGPlayCommon, ConstantPackage.MaxVersionGGPlayCommon);
            CPUtility.DrawButtonInstallPackage("Install Android App Bundle", "Remove Android App Bundle",
                ConstantPackage.PackageNameGGAndroidAppBundle, ConstantPackage.MaxVersionAndroidAppBundle);
            CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                "Remove Google External Dependency Manager",
                ConstantPackage.PackageNameGGExternalDependencyManager,
                ConstantPackage.MaxVersionGGExternalDependencyManager);

            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Define symbols");
            GUILayout.Space(10);
#if !VIRTUESKY_RATING
            EditorGUILayout.HelpBox(
                "Add scripting define symbols \"VIRTUESKY_RATING\" to use IAP",
                MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_RATING);

            GUILayout.EndVertical();
        }
    }
}