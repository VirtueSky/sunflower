using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using Object = UnityEngine.Object;
namespace VirtueSky.AssetFinder.Editor
{
    internal enum SubAssetType
    {
        Unknown,
        Sprite, // Part of a multi-sprite texture or sprite atlas
        Mesh, // Extracted from FBX, OBJ, or Blender files
        GameObject, // Child of prefab
        Material, // Can be embedded inside FBX models
        ScriptableObject, // Nested ScriptableObjects
        
        AnimationClip, // From FBX models or Animation files
        Avatar, // Humanoid Avatar inside an FBX model
        AudioMixerSnapshot, // Part of an Audio Mixer
        LightingDataAsset, // Generated lighting data
        BlendShape, // Internal morph target data from FBX
        PhysicsMaterial2D, // Sometimes stored inside assets
        ShaderVariantCollection, // Shader variant sub-assets
        TerrainLayer, // Used in Terrain systems
        Texture2DArray, // Array of textures stored inside a single asset
        Texture3D, // 3D texture data (e.g., volumetric effects)
        Cubemap, // Used in reflection probes
        VideoClip, // If part of a larger asset
        SpineAnimation, // Sub-asset in Spine skeleton data
        SpineSkeletonData, // Spine skeleton metadata
    }
    
    [Serializable] internal class SubAssetDetail
    {
        public long fileId;
        public string name;
        public SubAssetType type;

        public SubAssetDetail(long fileId, Object subAsset)
        {
            this.fileId = fileId;
            name = subAsset.name;
            type = GetSubAssetType(subAsset);
        }
        
        // Direct Type Mapping (UnityEngine types)
        private static readonly Dictionary<Type, SubAssetType> TypeToSubAssetType = new Dictionary<Type, SubAssetType>
        {
            { typeof(Sprite), SubAssetType.Sprite },
            { typeof(Mesh), SubAssetType.Mesh },
            { typeof(GameObject), SubAssetType.GameObject },
            { typeof(AnimationClip), SubAssetType.AnimationClip },
            { typeof(Avatar), SubAssetType.Avatar },
            { typeof(Material), SubAssetType.Material },
            { typeof(AudioMixerSnapshot), SubAssetType.AudioMixerSnapshot },
            { typeof(LightingDataAsset), SubAssetType.LightingDataAsset },
            { typeof(PhysicsMaterial2D), SubAssetType.PhysicsMaterial2D },
            { typeof(ShaderVariantCollection), SubAssetType.ShaderVariantCollection },
            { typeof(TerrainLayer), SubAssetType.TerrainLayer },
            { typeof(Texture2DArray), SubAssetType.Texture2DArray },
            { typeof(Texture3D), SubAssetType.Texture3D },
            { typeof(Cubemap), SubAssetType.Cubemap },
            { typeof(VideoClip), SubAssetType.VideoClip },
        };

        // String-based lookup for non-UnityEngine types (Spine, BlendShape, etc.)
        private static readonly Dictionary<string, SubAssetType> TypeNameToSubAssetType = new Dictionary<string, SubAssetType>
        {
            { "BlendShape", SubAssetType.BlendShape }, // Internal Unity representation
            { "SpineAnimation", SubAssetType.SpineAnimation }, // Spine-specific
            { "SpineSkeletonData", SubAssetType.SpineSkeletonData }
        };

        /// <summary>
        /// Gets the SubAssetType from an object instance.
        /// </summary>
        public static SubAssetType GetSubAssetType(Object subAsset)
        {
            if (subAsset == null) return SubAssetType.Unknown;

            Type assetType = subAsset.GetType();

            // 1. Check UnityEngine types directly
            if (TypeToSubAssetType.TryGetValue(assetType, out SubAssetType mappedType))
            {
                return mappedType;
            }
            
            // 2. Check if it's a ScriptableObject (catch all ScriptableObjects)
            if (typeof(ScriptableObject).IsAssignableFrom(assetType))
            {
                return SubAssetType.ScriptableObject;
            }

            // 3. Check by type name (for custom asset types like Spine)
            return TypeNameToSubAssetType.TryGetValue(assetType.Name, out mappedType)
                ? mappedType : SubAssetType.Unknown;

        }
        
        
    }
}
