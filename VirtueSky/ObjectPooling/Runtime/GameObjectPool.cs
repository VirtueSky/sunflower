using UnityEngine;
using VirtueSky.Inspector;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif

namespace VirtueSky.ObjectPooling
{
    [EditorIcon("scriptable_pool")]
    public class GameObjectPool : ScriptableObject
    {
        [SerializeField] private Pools pools;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int numberPreSpawn;
        public Pools Pools => pools;

        public void PreSpawn()
        {
            for (int i = 0; i < numberPreSpawn; i++)
            {
                pools.SpawnNew(prefab);
            }
        }

        public GameObject Spawn(Transform parent = null, bool initialize = true)
        {
            return pools.Spawn(prefab, parent, initialize);
        }

        public T Spawn<T>(Transform parent = null, bool initialize = true) where T : Component
        {
            return pools.Spawn(prefab, parent, initialize).GetComponent<T>();
        }

        public void DeSpawn(GameObject gameObject)
        {
            pools.DeSpawn(gameObject);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            pools = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.ObjectPooling.Pools>("/Pools");
        }
#endif
    }
}