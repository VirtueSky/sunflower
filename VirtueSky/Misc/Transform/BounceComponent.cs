using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Misc
{
    public class BounceComponent : BaseMono
    {
        [Header("Attributes")] public bool isRotate = false;

        public float degreesPerSecond = 15.0f;
        public float amplitude = 0.5f;
        public float frequency = 1f;

        private Vector3 _posOffset;
        private Vector3 _tempPos;

        void Start()
        {
            _posOffset = transform.position;
        }

        public override void FixedTick()
        {
            base.FixedTick();
            if (isRotate)
            {
                transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
            }

            _tempPos = _posOffset;
            _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = _tempPos;
        }
    }
}