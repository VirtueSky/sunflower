using UnityEditor;
using UnityEngine;
using VirtueSky.Attributes;


namespace VirtueSky.Core
{
    public class BaseMono : MonoBehaviour, IEntity
    {
        [Header("Base Mono")] [SerializeField, NamedId]
        string id;

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
            App.SubTick(this);
            App.SubLateTick(this);
            App.SubFixedTick(this);
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
            App.UnSubTick(this);
            App.UnSubLateTick(this);
            App.UnSubFixedTick(this);
        }
    }
}