using System.IO;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderChunk
    {
        public byte[] buffer;
        public string file;
        public long size;
        public FileStream stream;
        public bool streamError;
        public bool streamInited;
    }
}
