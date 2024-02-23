using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp")]
    public class RotateComponent : BaseMono
    {
        [Header("Attributes")] public bool ignoreTimeScale;

        public float speed = 1f;
        public bool rotateX;
        public bool rotateY;
        public bool rotateZ;
        public bool isReverse;

        public override void FixedTick()
        {
            base.FixedTick();
            var transformTemp = transform;
            if (rotateX)
            {
                if (!isReverse)
                {
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * 90f * speed);
                }
                else
                {
                    transform.RotateAround(transform.position, transform.right, Time.deltaTime * 90f * -speed);
                }
            }

            if (rotateY)
            {
                if (!isReverse)
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f * speed);
                }
                else
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f * -speed);
                }
            }

            if (rotateZ)
            {
                if (!isReverse)
                {
                    transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f * speed * 1);
                }
                else
                {
                    transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f * speed * -1);
                }
            }
        }
    }
}