using System.Collections;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Utils
{
    public class ScreenShooter : BaseMono
    {
#if UNITY_EDITOR
        public override void Initialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        public override void Tick()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                Shoot();
            }
        }

        [ContextMenu("Shoot")]
        public void Shoot()
        {
            StartCoroutine(IEShoot());
        }

        IEnumerator IEShoot()
        {
            var timeScale = Time.timeScale;
            Time.timeScale = 0;
            //
            // var cam = GetComponentInChildren<Camera>();
            // var camExtra = GetComponentInChildren<CameraExtra>();
            // camExtra.Setup(cam);

            var gameViewProfilesCount = GameViewUtils.GetViewListSize();
            for (var i = GameViewUtils.DefaultSizeCount; i < gameViewProfilesCount; i++)
            {
                var resolution = GameViewUtils.resolutions[i - GameViewUtils.DefaultSizeCount];

                GameViewUtils.SetSize(i);
                yield return null;
                // camExtra.Apply();
                yield return null;

                yield return new WaitForEndOfFrame();
                var tex = ScreenCapture.CaptureScreenshotAsTexture(1);
                TextureUtils.Texture2DToFile(tex, $"Assets/{TimeUtils.CurrentTicks}-{resolution.name}-{resolution.width}x{resolution.height}");
            }

            Time.timeScale = timeScale;
        }
#endif
    }
}