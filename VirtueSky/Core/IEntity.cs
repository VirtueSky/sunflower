namespace VirtueSky.Core
{
    public interface IEntity
    {
        void BindVariable();
        void DoEnable();
        void Initialize();
        void EarlyTick();
        void Tick();
        void LateTick();
        void FixedTick();
        void CleanUp();
        void DoDisable();
        void DoDestroy();
        void UnbindVariable();
    }
}