using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.EditorUtils;
using VirtueSky.Global;
using VirtueSky.ObjectPooling;


namespace VirtueSky.Core
{
    public class BaseSO : ScriptableObject, IEntity
    {
        [Header("Base SO")] [SerializeField, NamedId]
        string id;

        [SerializeField] [TextArea(2, 5)] private string description;

        [FoldoutGroup("Validate Update")] [InfoBox("<color=green>Tick same with Update</color>")] [SerializeField]
        bool tick;

        [FoldoutGroup("Validate Update")] [InfoBox("<color=green>LateTick same with LateUpdate</color>")] [SerializeField]
        bool lateTick;

        [FoldoutGroup("Validate Update")] [InfoBox("<color=green>FixedTick same with FixedUpdate</color>")] [SerializeField]
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
            SubTick();
            DoEnable();
        }

        public void Disable()
        {
            DoDisable();
            UnSubTick();
        }

        public void DoEnable()
        {
        }

        void SubTick()
        {
            if (tick) App.SubTick(this);
            if (lateTick) App.SubLateTick(this);
            if (fixedTick) App.SubFixedTick(this);
        }

        public virtual void Initialize()
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

        void UnSubTick()
        {
            if (tick) App.UnSubTick(this);
            if (lateTick) App.UnSubLateTick(this);
            if (fixedTick) App.UnSubFixedTick(this);
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            GetPools();
        }

        [ContextMenu("GetPools")]
        void GetPools()
        {
            pools = ScriptableSetting.CreateAndGetScriptableAsset<Pools>("/Core");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}