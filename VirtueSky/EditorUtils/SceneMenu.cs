#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VirtueSky.EditorUtils
{
    public static class SceneMenu
    {
        // const string LevelEditorScene = "LevelEditorScene";
        // const string PrefabScene = "PrefabScene";
        // const string LauncherScene = "LauncherScene";
        // const string GameScene = "GameScene";
        // const string FarmScene = "FarmScene";
        //
        // static void ChangeScene(string name)
        // {
        //     EditorSceneManager.SaveOpenScenes();
        //     EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/" + name + ".unity");
        // }
        //
        // static bool CanChangeScene(string name)
        // {
        //     return HasScene(name) && SceneManager.GetActiveScene().name != name;
        // }
        //
        // static bool HasScene(string name)
        // {
        //     return File.Exists(Application.dataPath + "/Scenes/" + name + ".unity");
        // }
        //
        // [MenuItem("Scenes/Level Editor Scene", false, 11)]
        // static void OpenLevelEditorScene()
        // {
        //     ChangeScene(LevelEditorScene);
        // }
        //
        // [MenuItem("Scenes/Level Editor Scene", true, 11)]
        // static bool CanOpenLevelEditorScene()
        // {
        //     return CanChangeScene(LevelEditorScene);
        // }
        //
        // [MenuItem("Scenes/Prefab Scene", false, 11)]
        // static void OpenPrefabScene()
        // {
        //     ChangeScene(PrefabScene);
        // }
        //
        // [MenuItem("Scenes/Prefab Scene", true, 11)]
        // static bool CanOpenPrefabScene()
        // {
        //     return CanChangeScene(PrefabScene);
        // }
        //
        // [MenuItem("Scenes/Launcher Scene", false, 22)]
        // static void OpenLauncherScene()
        // {
        //     ChangeScene(LauncherScene);
        // }
        //
        // [MenuItem("Scenes/Launcher Scene", true, 22)]
        // static bool CanOpenLauncherScene()
        // {
        //     return CanChangeScene(LauncherScene);
        // }
        //
        // [MenuItem("Scenes/Game Scene", false, 22)]
        // static void OpenGameScene()
        // {
        //     ChangeScene(GameScene);
        // }
        //
        // [MenuItem("Scenes/Game Scene", true, 22)]
        // static bool CanOpenGameScene()
        // {
        //     return CanChangeScene(GameScene);
        // }
        //
        // [MenuItem("Scenes/Farm Scene", false, 33)]
        // static void OpenFarmScene()
        // {
        //     ChangeScene(FarmScene);
        // }
        //
        // [MenuItem("Scenes/Farm Scene", true, 33)]
        // static bool CanOpenFarmScene()
        // {
        //     return CanChangeScene(FarmScene);
        // }
        //
        // [MenuItem("Scenes/Play", false, 44)]
        // public static void PlayLauncherScene()
        // {
        //     if (HasScene(LauncherScene))
        //     {
        //         EditorSceneManager.SaveOpenScenes();
        //         ChangeScene(LauncherScene);
        //         EditorApplication.isPlaying = true;
        //     }
        // }
        //
        // [MenuItem("Scenes/Play", true, 44)]
        // static bool CanPlayLauncherScene()
        // {
        //     return HasScene(LauncherScene) && !Application.isPlaying;
        // }
    }
}
#endif