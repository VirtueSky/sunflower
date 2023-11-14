#if UNITY_EDITOR
#endif
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VirtueSky.UtilsEditor
{
#if UNITY_EDITOR
    public static class TextureUtils
    {
        public static void CaptureToFile(Camera cam, string path, int width = 1024, int height = 1024, int depth = 24,
            RenderTextureFormat format = RenderTextureFormat.ARGB32, TextureExtension extension = TextureExtension.PNG, bool refresh = true)
        {
            var rt = new RenderTexture(width, height, depth, format);
            cam.targetTexture = rt;
            cam.Render();

            RenderTextureToFile(rt, path, extension, refresh);
            cam.targetTexture = null;
            Object.Destroy(rt);
        }

        public static void RenderTextureToFile(RenderTexture rt, string path, TextureExtension extension = TextureExtension.PNG, bool refresh = true)
        {
            var oldRt = RenderTexture.active;

            var tex = new Texture2D(rt.width, rt.height);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            Texture2DToFile(tex, path, extension);

            RenderTexture.active = oldRt;
        }

        public static void Texture2DToFile(Texture2D tex, string path, TextureExtension extension = TextureExtension.PNG, bool refresh = true)
        {
            switch (extension)
            {
                case TextureExtension.PNG:
                    File.WriteAllBytes(path + ".png", tex.EncodeToPNG());
                    break;
                case TextureExtension.JPG:
                    File.WriteAllBytes(path + ".jpg", tex.EncodeToJPG());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(extension), extension, null);
            }


            if (refresh)
            {
                AssetDatabase.Refresh();
            }
        }

        public enum TextureExtension
        {
            PNG,
            JPG
        }
    }
#endif
}