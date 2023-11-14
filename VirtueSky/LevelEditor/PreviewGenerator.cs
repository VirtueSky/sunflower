using System;
using System.Linq;
using UnityEngine;
using VirtueSky.UtilsEditor;
using Object = UnityEngine.Object;

namespace VirtueSky.LevelEditor
{
    public class PreviewGenerator
    {
        public static readonly PreviewGenerator Default = new PreviewGenerator();

        private const int MAX_TEXTURE_SIZE = 2048;
        private static int latePreviewQueued;

        // ReSharper disable once MemberCanBePrivate.Global
        public Vector3 previewPosition = new Vector3(9999, 9999, -9999);

        // ReSharper disable once MemberCanBePrivate.Global
        public Vector3 latePreviewOffset = new Vector3(100, 100, 0);

        public bool transparentBackground = true;

        // ReSharper disable once MemberCanBePrivate.Global
        public Color solidBackgroundColor = new Color(0.3f, 0.3f, 0.3f);

        // ReSharper disable once MemberCanBePrivate.Global
        public FilterMode imageFilterMode = FilterMode.Point;
        public ImageSizeType sizingType = ImageSizeType.PixelsPerUnit;

        // ReSharper disable once MemberCanBePrivate.Global
        public int pixelPerUnit = 32;
        public int width = 256;
        public int height = 256;

        // ReSharper disable once MemberCanBePrivate.Global
        public float timingCounter = 1;

        // ReSharper disable once MemberCanBePrivate.Global
        public Action<Texture2D> onCapturedCallback;

        // ReSharper disable once MemberCanBePrivate.Global
        public Action<GameObject> onPreCaptureCallback;


        public enum ImageSizeType
        {
            PixelsPerUnit,
            Fit,
            Fill,
            Stretch,
        }

        public PreviewGenerator Copy()
        {
            return new PreviewGenerator()
            {
                previewPosition = previewPosition,
                latePreviewOffset = latePreviewOffset,
                transparentBackground = transparentBackground,
                solidBackgroundColor = solidBackgroundColor,
                imageFilterMode = imageFilterMode,
                sizingType = sizingType,
                pixelPerUnit = pixelPerUnit,
                width = width,
                height = height,
                timingCounter = timingCounter,
                onCapturedCallback = onCapturedCallback,
                onPreCaptureCallback = onPreCaptureCallback,
            };
        }

        public PreviewGenerator OnCaptured(Action<Texture2D> callback)
        {
            onCapturedCallback = callback;
            return this;
        }

        public PreviewGenerator OnPreCaptured(Action<GameObject> callback)
        {
            onPreCaptureCallback = callback;
            return this;
        }


        public Texture2D CreatePreview(GameObject obj, bool clone = true)
        {
            if (!CanCreatePreview(obj))
            {
                onCapturedCallback?.Invoke(null);
                return EditorResources.ScriptableFactory;
            }

            var cachedPosition = obj.transform.position;
            var prevObj = clone ? Object.Instantiate(obj, null) : obj;
            prevObj.transform.position = previewPosition + latePreviewQueued * latePreviewOffset;

            var bounds = GetBounds<Renderer>(prevObj, false);
            var size = GetImageSize(bounds);
            var cam = CreatePreviewCamera(bounds);
            var light = CreatePreviewLight(bounds);

            latePreviewQueued++;
            var dummy = new GameObject("Preview Dummy");

            void Callback()
            {
                latePreviewQueued--;
                if (clone)
                {
                    Object.DestroyImmediate(prevObj);
                }
                else
                {
                    prevObj.transform.position = cachedPosition;
                }

                Object.DestroyImmediate(cam.gameObject);
                Object.DestroyImmediate(light.gameObject);
                Object.DestroyImmediate(dummy.gameObject);
            }

            return WrappedCapture(prevObj,
                cam,
                size.x,
                size.y,
                Callback);
        }


        private void NotifyPreviewTaking(GameObject go, Action<IPreviewComponent> action)
        {
            var allComps = go.GetComponentsInChildren<Component>();
            foreach (var comp in allComps)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (comp is IPreviewComponent component) action?.Invoke(component);
            }
        }

        private bool CanCreatePreview(GameObject obj)
        {
            return obj != null && obj.GetComponentsInChildren<Renderer>().Any(r => r != null && r.enabled);
        }

        private Camera CreatePreviewCamera(Bounds bounds)
        {
            var camObj = new GameObject("Preview generator camera");
            var cam = camObj.AddComponent<Camera>();

            cam.transform.position = bounds.center + Vector3.back * (bounds.extents.z + 2);
            cam.nearClipPlane = 0.01f;
            cam.farClipPlane = bounds.size.z + 4;

            cam.orthographic = true;
            cam.orthographicSize = bounds.extents.y;
            cam.aspect = bounds.extents.x / bounds.extents.y;

            cam.clearFlags = CameraClearFlags.Color;
            cam.backgroundColor = solidBackgroundColor;
            if (transparentBackground) cam.backgroundColor *= 0;

            cam.enabled = false;

            return cam;
        }

