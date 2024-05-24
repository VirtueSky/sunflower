using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class BounceComponent : BaseMono
    {
        [Header("Attributes")] public bool isRotate = false;

        public float degreesPerSecond = 15.0f;
        public float amplitude = 5f;
        public float frequency = 1f;

        private Vector3 _posOffset;
        private Vector3 _tempPos;
        private bool isBounce = true;

        public override void OnEnable()
        {
            base.OnEnable();
            isBounce = true;
            _posOffset = transform.localPosition;
        }

        public void Pause()
        {
            isBounce = false;
        }

        public void Resume()
        {
            isBounce = true;
        }

        public override void FixedTick()
        {
            base.FixedTick();
            if (isBounce)
            {
                if (isRotate)
                {
                    transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
                }

                _tempPos = _posOffset;
                _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

                transform.localPosition = _tempPos;
            }
        }
    }
}