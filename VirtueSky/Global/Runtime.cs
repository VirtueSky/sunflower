using UnityEngine;

namespace VirtueSky.Global
{
    public class Runtime
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            var app = new GameObject("App");
            App.InitMonoGlobalComponent(app.AddComponent<MonoGlobalComponent>());
            Object.DontDestroyOnLoad(app);
        }
    }
}