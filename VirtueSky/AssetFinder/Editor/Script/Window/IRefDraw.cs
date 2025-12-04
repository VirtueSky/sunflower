using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal interface IRefDraw
    {
        IWindow window { get; }
        int ElementCount();
        bool DrawLayout();
        bool Draw(Rect rect);
    }
}
