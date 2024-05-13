using UnityEngine;
using VirtueSky.Inspector;


namespace VirtueSky.Core
{
    [EditorIcon("icon_csharp")]
    public class BaseMono : MonoBehaviour, IEntity
    {
        public virtual void OnEnable()
        {
            SubTick();
        }

        public virtual void OnDisable()
        {
            UnSubTick();
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

        void SubTick()
        {
            App.SubTick(this);
            App.SubLateTick(this);
            App.SubFixedTick(this);
        }

        void UnSubTick()
        {
            App.UnSubTick(this);
            App.UnSubLateTick(this);
            App.UnSubFixedTick(this);
        }
    }
}