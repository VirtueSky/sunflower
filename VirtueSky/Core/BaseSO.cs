using System.IO;
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
    public class BaseSO : ScriptableObject, IEntity
    {
        [Header("Base SO")] [SerializeField, NamedId]
        string id;

        [SerializeField] [TextArea(2, 5)] private string description;

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

        public void Enable()
        {
            BindVariable();
            SubTick();
            DoEnable();
        }

        public void Disable()
        {
            DoDisable();
            UnsubTick();
            UnbindVariable();
        }

        public virtual void BindVariable()
        {
        }

        public void DoEnable()
        {
        }

        void SubTick()
        {
            if (earlyTick) ticker.SubEarlyTick(this);
            if (tick) ticker.SubTick(this);
            if (lateTick) ticker.SubLateTick(this);
            if (fixedTick) ticker.SubFixedTick(this);
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

        public void DoDisable()
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
            ticker = ScriptableSetting.CreateAndGetScriptableAsset<Ticker>("/Core");
            pools = ScriptableSetting.CreateAndGetScriptableAsset<Pools>("/Core");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}