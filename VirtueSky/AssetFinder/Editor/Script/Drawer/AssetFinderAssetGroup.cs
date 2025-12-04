using System.Collections.Generic;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderAssetGroup
    {
        public string name;
        public HashSet<string> extension;
        public AssetFinderAssetGroup(string name, params string[] exts)
        {
            this.name = name;
            extension = new HashSet<string>();
            for (var i = 0; i < exts.Length; i++)
            {
                extension.Add(exts[i]);
            }
        }
    }
}
