using UnityEngine;

namespace VirtueSky.ObjectPooling
{
    public static class Pool
    {
        private static PoolHandle _poolHandle;

        public static void InitPool()
        {
            if (_poolHandle == null)
            {
                _poolHandle = new PoolHandle();
                _poolHandle.Initialize();
            }
        }


        #region API Spawn

        public static void PreSpawn(this PoolData poolData)
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.PreSpawn(poolData);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true)
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return null;
            }

            return _poolHandle.Spawn(prefab, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true) where T : Component
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return null;
            }

            return _poolHandle.Spawn(type, parent, worldPositionStays, initialize).GetComponent<T>();
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null,
            bool worldPositionStays = true,
            bool initialize = true)
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return null;
            }

            return _poolHandle.Spawn(prefab, position, rotation, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool initialize = true)
            where T : Component
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return null;
            }

            return _poolHandle.Spawn(type, position, rotation, parent, worldPositionStays, initialize)
                .GetComponent<T>();
        }

        #endregion

        #region API DeSpawn

        public static void DeSpawn(this GameObject gameObject, bool destroy = false, bool worldPositionStays = true)
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.DeSpawn(gameObject, destroy, worldPositionStays);
        }

        public static void DeSpawn<T>(this T type, bool destroy = false, bool worldPositionStays = true)
            where T : Component
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.DeSpawn(type, destroy, worldPositionStays);
        }

        public static void DeSpawnAll()
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.DeSpawnAll();
        }

        #endregion

        #region API Destroy

        public static void DestroyAll()
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.DestroyAll();
        }

        public static void DestroyAllWaitPools()
        {
            if (_poolHandle == null)
            {
                Debug.Log($"Please init pool before {System.Reflection.MethodBase.GetCurrentMethod()?.Name}");
                return;
            }

            _poolHandle.DestroyAllWaitPools();
        }

        #endregion
    }
}