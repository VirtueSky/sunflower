namespace VirtueSky.Core
{
    public interface IEntity
    {
        void Initialize();
        void Tick();
        void LateTick();
        void FixedTick();
        void CleanUp();
    }
}