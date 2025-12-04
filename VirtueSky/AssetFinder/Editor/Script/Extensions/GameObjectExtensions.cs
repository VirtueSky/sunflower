using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class GameObjectExtensions
    {
        internal static IEnumerable<GameObject> GetAllChildren(this GameObject target, bool includeTarget = false)
        {
            if (target == null) yield break;
            
            if (includeTarget) yield return target;

            if (target.transform.childCount > 0)
            {
                for (var i = 0; i < target.transform.childCount; i++)
                {
                    var child = target.transform.GetChild(i).gameObject;
                    if (child == null) continue;
                    
                    yield return child;
                    
                    foreach (var grandChild in child.GetAllChildren(false))
                    {
                        yield return grandChild;
                    }
                }
            }
        }
        
        internal static IEnumerable<Transform> GetAllChildTransforms(this Transform root)
        {
            yield return root;
            
            for (var i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                foreach (var descendant in child.GetAllChildTransforms())
                {
                    yield return descendant;
                }
            }
        }
        
        internal static IEnumerable<GameObject> GetAllGameObjectsInCurrentScenes()
        {
            for (var j = 0; j < SceneManager.sceneCount; j++)
            {
                var scene = SceneManager.GetSceneAt(j);
                foreach (var gameObject in scene.GetAllGameObjects())
                {
                    yield return gameObject;
                }
            }

            if (EditorApplication.isPlaying)
            {
                GameObject temp = null;
                try
                {
                    temp = new GameObject();
                    UnityObject.DontDestroyOnLoad(temp);
                    var dontDestroyOnLoad = temp.scene;
                    UnityObject.DestroyImmediate(temp);
                    temp = null;

                    foreach (var gameObject in dontDestroyOnLoad.GetAllGameObjects())
                    {
                        yield return gameObject;
                    }
                }
                finally
                {
                    if (temp != null) UnityObject.DestroyImmediate(temp);
                }
            }
        }
        
        internal static IEnumerable<GameObject> GetAllGameObjects(this Scene scene)
        {
            if (!scene.isLoaded) yield break;
            
            var rootObjects = new List<GameObject>();
            scene.GetRootGameObjects(rootObjects);

            for (var i = 0; i < rootObjects.Count; ++i)
            {
                var rootObject = rootObjects[i];
                
                foreach (var child in rootObject.GetAllChildren(false))
                {
                    yield return child;
                }
                
                yield return rootObject;
            }
        }
        
        internal static void InitializeOrClear<T>(ref List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            else
            {
                list.Clear();
            }
        }
        
        internal static void InitializeOrClear<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<TKey, TValue>();
            }
            else
            {
                dictionary.Clear();
            }
        }
    }
} 