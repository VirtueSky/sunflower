namespace VirtueSky.Tween
{
    using UnityEngine;


    /// <summary>
    /// Unity singleton. A Monobehaviour variant of the Singleton Pattern
    /// </summary>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;

        /// <summary>
        /// Gets the instance or instantiates an instance on a new Gameobject
        /// </summary>
        /// <value>The instance.</value>
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        _instance = new GameObject().AddComponent<T>();
                        _instance.gameObject.name = _instance.GetType().Name;
                    }
                }

                return _instance;
            }
        }

        public static bool HasInstance
        {
            get { return !IsDestroyed; }
        }

        public static bool IsDestroyed
        {
            get { return (_instance == null) ? true : false; }
        }

        protected virtual void OnDestroy()
        {
            onDestruction();
            _instance = null;
        }

        protected void OnApplicationQuit()
        {
            onDestruction();
            _instance = null;
        }

        protected virtual void onDestruction()
        {
            StopAllCoroutines();
        }
    }
}