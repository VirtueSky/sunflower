using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class FollowTargetComponent : BaseMono
    {
        [SerializeField] private DirectionFollowTarget directionFollowTarget;
        [Space, SerializeField] private Transform target;
        [ReadOnly, SerializeField] private Vector3 offset;
        [Space, SerializeField] private TypeFollowTarget typeFollowTarget;

        [Tooltip("Value used to interpolate between target and this object"),
         ShowIf(nameof(typeFollowTarget), TypeFollowTarget.Lerp), SerializeField]
        private float interpolateValue = 0.05f;

        [Tooltip("The current velocity, this value is modified by the function every time you call it."),
         ShowIf(nameof(typeFollowTarget), TypeFollowTarget.SmoothDamp), SerializeField]
        private Vector3 currentVelocity = Vector3.zero;

        [Tooltip(
             "Approximately the time it will take to reach the target. A smaller value will reach the target faster."),
         ShowIf(nameof(typeFollowTarget), TypeFollowTarget.SmoothDamp), SerializeField]
        private float smoothTime = 0.05f;

        [Tooltip("Optionally allows you to clamp the maximum speed."),
         ShowIf(nameof(typeFollowTarget), TypeFollowTarget.SmoothDamp), SerializeField]
        private float maxSpeed = Mathf.Infinity;


        private void Awake()
        {
            offset = transform.position - target.position;
        }

        public void SetTarget(Transform t)
        {
            target = t;
        }

        public void SetDirectionFollowTarget(DirectionFollowTarget d)
        {
            directionFollowTarget = d;
        }

        public void SetTypeFollowTarget(TypeFollowTarget t)
        {
            typeFollowTarget = t;
        }

        public override void LateTick()
        {
            base.LateTick();
            switch (typeFollowTarget)
            {
                case TypeFollowTarget.SetPosition:
                    HandleSetPos();
                    break;
                case TypeFollowTarget.Lerp:
                    HandleLerp();
                    break;
                case TypeFollowTarget.SmoothDamp:
                    HandleSmoothDamp();
                    break;
            }
        }

        private void HandleSetPos()
        {
            Vector3 targetPos = target.position + offset;
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    transform.SetPosition(targetPos);
                    break;
                case DirectionFollowTarget.XY:
                    transform.SetPositionXY(targetPos);
                    break;
                case DirectionFollowTarget.XZ:
                    transform.SetPositionXZ(targetPos);
                    break;
                case DirectionFollowTarget.YZ:
                    transform.SetPositionYZ(targetPos);
                    break;
                case DirectionFollowTarget.X:
                    transform.SetPositionX(targetPos.x);
                    break;
                case DirectionFollowTarget.Y:
                    transform.SetPositionY(targetPos.y);
                    break;
                case DirectionFollowTarget.Z:
                    transform.SetPositionZ(targetPos.y);
                    break;
            }
        }

        private void HandleLerp()
        {
            Vector3 interpolateVector3 = Vector3.Lerp(transform.position, target.position + offset,
                interpolateValue);
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    transform.SetPosition(interpolateVector3);
                    break;
                case DirectionFollowTarget.XY:
                    transform.SetPositionXY(interpolateVector3);
                    break;
                case DirectionFollowTarget.XZ:
                    transform.SetPositionXZ(interpolateVector3);
                    break;
                case DirectionFollowTarget.YZ:
                    transform.SetPositionYZ(interpolateVector3);
                    break;
                case DirectionFollowTarget.X:
                    transform.SetPositionX(interpolateVector3.x);
                    break;
                case DirectionFollowTarget.Y:
                    transform.SetPositionY(interpolateVector3.y);
                    break;
                case DirectionFollowTarget.Z:
                    transform.SetPositionZ(interpolateVector3.z);
                    break;
            }
        }

        private void HandleSmoothDamp()
        {
            Vector3 smoothDampVector3 = Vector3.SmoothDamp(transform.position, target.position + offset,
                ref currentVelocity, smoothTime, maxSpeed);
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    transform.SetPosition(smoothDampVector3);
                    break;
                case DirectionFollowTarget.XY:
                    transform.SetPositionXY(smoothDampVector3);
                    break;
                case DirectionFollowTarget.XZ:
                    transform.SetPositionXZ(smoothDampVector3);
                    break;
                case DirectionFollowTarget.YZ:
                    transform.SetPositionYZ(smoothDampVector3);
                    break;
                case DirectionFollowTarget.X:
                    transform.SetPositionX(smoothDampVector3.x);
                    break;
                case DirectionFollowTarget.Y:
                    transform.SetPositionY(smoothDampVector3.y);
                    break;
                case DirectionFollowTarget.Z:
                    transform.SetPositionZ(smoothDampVector3.z);
                    break;
            }
        }
    }

    public enum DirectionFollowTarget
    {
        XYZ,
        XY,
        XZ,
        YZ,
        X,
        Y,
        Z
    }

    public enum TypeFollowTarget
    {
        SetPosition,
        Lerp,
        SmoothDamp
    }
}