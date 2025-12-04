// #define AssetFinderDEV
using System;

namespace VirtueSky.AssetFinder.Editor
{
    public class AssetFinderDev
    {
        public static IDisposable NoLog => new NoLogScope();

        private readonly struct NoLogScope : IDisposable
        {
#if AssetFinderDEV
            internal NoLogScope(bool _) { }
            public void Dispose() { }
#else
            private readonly bool _saved;
            internal NoLogScope(bool _)
            {
                _saved = UnityEngine.Debug.unityLogger.logEnabled;
                UnityEngine.Debug.unityLogger.logEnabled = false;
            }
            public void Dispose() => UnityEngine.Debug.unityLogger.logEnabled = _saved;
#endif
        }
    }
}