using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class UnityObjectExtensions
    {
        public static bool IsSceneObject(this UnityObject obj)
        {
            // null, destroyed or asset-stored objects all return false
            return obj != null && !EditorUtility.IsPersistent(obj) && (obj is GameObject || obj is Component);
        }

        public static bool IsAssetObject(this UnityObject obj)
        {
            return obj != null && EditorUtility.IsPersistent(obj);
        }

        internal static GameObject GetGameObjectFromTarget(this UnityObject target)
        {
            if (!target) return null;
            if (target is GameObject go) return go;
            if (target is Component comp && comp != null) return comp.gameObject;
            return null;
        }
    }
} 