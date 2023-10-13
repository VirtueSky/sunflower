namespace VirtueSky.Utils
{
    using UnityEngine;
    using System;

    public abstract class ScriptableSettings<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = Resources.Load<T>(typeof(T).Name);
                if (instance == null) throw new Exception($"Scriptable setting for {typeof(T)} must be create before run!");
                return instance;
            }
        }
    }
}