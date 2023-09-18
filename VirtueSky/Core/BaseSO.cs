using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.Misc;
using VirtueSky.ObjectPooling;
using VirtueSky.Utils;

namespace VirtueSky.Core
{
    public class BaseSO : ScriptableObject, IEntity
    {
        [SerializeField, NamedId] string id;
        [Header("Base")] [SerializeField] public Pools pools;
        [SerializeField] public Ticker ticker;
        [SerializeField] bool earlyTick;
        [SerializeField] bool tick;
        [SerializeField] bool lateTick;
        [SerializeField] bool fixedTick;

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
            ListenEvents();
            SubTick();
            DoEnable();
        }

        public void Disable()
        {
            DoDisable();
            UnsubTick();
            //StopListenEvents();
            UnbindVariable();
        }

        public virtual void BindVariable()
        {
        }

        public virtual void ListenEvents()
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
            ticker = AssetUtils.FindAssetAtFolder<Ticker>(new string[] { "Assets" }).FirstOrDefault();
            if (ticker == null)
            {
                Common.CreateSettingAssets<Ticker>();
                ticker = AssetUtils.FindAssetAtFolder<Ticker>(new string[] { "Assets" }).FirstOrDefault();
            }

            pools = AssetUtils.FindAssetAtFolder<Pools>(new string[] { "Assets" }).FirstOrDefault();
            if (pools == null)
            {
                Common.CreateSettingAssets<Pools>();
                pools = AssetUtils.FindAssetAtFolder<Pools>(new string[] { "Assets" }).FirstOrDefault();
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }
}