using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
   [StructLayout(LayoutKind.Sequential)]
   [Serializable] 
   public struct AssetFinderID: IEquatable<AssetFinderID>
   {
       private const int SCENE_FLAG_BIT = 31;
       private const int SUB_INDEX_SHIFT = 21;
       private const uint SUB_INDEX_MASK = 0x3FFu << SUB_INDEX_SHIFT;
       private const uint MAIN_INDEX_MASK = 0x1FFFFFu;
       
       [SerializeField] private int value;
        
       private uint uValue
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => unchecked((uint)value);
           
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           set => this.value = unchecked((int)value);
       }
       
       private AssetFinderID(int pValue)
       {
           value = pValue;
       }
       
       public AssetFinderID(int mainIndex, int subIndex, bool isSceneObject)
       {
           if (mainIndex < 0 || mainIndex >= (1 << 21))
           {
               throw new ArgumentOutOfRangeException(nameof(mainIndex));
           }
           if (subIndex < 0 || subIndex >= (1 << 10))
           {
               throw new ArgumentOutOfRangeException(nameof(subIndex));
           }

           uint u = ( (uint)(isSceneObject ? 1 : 0) << SCENE_FLAG_BIT)
               | ((uint)subIndex << SUB_INDEX_SHIFT)
               | ((uint)mainIndex & MAIN_INDEX_MASK);
           value = unchecked((int)u);
       }

       [MethodImpl(MethodImplOptions.AggressiveInlining)]
       public AssetFinderID WithoutSubAssetIndex() => WithSubAssetIndex(0);
       
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
       public AssetFinderID WithSubAssetIndex(int subIndex)
       {   
           return new AssetFinderID(MainIndex, subIndex, IsSceneObject);
       }
       
       public bool IsSceneObject
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => (uValue & (1u << SCENE_FLAG_BIT)) != 0;
       } 
       private int MainIndex 
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => (int)(uValue & MAIN_INDEX_MASK);
       }
       
       private int SubIndex 
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => (int)((uValue & SUB_INDEX_MASK) >> SUB_INDEX_SHIFT);
       } 
       
       public int AssetIndex
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get  => MainIndex;
       } 
       
       public int SubAssetIndex
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => SubIndex;
       } 
       public int GameObjectIndex
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => MainIndex; 
       } 
       public int ComponentIndex
       {
           [MethodImpl(MethodImplOptions.AggressiveInlining)]
           get => SubIndex;
       } 

       public static implicit operator int(AssetFinderID id) => id.value;
       public static implicit operator AssetFinderID(int value) => new AssetFinderID(value);

       public override int GetHashCode() => value;
       public bool Equals(AssetFinderID other) => value == other.value;
       public override bool Equals(object obj) => obj is AssetFinderID other && Equals(other);
       
       public override string ToString() => IsSceneObject
           ? $"SceneObject - GameObjectIndex: {GameObjectIndex}, ComponentIndex: {ComponentIndex}"
           : $"Asset - AssetIndex: {AssetIndex}, SubAssetIndex: {SubAssetIndex}";
           
       // private sealed class Comparer : IEqualityComparer<AssetFinderID>
       // {
       //     public static readonly Comparer Instance = new Comparer();
       //     public bool Equals(AssetFinderID x, AssetFinderID y) => x.serializedValue == y.serializedValue;
       //     public int GetHashCode(AssetFinderID obj) => obj.serializedValue;
       // }
   }
}