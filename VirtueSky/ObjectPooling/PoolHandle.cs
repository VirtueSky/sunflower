using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Core;
#if UNITY_EDITOR
#endif

namespace VirtueSky.ObjectPooling
{
    internal sealed class PoolHandle
    {
        private Dictionary<GameObject, Queue<GameObject>> waitPool;
        private LinkedList<GameObject> activePool;
        private Transform container;
        private bool initialized;

        internal void Initialize()
        {
            if (initialized) return;
            initialized = true;

            waitPool = new Dictionary<GameObject, Queue<GameObject>>();
            activePool = new LinkedList<GameObject>();
            container = new GameObject("PoolContainer").transform;
            UnityEngine.Object.DontDestroyOnLoad(container.gameObject);
        }

        internal void PreSpawn(PoolData poolData)
        {
            for (var i = 0; i < poolData.count; i++)
            {
                SpawnNew(poolData.prefab);
            }
        }

        private void SpawnNew(GameObject prefab)
        {
            var gameObject = UnityEngine.Object.Instantiate(prefab);
            var id = gameObject.AddComponent<PooledObjectId>();
            id.prefab = prefab;

            activePool.AddLast(gameObject);

            DeSpawn(gameObject, false);
        }

        internal void DeSpawn<T>(T type, bool destroy = false, bool worldPositionStays = true) where T : Component
        {
            DeSpawn(type.gameObject, destroy, worldPositionStays);
        }

        internal void DeSpawn(GameObject gameObject, bool destroy = false, bool worldPositionStays = true)
        {
            var id = gameObject.GetComponent<PooledObjectId>();
            if (id == null)
            {
                Debug.LogError($"{gameObject.name} is not a pooled object!");
                return;
            }

            if (!activePool.Contains(gameObject))
            {
                Debug.LogError($"{gameObject.name} is not in active pool!");
                return;
            }

            activePool.Remove(gameObject);
            if (!waitPool.ContainsKey(id.prefab))
            {
                waitPool.Add(id.prefab, new Queue<GameObject>());
            }

            var stack = waitPool[id.prefab];
            if (stack.Contains(gameObject))
            {
                Debug.LogError($"{gameObject.name} is already pooled!");
                return;
            }

            CleanUp(gameObject);
            if (destroy)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                gameObject.transform.SetParent(container, worldPositionStays);
                stack.Enqueue(gameObject);
            }
        }

        internal void DeSpawnAll()
        {
            var arr = activePool.ToArray();
            foreach (var o in arr)
            {
                if (o != null) DeSpawn(o);
            }
        }

        internal void DestroyAllWaitPools()
        {
            foreach (var (key, queue) in waitPool)
            {
                foreach (var go in queue)
                {
                    CleanUp(go);
                    UnityEngine.Object.DestroyImmediate(go);
                }

                queue.Clear();
            }

            waitPool.Clear();
        }

        internal void DestroyAll()
        {
            var arr = waitPool.Values.SelectMany(g => g).ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                UnityEngine.Object.Destroy(arr[i].gameObject);
            }

            waitPool.Clear();
        }

        internal T Spawn<T>(T type, Transform parent = null, bool worldPositionStays = true, bool initialize = true)
            where T : Component
        {
            return Spawn(type.gameObject, parent, worldPositionStays, initialize).GetComponent<T>();
        }

        internal GameObject Spawn(GameObject prefab, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true)
        {
            if (!waitPool.ContainsKey(prefab))
            {
                waitPool.Add(prefab, new Queue<GameObject>());
            }

            var stack = waitPool[prefab];
            if (stack.Count == 0)
            {
                SpawnNew(prefab);
            }

            var gameObject = stack.Dequeue();

            gameObject.transform.SetParent(parent, worldPositionStays);

            if (parent == null)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            }

            gameObject.SetActive(true);

            if (initialize)
            {
                InitializeObj(gameObject);
            }

            activePool.AddLast(gameObject);

            return gameObject;
        }

        internal T Spawn<T>(T type, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool initialize = true)
            where T : Component
        {
            return Spawn(type.gameObject, position, rotation, parent, worldPositionStays, initialize).GetComponent<T>();
        }

        internal GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true,
            bool initialize = true)
        {
            if (!waitPool.ContainsKey(prefab))
            {
                waitPool.Add(prefab, new Queue<GameObject>());
            }

            var stack = waitPool[prefab];
            if (stack.Count == 0)
            {
                SpawnNew(prefab);
            }

            var gameObject = stack.Dequeue();

            gameObject.transform.SetParent(parent, worldPositionStays);
            gameObject.transform.SetPositionAndRotation(position, rotation);

            if (parent == null)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            }

            gameObject.SetActive(true);

            if (initialize)
            {
                InitializeObj(gameObject);
            }

            activePool.AddLast(gameObject);

            return gameObject;
        }

        void InitializeObj(GameObject go)
        {
            var monos = go.GetComponentsInChildren<BaseMono>(true);
            foreach (var mono in monos)
            {
                mono.Initialize();
            }
        }

        void CleanUp(GameObject go)
        {
            var monos = go.GetComponentsInChildren<BaseMono>(true);
            foreach (var mono in monos)
            {
                mono.CleanUp();
            }
        }
    }
}