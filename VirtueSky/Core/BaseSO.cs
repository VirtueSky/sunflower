using UnityEngine;


namespace VirtueSky.Core
{
    public class BaseSO : ScriptableObject, IEntity
    {
        [Space(10)] [SerializeField] [TextArea(2, 5)]
        private string description;

        public void Enable()
        {
            SubTick();
        }

        public void Disable()
        {
            UnSubTick();
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

        public virtual void Destroy()
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