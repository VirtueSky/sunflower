using UnityEngine;

namespace VirtueSky.Core
{
    public class CacheComponent<T> : BaseMono
    {
        public T component;
        public Transform CacheTransform { get; private set; }

        protected virtual void Awake()
        {
            if (CacheTransform == null) CacheTransform = transform;
            GetCacheComponent();
        }

        void GetCacheComponent()
        {
            if (component == null)
            {
                component = GetComponent<T>();
            }
        }
#if UNITY_EDITOR
        protected virtual void Reset()
        {
            GetCacheComponent();
        }
#endif
    }
}