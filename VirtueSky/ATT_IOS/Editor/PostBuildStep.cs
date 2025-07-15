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
        ModifyInfoPlist(pathToXcode);
        CopyGoogleServiceInfoPlist(pathToXcode);
        AddCapability(pathToXcode);
    }

    static void ModifyInfoPlist(string pathToXcode)
    {
        string plistPath = Path.Combine(pathToXcode, "Info.plist");
        PlistDocument plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict plistRoot = plistObj.root;
        // add privacy tracking
        plistRoot.SetString("NSUserTrackingUsageDescription",
            "Your data will be used to provide you a better and personalized ad experience.");
        // add non-exempt encryption
        plistRoot.SetBoolean("ITSAppUsesNonExemptEncryption", false);
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }

    static void CopyGoogleServiceInfoPlist(string pathToXcode)
    {
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
    }

    static void AddCapability(string pathToXcode)
    {
        string projPath = PBXProject.GetPBXProjectPath(pathToXcode);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGuid = proj.GetUnityMainTargetGuid();
        string frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

        string apsEnvironment = EditorUserBuildSettings.development ? "development" : "production";

        // Use a safe path
        string entitlementsFileName = "Unity-iPhone/Entitlements.entitlements";
        string entitlementsFullPath = Path.Combine(pathToXcode, entitlementsFileName);

        // Create Entitlements file
        PlistDocument entitlements = new PlistDocument();
        entitlements.root.SetString("aps-environment", apsEnvironment);
        File.WriteAllText(entitlementsFullPath, entitlements.WriteToString());

        // Capability manager
        var projCapability = new ProjectCapabilityManager(projPath, entitlementsFileName, "Unity-iPhone");

        projCapability.AddPushNotifications(false);

#if VIRTUESKY_APPLE_AUTH
        projCapability.AddSignInWithApple();
#endif

        projCapability.WriteToFile();

        // update build property for target
        proj.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsFileName);
        proj.SetBuildProperty(frameworkTargetGuid, "CODE_SIGN_ENTITLEMENTS", entitlementsFileName);

        proj.WriteToFile(projPath);
    }

    [PostProcessBuild]
    public static void OnPostProcessBuildAddFirebaseFile(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS) return;

        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        proj.AddFileToBuild(proj.GetUnityMainTargetGuid(),
            proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));
        proj.WriteToFile(projPath);
    }
}
#endif