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
            App.SubTick(this);
            App.SubLateTick(this);
            App.SubFixedTick(this);
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
            App.UnSubTick(this);
            App.UnSubLateTick(this);
            App.UnSubFixedTick(this);
        }
    }
}