        private Light CreatePreviewLight(Bounds bounds)
        {
            var lightObj = new GameObject("Preview generator light");
            var light = lightObj.AddComponent<Light>();

            light.type = LightType.Directional;
            light.transform.position = bounds.center + Vector3.back * (bounds.extents.z + 2);
            light.color = Color.white;
            light.intensity = 1;
            return light;
        }

        private Vector2Int GetImageSize(Bounds bounds)
        {
            var w = 1;
            var h = 1;

            if (sizingType == ImageSizeType.PixelsPerUnit)
            {
                w = Mathf.CeilToInt(bounds.size.x * pixelPerUnit);
                h = Mathf.CeilToInt(bounds.size.y * pixelPerUnit);
            }
            else if (sizingType == ImageSizeType.Stretch)
            {
                w = width;
                h = height;
            }
            else if (sizingType == ImageSizeType.Fit || sizingType == ImageSizeType.Fill)
            {
                float widthFactor = width / bounds.size.x;
                float heightFactor = height / bounds.size.y;
                float factor = sizingType == ImageSizeType.Fit ? Mathf.Min(widthFactor, heightFactor) : Mathf.Max(widthFactor, heightFactor);

                w = Mathf.CeilToInt(bounds.size.x * factor);
                h = Mathf.CeilToInt(bounds.size.y * factor);
            }

            if (w > MAX_TEXTURE_SIZE || h > MAX_TEXTURE_SIZE)
            {
                float downscaleWidthFactor = (float)MAX_TEXTURE_SIZE / w;
                float downscaleHeightFactor = (float)MAX_TEXTURE_SIZE / h;
                float downscaleFactor = Mathf.Min(downscaleWidthFactor, downscaleHeightFactor);

                w = Mathf.CeilToInt(w * downscaleFactor);
                h = Mathf.CeilToInt(h * downscaleFactor);
            }

            return new Vector2Int(w, h);
        }

        private Texture2D WrappedCapture(GameObject obj, Camera cam, int w, int h, Action callback)
        {
            onPreCaptureCallback?.Invoke(obj);

            NotifyPreviewTaking(obj, i => i.OnPreviewCapturing(this));
            var tex = DoCapture(cam, w, h);
            NotifyPreviewTaking(obj, i => i.OnPreviewCaptured(this));

            callback?.Invoke();
            onCapturedCallback?.Invoke(tex);

            return tex;
        }

        private Texture2D DoCapture(Camera cam, int w, int h)
        {
            var temp = RenderTexture.active;
            RenderTexture renderTex = null;
            try
            {
                renderTex = RenderTexture.GetTemporary(w, h, 16);
            }
            catch (Exception)
            {
                //
            }

            RenderTexture.active = renderTex;

            cam.enabled = true;
            cam.targetTexture = renderTex;
            cam.Render();
            cam.targetTexture = null;
            cam.enabled = false;

            if (w <= 0) w = 512;
            if (h <= 0) h = 512;
            var tex = new Texture2D(w, h, transparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false) { filterMode = imageFilterMode };
            tex.ReadPixels(new Rect(0, 0, w, h), 0, 0, false);
            tex.Apply(false, false);

            RenderTexture.active = temp;
            RenderTexture.ReleaseTemporary(renderTex);
            return tex;
        }

        /// <summary>
        /// Get bound of gameobject via collider or renderer
        /// </summary>
        /// <param name="go"></param>
        /// <param name="includeInactive"></param>
        /// <param name="getBounds"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Bounds GetBounds<T>(GameObject go, bool includeInactive = true, System.Func<T, Bounds> getBounds = null) where T : Component
        {
            if (getBounds == null)
                getBounds = (t) => (t as Collider)?.bounds ?? (t as Collider2D)?.bounds ?? (t as Renderer)?.bounds ?? default;
            var comps = go.GetComponentsInChildren<T>(includeInactive).Where(_ => !(_.gameObject.GetComponent<ParticleSystem>())).ToArray();

            Bounds bound = default;
            bool found = false;

            foreach (var comp in comps)
            {
                if (comp)
                {
                    if (!includeInactive)
                    {
                        if (!(comp as Collider)?.enabled ?? false) continue;
                        if (!(comp as Collider2D)?.enabled ?? false) continue;
                        if (!(comp as Renderer)?.enabled ?? false) continue;
                        if (!(comp as MonoBehaviour)?.enabled ?? false) continue;
                    }

                    if (!found || bound.size == Vector3.zero)
                    {
                        bound = getBounds(comp);
                        found = true;
                    }
                    else bound.Encapsulate(getBounds(comp));
                }
            }

            return bound;
        }
    }

    internal interface IPreviewComponent
    {
        void OnPreviewCapturing(PreviewGenerator preview);

        void OnPreviewCaptured(PreviewGenerator preview);
    }
}