using UnityEditor;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class RotateComponent : BaseMono
    {
        [Header("Attributes")] public bool ignoreTimeScale;

        public float speed = 1f;
        public bool rotateX;
        public bool rotateY;
        public bool rotateZ;
        public bool isReverse;
        private bool isRotate = true;

        public void Resume()
        {
            isRotate = true;
        }

        public void Pause()
        {
            isRotate = false;
        }

        public override void FixedTick()
        {
            base.FixedTick();
            if (isRotate)
            {
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
                        transform.RotateAround(transform.position, transform.forward,
                            Time.deltaTime * 90f * speed * -1);
                    }
                }
            }
        }
    }
}