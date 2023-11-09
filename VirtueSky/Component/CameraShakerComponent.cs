using DG.Tweening;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(Camera))]
    public class CameraShakerComponent : BaseMono
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float durationPosition = .3f;
        [SerializeField] private float durationRotation = .3f;
        [SerializeField] private Vector3 positionStrength;
        [SerializeField] private Vector3 rotationStrength;

        public void CameraShake()
        {
            camera.DOComplete();
            camera.DOShakePosition(durationPosition, positionStrength);
            camera.DOShakeRotation(durationRotation, rotationStrength);
        }

        public void CameraShake(float _durationPosition, float _durationRotation, Vector3 _positionStrength,
            Vector3 _rotationStrength)
        {
            camera.DOComplete();
            camera.DOShakePosition(_durationPosition, _positionStrength);
            camera.DOShakeRotation(_durationRotation, _rotationStrength);
        }
#if UNITY_EDITOR
        private void Reset()
        {
            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }
        }
#endif
    }
}