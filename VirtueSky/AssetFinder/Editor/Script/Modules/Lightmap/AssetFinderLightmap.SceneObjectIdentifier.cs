using System;
using UnityEditor;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct SceneObjectIdentifier : IEquatable<SceneObjectIdentifier>
        {
            public long targetObject;

            public long targetPrefab;

            public SceneObjectIdentifier(GlobalObjectId id)
            {
                if (id.identifierType != 2) throw new ArgumentException("GlobalObjectId must refer to a scene object.", nameof(id));

                targetObject = unchecked((long)id.targetObjectId);
                targetPrefab = unchecked((long)id.targetPrefabId);
            }

            public bool Equals(SceneObjectIdentifier other)
            {
                return (targetObject == other.targetObject) && (targetPrefab == other.targetPrefab);
            }

            // public GlobalObjectId ToGlobalObjectId(SceneAsset scene)
            // {
            //     return ToGlobalObjectId(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(scene)));
            // }
            //
            // public GlobalObjectId ToGlobalObjectId(Scene scene)
            // {
            //     return ToGlobalObjectId(AssetDatabase.GUIDFromAssetPath(scene.path));
            // }
            //
            // public GlobalObjectId ToGlobalObjectId(GUID sceneGuid)
            // {
            //     GlobalObjectId id;
            //     GlobalObjectId.TryParse($"GlobalObjectId_V1-2-{sceneGuid}-{unchecked((ulong)targetObject)}-{unchecked((ulong)targetPrefab)}", out id);
            //     return id;
            // }
            //
            // public static Object SceneObjectIdentifierToObjectSlow(SceneAsset scene, SceneObjectIdentifier id)
            // {
            //     return SceneObjectIdentifierToObjectSlow(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(scene)), id);
            // }
            //
            // public static Object SceneObjectIdentifierToObjectSlow(Scene scene, SceneObjectIdentifier id)
            // {
            //     return SceneObjectIdentifierToObjectSlow(AssetDatabase.GUIDFromAssetPath(scene.path), id);
            // }
            //
            // public static Object SceneObjectIdentifierToObjectSlow(GUID sceneGuid, SceneObjectIdentifier id)
            // {
            //     return GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id.ToGlobalObjectId(sceneGuid));
            // }
            //
            // public static void SceneObjectIdentifiersToObjectsSlow(SceneAsset scene, SceneObjectIdentifier[] identifiers, Object[] outputObjects)
            // {
            //     SceneObjectIdentifiersToObjectsSlow(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(scene)), identifiers, outputObjects);
            // }
            //
            // public static void SceneObjectIdentifiersToObjectsSlow(Scene scene, SceneObjectIdentifier[] identifiers, Object[] outputObjects)
            // {
            //     SceneObjectIdentifiersToObjectsSlow(AssetDatabase.GUIDFromAssetPath(scene.path), identifiers, outputObjects);
            // }

            // public static void SceneObjectIdentifiersToObjectsSlow(GUID sceneGuid, SceneObjectIdentifier[] identifiers, Object[] outputObjects)
            // {
            //     var globalIdentifiers = new GlobalObjectId[identifiers.Length];
            //
            //     for (int i = 0; i < identifiers.Length; i++)
            //         globalIdentifiers[i] = identifiers[i].ToGlobalObjectId(sceneGuid);
            //
            //     GlobalObjectId.GlobalObjectIdentifiersToObjectsSlow(globalIdentifiers, outputObjects);
            // }
            //
            // public static void GetSceneObjectIdentifiersSlow(Object[] objects, SceneObjectIdentifier[] outputIdentifiers)
            // {
            //     var globalIdentifiers = new GlobalObjectId[outputIdentifiers.Length];
            //     GlobalObjectId.GetGlobalObjectIdsSlow(objects, globalIdentifiers);
            //
            //     for (int i = 0; i < outputIdentifiers.Length; i++)
            //     {
            //         outputIdentifiers[i] = new SceneObjectIdentifier(globalIdentifiers[i]);
            //     }
            // }
        }
    }
}
