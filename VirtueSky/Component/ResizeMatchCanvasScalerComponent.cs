using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class ResizeMatchCanvasScalerComponent : CacheComponent<CanvasScaler>
    {
        [SerializeField] private bool isHorizontal;

        [ShowIf("isHorizontal"), SerializeField, Range(0, 2)]
        private float aspectRatioH = 16f / 9f;

        [ShowIf("isHorizontal", false), SerializeField, Range(0, 1)]
        private float aspectRatioV = 9f / 16f;

        [SerializeField] private Canvas canvas;
        [SerializeField, ReadOnly] private Camera camera;

        protected override void Awake()
        {
            base.Awake();
            GetCanvas();
            if (camera != null)
            {
                float aspectRatio = isHorizontal ? aspectRatioH : aspectRatioV;
                component.matchWidthOrHeight = camera.aspect > aspectRatio ? 1 : 0;
            }
            else
            {
                float screenAspect = isHorizontal ? (float)Screen.height / Screen.width : (float)Screen.width / Screen.height;
                float aspectRatio = isHorizontal ? aspectRatioH : aspectRatioV;
                component.matchWidthOrHeight = screenAspect > aspectRatio ? 1 : 0;
            }
        }

        void GetCanvas()
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
                if (canvas.worldCamera)
                {
                    camera = canvas.worldCamera;
                }
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