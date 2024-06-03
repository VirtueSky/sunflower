using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class ResizeMatchCanvasScalerComponent : CacheComponent<CanvasScaler>
    {
        [SerializeField, Range(0, 1)] private float aspectRatio = 0.6f;
        [SerializeField] private Canvas canvas;
        [SerializeField, ReadOnly] private Camera camera;

        protected override void Awake()
        {
            base.Awake();
            GetCanvas();
            component.matchWidthOrHeight = camera.aspect > aspectRatio ? 1 : 0;
        }

        void GetCanvas()
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
                camera = canvas.worldCamera;
            }
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            GetCanvas();
        }
#endif
    }
}