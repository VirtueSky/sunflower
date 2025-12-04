using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace VirtueSky.AssetFinder.Editor
{
    internal static class AssetFinderHelper
    {


        public static IEnumerable<GameObject> getAllObjsInCurScene()
        {
            return GameObjectExtensions.GetAllGameObjectsInCurrentScenes();
        }

        private static IEnumerable<GameObject> GetGameObjectsInScene(Scene scene)
        {
            var rootObjects = new List<GameObject>();
            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (var i = 0; i < rootObjects.Count; ++i)
            {
                GameObject gameObject = rootObjects[i];

                foreach (GameObject item in getAllChild(gameObject))
                {
                    yield return item;
                }

                yield return gameObject;
            }
        }

        public static IEnumerable<GameObject> getAllChild(GameObject target)
        {
            return target.GetAllChildren(false);
        }

        public static IEnumerable<Object> GetAllRefObjects(GameObject obj)
        {
            return obj.GetAllObjectReferences();
        }

        public static int StringMatch(string pattern, string input)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(input)) return 0;
            
            pattern = pattern.ToLower();
            input = input.ToLower();
            
            if (input.Contains(pattern)) return 100;
            
            int score = 0;
            int patternIdx = 0;
            
            for (int i = 0; i < input.Length && patternIdx < pattern.Length; i++)
            {
                if (input[i] == pattern[patternIdx])
                {
                    score += 10;
                    patternIdx++;
                }
            }
            
            return patternIdx == pattern.Length ? score : 0;
        }

        public static string GetfileSizeString(long fileSize)
        {
            if (fileSize < 1024) return fileSize + " B";
            if (fileSize < 1024 * 1024) return (fileSize / 1024f).ToString("F1") + " KB";
            if (fileSize < 1024 * 1024 * 1024) return (fileSize / (1024f * 1024f)).ToString("F1") + " MB";
            return (fileSize / (1024f * 1024f * 1024f)).ToString("F1") + " GB";
        }
    }
}
