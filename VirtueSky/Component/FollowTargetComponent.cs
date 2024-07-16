using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class FollowTargetComponent : BaseMono
    {
        [Tooltip("if currentTrans is null, then currentTrans will be set up with the current game object"),
         SerializeField]
        private Transform currentTrans;

        [Space, SerializeField] private Transform targetTrans;
        [ReadOnly, SerializeField] private Vector3 offsetTrans;
        [Space, SerializeField] private DirectionFollowTarget directionFollowTarget;
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
            if (currentTrans == null)
            {
                currentTrans = gameObject.transform;
            }

            offsetTrans = currentTrans.position - targetTrans.position;
        }

        public void SetTarget(Transform t)
        {
            targetTrans = t;
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
            Vector3 targetPos = targetTrans.position + offsetTrans;
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    currentTrans.SetPosition(targetPos);
                    break;
                case DirectionFollowTarget.XY:
                    currentTrans.SetPositionXY(targetPos);
                    break;
                case DirectionFollowTarget.XZ:
                    currentTrans.SetPositionXZ(targetPos);
                    break;
                case DirectionFollowTarget.YZ:
                    currentTrans.SetPositionYZ(targetPos);
                    break;
                case DirectionFollowTarget.X:
                    currentTrans.SetPositionX(targetPos.x);
                    break;
                case DirectionFollowTarget.Y:
                    currentTrans.SetPositionY(targetPos.y);
                    break;
                case DirectionFollowTarget.Z:
                    currentTrans.SetPositionZ(targetPos.y);
                    break;
            }
        }

        private void HandleLerp()
        {
            Vector3 interpolateVector3 = Vector3.Lerp(currentTrans.position, targetTrans.position + offsetTrans,
                interpolateValue);
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    currentTrans.SetPosition(interpolateVector3);
                    break;
                case DirectionFollowTarget.XY:
                    currentTrans.SetPositionXY(interpolateVector3);
                    break;
                case DirectionFollowTarget.XZ:
                    currentTrans.SetPositionXZ(interpolateVector3);
                    break;
                case DirectionFollowTarget.YZ:
                    currentTrans.SetPositionYZ(interpolateVector3);
                    break;
                case DirectionFollowTarget.X:
                    currentTrans.SetPositionX(interpolateVector3.x);
                    break;
                case DirectionFollowTarget.Y:
                    currentTrans.SetPositionY(interpolateVector3.y);
                    break;
                case DirectionFollowTarget.Z:
                    currentTrans.SetPositionZ(interpolateVector3.z);
                    break;
            }
        }

        private void HandleSmoothDamp()
        {
            Vector3 smoothDampVector3 = Vector3.SmoothDamp(currentTrans.position, targetTrans.position + offsetTrans,
                ref currentVelocity, smoothTime, maxSpeed);
            switch (directionFollowTarget)
            {
                case DirectionFollowTarget.XYZ:
                    currentTrans.SetPosition(smoothDampVector3);
                    break;
                case DirectionFollowTarget.XY:
                    currentTrans.SetPositionXY(smoothDampVector3);
                    break;
                case DirectionFollowTarget.XZ:
                    currentTrans.SetPositionXZ(smoothDampVector3);
                    break;
                case DirectionFollowTarget.YZ:
                    currentTrans.SetPositionYZ(smoothDampVector3);
                    break;
                case DirectionFollowTarget.X:
                    currentTrans.SetPositionX(smoothDampVector3.x);
                    break;
                case DirectionFollowTarget.Y:
                    currentTrans.SetPositionY(smoothDampVector3.y);
                    break;
                case DirectionFollowTarget.Z:
                    currentTrans.SetPositionZ(smoothDampVector3.z);
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