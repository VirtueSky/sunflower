using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.EditorUtils;
using VirtueSky.Global;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Core
{
    public class BaseMono : MonoBehaviour, IEntity
    {
        [Header("Base Mono")] [SerializeField, NamedId]
        string id;

        [FoldoutGroup("Validate Update")] [InfoBox("<color=green>Tick same with Update</color>")] [SerializeField]
        bool tick;

        [FoldoutGroup("Validate Update")]
        [InfoBox("<color=green>LateTick same with LateUpdate</color>")]
        [SerializeField]
        bool lateTick;

        [FoldoutGroup("Validate Update")]
        [InfoBox("<color=green>FixedTick same with FixedUpdate</color>")]
        [SerializeField]
        bool fixedTick;

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
            SubTick();
            DoEnable();
        }

        void OnDisable()
        {
            DoDisable();
            UnSubTick();
        }

        private void OnDestroy()
        {
            DoDestroy();
        }

        void SubTick()
        {
            if (tick) App.SubTick(this);
            if (lateTick) App.SubLateTick(this);
            if (fixedTick) App.SubFixedTick(this);
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

        void UnSubTick()
        {
            if (tick) App.UnSubTick(this);
            if (lateTick) App.UnSubLateTick(this);
            if (fixedTick) App.UnSubFixedTick(this);
        }
    }
}