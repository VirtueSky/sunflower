using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Core;
using VirtueSky.Utils;
#if UNITY_EDITOR
#endif

namespace VirtueSky.ObjectPooling
{
    [CreateAssetMenu(menuName = "ObjectPooling/Pools")]
    public class Pools : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] PoolData[] poolDatas;

        Dictionary<GameObject, Queue<GameObject>> waitPool;
        LinkedList<GameObject> activePool;

        Transform container;

        bool initialized;

        public void Initialize()
        {
            if (initialized) return;
            initialized = true;

            waitPool = new Dictionary<GameObject, Queue<GameObject>>();
            activePool = new LinkedList<GameObject>();
            container = new GameObject("PoolContainer").transform;
            DontDestroyOnLoad(container.gameObject);

            PreSpawn();
        }

        void PreSpawn()
        {
            foreach (var data in poolDatas)
            {
                for (var i = 0; i < data.preSpawn; i++)
                {
                    SpawnNew(data.prefab);
                }
            }
        }

        public void SpawnNew(GameObject prefab)
        {
            var gameObject = Instantiate(prefab);
            var id = gameObject.AddComponent<PooledObjectId>();
            id.prefab = prefab;

            activePool.AddLast(gameObject);

            Despawn(gameObject, false);
        }

        public void Despawn(GameObject gameObject, bool destroy = false)
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
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                gameObject.transform.parent = container;
                stack.Enqueue(gameObject);
            }
        }

        public void DespawnAll()
        {
            var arr = activePool.ToArray();
            foreach (var o in arr)
            {
                if (o != null) Despawn(o);
            }
        }

        public void DestroyAllWaitPools()
        {
            foreach (var (key, queue) in waitPool)
            {
                foreach (var go in queue)
                {
                    CleanUp(go);
                    DestroyImmediate(go);
                }

                queue.Clear();
            }

            waitPool.Clear();
        }

        public void DestroyAll()
        {
            var arr = waitPool.Values.SelectMany(g => g).ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                Destroy(arr[i].gameObject);
            }

            waitPool.Clear();
        }

        public T Spawn<T>(T type, Transform parent = null, bool initialize = true) where T : Component
        {
            return Spawn(type.gameObject, parent, initialize).GetComponent<T>();
        }

        public GameObject Spawn(GameObject prefab, Transform parent = null, bool initialize = true)
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

            gameObject.transform.parent = parent;

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

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            initialized = false;
        }
    }

    [Serializable]
    public class PoolData
    {
        public GameObject prefab;
        public int preSpawn;
    }
}