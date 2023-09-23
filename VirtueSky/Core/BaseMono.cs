using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.EditorUtils;
using VirtueSky.Misc;
using VirtueSky.ObjectPooling;
using VirtueSky.Utils;

namespace VirtueSky.Core
{
    public class BaseMono : MonoBehaviour, IEntity
    {
        [Header("Base Mono")] [SerializeField, NamedId]
        string id;

        [FoldoutGroup("Ticker")] [SerializeField]
        public Ticker ticker;

        [FoldoutGroup("Ticker")] [SerializeField]
        bool earlyTick;

        [FoldoutGroup("Ticker")] [InfoBox("<color=green>Tick same with Update</color>")] [SerializeField]
        bool tick;

        [FoldoutGroup("Ticker")] [InfoBox("<color=green>LateTick same with LateUpdate</color>")] [SerializeField]
        bool lateTick;

        [FoldoutGroup("Ticker")] [InfoBox("<color=green>FixedTick same with FixedUpdate</color>")] [SerializeField]
        bool fixedTick;

        [FoldoutGroup("Pools")] [SerializeField]
        public Pools pools;

        public string Id => id;

#if UNITY_EDITOR
        [ContextMenu("ResetId")]
        public void ResetId()
        {
            id = NamedIdAttributeDrawer.ToSnakeCase(name);
            EditorUtility.SetDirty(this);
        }
#endif

        void OnEnable()
        {
            BindVariable();
            ListenEvents();
            SubTick();
            DoEnable();
        }

        void OnDisable()
        {
            DoDisable();
            UnsubTick();
            //   StopListenEvents();
            UnbindVariable();
        }

        private void OnDestroy()
        {
            DoDestroy();
        }

        public virtual void BindVariable()
        {
        }

        public virtual void ListenEvents()
        {
        }

        void SubTick()
        {
            if (earlyTick) ticker.SubEarlyTick(this);
            if (tick) ticker.SubTick(this);
            if (lateTick) ticker.SubLateTick(this);
            if (fixedTick) ticker.SubFixedTick(this);
        }

        public virtual void DoEnable()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void EarlyTick()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void LateTick()
        {
        }

        public virtual void FixedTick()
        {
        }

        public virtual void CleanUp()
        {
        }

        public virtual void DoDisable()
        {
        }

        public virtual void DoDestroy()
        {
        }

        void UnsubTick()
        {
            if (earlyTick) ticker.UnsubEarlyTick(this);
            if (tick) ticker.UnsubTick(this);
            if (lateTick) ticker.UnsubLateTick(this);
            if (fixedTick) ticker.UnsubFixedTick(this);
        }

        public virtual void StopListenEvents()
        {
        }

        public virtual void UnbindVariable()
        {
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            GetTickerAndPools();
        }

        [ContextMenu("GetTickerAndPools")]
        void GetTickerAndPools()
        {
            ticker = ScriptableSetting.CreateAndGetScriptableAsset<Ticker>();
            pools = ScriptableSetting.CreateAndGetScriptableAsset<Pools>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}