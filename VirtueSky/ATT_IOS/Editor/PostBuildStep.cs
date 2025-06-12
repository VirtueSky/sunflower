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
        if (buildTarget == BuildTarget.iOS)
        {
            // Retrieve the plist file from the Xcode project directory:
            string plistPath = pathToXcode + "/Info.plist";
            PlistDocument plistObj = new PlistDocument();


            // Read the values from the plist file:
            plistObj.ReadFromString(File.ReadAllText(plistPath));

            // Set values from the root object:
            PlistElementDict plistRoot = plistObj.root;

            // Set the description key-value in the plist:
            plistRoot.SetString("NSUserTrackingUsageDescription", "Your data will be used to provide you a better and personalized ad experience.");

            // Save changes to the plist:
            File.WriteAllText(plistPath, plistObj.WriteToString());
            
            // --- Copy GoogleService-Info.split if exist ---
            string sourceSplitPath = Path.Combine("Assets", "GoogleService-Info.split");
            string destSplitPath = Path.Combine(pathToXcode, "GoogleService-Info.split");
            if (File.Exists(sourceSplitPath))
            {
                File.Copy(sourceSplitPath, destSplitPath, true);
                UnityEngine.Debug.Log("[PostBuildStep] Copied GoogleService-Info.split to Xcode build folder.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[PostBuildStep] GoogleService-Info.split not found in Assets.");
            }
        }
    }

    [PostProcessBuild]
    public static void OnPostProcessBuildAddFirebaseFile(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);
            proj.AddFileToBuild(proj.GetUnityMainTargetGuid(), proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist"));
            proj.WriteToFile(projPath);
        }
    }
}
#endif