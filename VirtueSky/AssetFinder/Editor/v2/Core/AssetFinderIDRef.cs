using System;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    [Serializable] internal class AssetFinderIDRef
    {
        [SerializeField] internal AssetFinderID fromId;
        [SerializeField] internal AssetFinderID toId;
        
        // public string type; // The class that reference this asset
        // public string path; // The property path that the class used to reference to asset
        // public bool isWeak; // Weak: Addressable / Atlas

        public override string ToString()
        {
            return $"{fromId.ToString()} -> {toId.ToString()}";
        }
    }
}
