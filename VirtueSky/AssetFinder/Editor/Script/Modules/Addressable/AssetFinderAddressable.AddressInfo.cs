using System;
using System.Collections.Generic;
namespace VirtueSky.AssetFinder.Editor
{
    public static partial class AssetFinderAddressable
    {
        [Serializable]
        public class AddressInfo
        {
            public string address;
            public string bundleGroup;
            public HashSet<string> assetGUIDs;
            public HashSet<string> childGUIDs;
        }
    }
}
