#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class PostBuildStep
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
    {
        if (buildTarget != BuildTarget.iOS) return;

        // === 1. Modify Info.plist ===
        string plistPath = Path.Combine(pathToXcode, "Info.plist");
        PlistDocument plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict plistRoot = plistObj.root;
        plistRoot.SetString("NSUserTrackingUsageDescription", "Your data will be used to provide you a better and personalized ad experience.");
        File.WriteAllText(plistPath, plistObj.WriteToString());

        // === 2. Copy GoogleService-Info.plist ===
        string sourceSplitPath = Path.Combine("Assets", "GoogleService-Info.plist");
        string destSplitPath = Path.Combine(pathToXcode, "GoogleService-Info.plist");
        if (File.Exists(sourceSplitPath))
        {
            File.Copy(sourceSplitPath, destSplitPath, true);
            UnityEngine.Debug.Log("[PostBuildStep] Copied GoogleService-Info.plist to Xcode build folder.");
        }
        else
        {
            UnityEngine.Debug.LogWarning("[PostBuildStep] GoogleService-Info.plist not found in Assets.");
        }

        // === 3. Setup Push Notification Capability ===
        string projPath = PBXProject.GetPBXProjectPath(pathToXcode);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGuid = proj.GetUnityMainTargetGuid();

        // Determine environment type (Debug or Release)
        string apsEnvironment = EditorUserBuildSettings.development ? "development" : "production";

        // Create Entitlements.entitlements
        string entitlementsFileName = "Entitlements.entitlements";
        string entitlementsPath = Path.Combine(pathToXcode, entitlementsFileName);
        PlistDocument entitlements = new PlistDocument();
        entitlements.root.SetString("aps-environment", apsEnvironment);
        File.WriteAllText(entitlementsPath, entitlements.WriteToString());

        // Add Push Notification capability and entitlements
        proj.AddCapability(targetGuid, PBXCapabilityType.PushNotifications, entitlementsFileName);

        // Use SetBuildProperty to avoid duplicate CODE_SIGN_ENTITLEMENTS
        proj.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsFileName);

        proj.WriteToFile(projPath);
    }

    [PostProcessBuild]
    public static void OnPostProcessBuildAddFirebaseFile(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS) return;

        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        proj.AddFileToBuild(proj.GetUnityMainTargetGuid(), proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));
        proj.WriteToFile(projPath);
    }
}
#endif