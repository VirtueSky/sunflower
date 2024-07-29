#if UNITY_EDITOR && UNITY_IOS && VIRTUESKY_APPLE_AUTH
using AppleAuth.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;


namespace VirtueSky.GameServiceEditor
{
    public static class SignInWithApplePostprocessor
    {
        [PostProcessBuild(1)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS || target == BuildTarget.tvOS)
            {
                var projectPath = PBXProject.GetPBXProjectPath(path);
#if UNITY_2019_3_OR_NEWER
                var project = new PBXProject();
                project.ReadFromString(System.IO.File.ReadAllText(projectPath));
                var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null,
                    project.GetUnityMainTargetGuid());
                manager.AddSignInWithAppleWithCompatibility(project.GetUnityFrameworkTargetGuid());
                manager.WriteToFile();
#else
                        var manager =
 new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", PBXProject.GetUnityTargetName());
                        manager.AddSignInWithAppleWithCompatibility();
                        manager.WriteToFile();
#endif
            }
            else if (target == BuildTarget.StandaloneOSX)
            {
                AppleAuthMacosPostprocessorHelper.FixManagerBundleIdentifier(target, path);
            }
        }
    }
}
#endif