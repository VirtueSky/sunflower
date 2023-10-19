namespace VirtueSky.Core
{
    public interface IEntity
    {
        // void DoEnable();
        void Initialize();
        void Tick();
        void LateTick();
        void FixedTick();
        void CleanUp();
        // void DoDisable();

        //  void DoDestroy();
    }
}