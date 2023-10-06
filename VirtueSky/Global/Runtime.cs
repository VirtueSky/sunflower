using UnityEngine;

namespace VirtueSky.Global
{
    public class Runtime
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            var app = new GameObject("AppGlobal");
            App.InitMonoGlobalComponent(app.AddComponent<MonoGlobal>());
            Object.DontDestroyOnLoad(app);
        }
    }
}