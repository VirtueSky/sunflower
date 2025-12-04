namespace VirtueSky.AssetFinder.Editor
{
    public interface IWindow
    {
        bool WillRepaint { get; set; }
        void Repaint();
    }
}
