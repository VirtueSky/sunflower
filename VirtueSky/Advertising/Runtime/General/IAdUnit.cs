namespace VirtueSky.Ads
{
    public interface IAdUnit
    {
        public void Init();
        public void Load();
        public bool IsReady();
        public void Destroy();
    }
}