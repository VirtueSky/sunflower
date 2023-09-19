using UnityEngine;

namespace VirtueSky.AutoRuntimeInit
{
    public class Runtime
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            GameObject obj = new GameObject("AutoInitialize");
            obj.AddComponent<VirtueSky.AutoRuntimeInit.AutoInitialize>();
        }
    }
}