using System;
using PrimeTween;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static Transform ClearTransform(this Transform transform)
        {
            var childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject, true);
            }

            return transform;
        }

        public static void Shrug(this Transform transformObj, float time, float strength = .1f,
            Ease easingTypes = Ease.OutQuad,
            Action completed = null)
        {
            Tween tween = default;
            Vector3 baseScale = transformObj.localScale;
            Vector3 targetBounceX = new Vector3(1 + strength, 1 - strength) * baseScale.x;
            Vector3 targetBounceY = new Vector3(1 - strength, 1 + strength) * baseScale.y;
            tween = Tween.Scale(transformObj, targetBounceX, time / 3, easingTypes).OnComplete(() =>
            {
                Tween.Scale(transformObj, targetBounceY, time / 3, easingTypes).OnComplete(() =>
                {
                    Tween.Scale(transformObj, baseScale, time / 3, easingTypes).OnComplete(() =>
                    {
                        tween.Stop();
                        completed?.Invoke();
                    });
                });
            });
        }

        public static Camera CameraShake(this Camera camera, float strengthFactor = 1.0f, float duration = 0.5f,
            int frequency = 10)
        {
            Tween.ShakeCamera(camera, strengthFactor, duration, frequency);
            return camera;
        }

        /// <summary>
        /// Convert UI positon to world position
        /// </summary>
        /// <param name="transform">transform is transform in canvas space (RectTransfom</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 ToWorldPosition(this RectTransform transform, Camera camera = null)
        {
            var cam = camera;
            if (cam == null) cam = Camera.main;
            if (cam == null) return Vector2.zero;
            var pos = cam.ViewportToWorldPoint(transform.position);
            var worldPosition = cam.WorldToViewportPoint(pos);
            return worldPosition;
        }

        #region Position X Y Z

        public static void SetPositionX(this Transform transform, float x)
        {
            var v3 = transform.position;
            v3.x = x;
            transform.position = v3;
        }


        public static void SetPositionY(this Transform transform, float y)
        {
            var v3 = transform.position;
            v3.y = y;
            transform.position = v3;
        }


        public static void SetPositionZ(this Transform transform, float z)
        {
            var v3 = transform.position;
            v3.z = z;
            transform.position = v3;
        }

        #endregion


        #region Position XY

        public static void SetPositionXY(this Transform transform, in Vector2 v2)
        {
            transform.position = new Vector3(v2.x, v2.y, transform.position.z);
        }


        public static void SetPositionXY(this Transform transform, float x, float y)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }


        public static void SetPositionXY(this Transform transform, Transform target)
        {
            var v3 = target.position;
            v3.z = transform.position.z;
            transform.position = v3;
        }

        #endregion


        #region Position XZ

        public static void GetPositionXZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.position;
            v2.x = v3.x;
            v2.y = v3.z;
        }


        public static Vector2 GetPositionXZ(this Transform transform)
        {
            var v3 = transform.position;
            return new Vector2(v3.x, v3.z);
        }


        public static void SetPositionXZ(this Transform transform, in Vector2 v2)
        {
            transform.position = new Vector3(v2.x, transform.position.y, v2.y);
        }


        public static void SetPositionXZ(this Transform transform, float x, float z)
        {
            transform.position = new Vector3(x, transform.position.y, z);
        }


        public static void SetPositionXZ(this Transform transform, Transform target)
        {
            var v3 = target.position;
            v3.y = transform.position.y;
            transform.position = v3;
        }

        #endregion


        #region Position YZ

        public static void GetPositionYZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.position;
            v2.x = v3.y;
            v2.y = v3.z;
        }


        public static Vector2 GetPositionYZ(this Transform transform)
        {
            var v3 = transform.position;
            return new Vector2(v3.y, v3.z);
        }


        public static void SetPositionYZ(this Transform transform, in Vector2 v2)
        {
            transform.position = new Vector3(transform.position.x, v2.x, v2.y);
        }


        public static void SetPositionYZ(this Transform transform, float y, float z)
        {
            transform.position = new Vector3(transform.position.x, y, z);
        }


        public static void SetPositionYZ(this Transform transform, Transform target)
        {
            var v3 = target.position;
            v3.x = transform.position.x;
            transform.position = v3;
        }

        #endregion


        #region Relative Position X Y Z

        public static void SetRelativePositionX(this Transform transform, float x)
        {
            var v3 = transform.position;
            v3.x += x;
            transform.position = v3;
        }


        public static void SetRelativePositionY(this Transform transform, float y)
        {
            var v3 = transform.position;
            v3.y += y;
            transform.position = v3;
        }


        public static void SetRelativePositionZ(this Transform transform, float z)
        {
            var v3 = transform.position;
            v3.z += z;
            transform.position = v3;
        }

        #endregion


        #region Relative Position XY

        public static void SetRelativePositionXY(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.position;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this Transform transform, float x, float y)
        {
            var v3 = transform.position;
            v3.x += x;
            v3.y += y;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this Transform transform, Transform target)
        {
            var v3 = transform.position;
            var targetV3 = target.position;
            v3.x += targetV3.x;
            v3.y += targetV3.y;
            transform.position = v3;
        }

        #endregion


        #region Relative Position XZ

        public static void SetRelativePositionXZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.position;
            v3.x += v2.x;
            v3.z += v2.y;
            transform.position = v3;
        }


        public static void SetRelativePositionXZ(this Transform transform, float x, float z)
        {
            var v3 = transform.position;
            v3.x += x;
            v3.z += z;
            transform.position = v3;
        }


        public static void SetRelativePositionXZ(this Transform transform, Transform target)
        {
            var v3 = transform.position;
            var targetV3 = target.position;
            v3.x += targetV3.x;
            v3.z += targetV3.z;
            transform.position = v3;
        }

        #endregion


        #region Relative Position YZ

        public static void SetRelativePositionYZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.position;
            v3.y += v2.x;
            v3.z += v2.y;
            transform.position = v3;
        }


        public static void SetRelativePositionYZ(this Transform transform, float y, float z)
        {
            var v3 = transform.position;
            v3.y += y;
            v3.z += z;
            transform.position = v3;
        }


        public static void SetRelativePositionYZ(this Transform transform, Transform target)
        {
            var v3 = transform.position;
            var targetV3 = target.position;
            v3.y += targetV3.y;
            v3.z += targetV3.z;
            transform.position = v3;
        }

        #endregion


        #region Local Position X Y Z

        public static void SetLocalPositionX(this Transform transform, float x)
        {
            var v3 = transform.localPosition;
            v3.x = x;
            transform.localPosition = v3;
        }


        public static void SetLocalPositionY(this Transform transform, float y)
        {
            var v3 = transform.localPosition;
            v3.y = y;
            transform.localPosition = v3;
        }


        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            var v3 = transform.localPosition;
            v3.z = z;
            transform.localPosition = v3;
        }

        #endregion


        #region Local Position XY

        public static void SetLocalPositionXY(this Transform transform, in Vector2 v2)
        {
            transform.localPosition = new Vector3(v2.x, v2.y, transform.localPosition.z);
        }


        public static void SetLocalPositionXY(this Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }


        public static void SetLocalPositionXY(this Transform transform, Transform target)
        {
            var v3 = target.localPosition;
            v3.z = transform.localPosition.z;
            transform.localPosition = v3;
        }

        #endregion


        #region Local Position XZ

        public static void GetLocalPositionXZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localPosition;
            v2.x = v3.x;
            v2.y = v3.z;
        }


        public static Vector2 GetLocalPositionXZ(this Transform transform)
        {
            var v3 = transform.localPosition;
            return new Vector2(v3.x, v3.z);
        }


        public static void SetLocalPositionXZ(this Transform transform, in Vector2 v2)
        {
            transform.localPosition = new Vector3(v2.x, transform.localPosition.y, v2.y);
        }


        public static void SetLocalPositionXZ(this Transform transform, float x, float z)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        }


        public static void SetLocalPositionXZ(this Transform transform, Transform target)
        {
            var v3 = target.localPosition;
            v3.y = transform.localPosition.y;
            transform.localPosition = v3;
        }

        #endregion


        #region Local Position YZ

        public static void GetLocalPositionYZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localPosition;
            v2.x = v3.y;
            v2.y = v3.z;
        }


        public static Vector2 GetLocalPositionYZ(this Transform transform)
        {
            var v3 = transform.localPosition;
            return new Vector2(v3.y, v3.z);
        }


        public static void SetLocalPositionYZ(this Transform transform, in Vector2 v2)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, v2.x, v2.y);
        }


        public static void SetLocalPositionYZ(this Transform transform, float y, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, z);
        }


        public static void SetLocalPositionYZ(this Transform transform, Transform target)
        {
            var v3 = target.localPosition;
            v3.x = transform.localPosition.x;
            transform.localPosition = v3;
        }

        #endregion


        #region Relative Local Position X Y Z

        public static void SetRelativeLocalPositionX(this Transform transform, float x)
        {
            var v3 = transform.localPosition;
            v3.x += x;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionY(this Transform transform, float y)
        {
            var v3 = transform.localPosition;
            v3.y += y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionZ(this Transform transform, float z)
        {
            var v3 = transform.localPosition;
            v3.z += z;
            transform.localPosition = v3;
        }

        #endregion


        #region Relative Local Position XY

        public static void SetRelativeLocalPositionXY(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localPosition;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this Transform transform, float x, float y)
        {
            var v3 = transform.localPosition;
            v3.x += x;
            v3.y += y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this Transform transform, Transform target)
        {
            var v3 = transform.localPosition;
            var targetV3 = target.localPosition;
            v3.x += targetV3.x;
            v3.y += targetV3.y;
            transform.localPosition = v3;
        }

        #endregion


        #region Relative Local Position XZ

        public static void SetRelativeLocalPositionXZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localPosition;
            v3.x += v2.x;
            v3.z += v2.y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXZ(this Transform transform, float x, float z)
        {
            var v3 = transform.localPosition;
            v3.x += x;
            v3.z += z;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXZ(this Transform transform, Transform target)
        {
            var v3 = transform.localPosition;
            var targetV3 = target.localPosition;
            v3.x += targetV3.x;
            v3.z += targetV3.z;
            transform.localPosition = v3;
        }

        #endregion


        #region Relative Local Postion YZ

        public static void SetRelativeLocalPositionYZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localPosition;
            v3.y += v2.x;
            v3.z += v2.y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionYZ(this Transform transform, float y, float z)
        {
            var v3 = transform.localPosition;
            v3.y += y;
            v3.z += z;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionYZ(this Transform transform, Transform target)
        {
            var v3 = transform.localPosition;
            var targetV3 = target.localPosition;
            v3.y += targetV3.y;
            v3.z += targetV3.z;
            transform.localPosition = v3;
        }

        #endregion


        #region Scale X Y Z

        public static void SetScaleX(this Transform transform, float x)
        {
            var v3 = transform.localScale;
            v3.x = x;
            transform.localScale = v3;
        }


        public static void SetScaleY(this Transform transform, float y)
        {
            var v3 = transform.localScale;
            v3.y = y;
            transform.localScale = v3;
        }


        public static void SetScaleZ(this Transform transform, float z)
        {
            var v3 = transform.localScale;
            v3.z = z;
            transform.localScale = v3;
        }

        #endregion


        #region Scale XY

        public static void SetScaleXY(this Transform transform, in Vector2 v2)
        {
            transform.localScale = new Vector3(v2.x, v2.y, transform.localScale.z);
        }


        public static void SetScaleXY(this Transform transform, float x, float y)
        {
            transform.localScale = new Vector3(x, y, transform.localScale.z);
        }


        public static void SetScaleXY(this Transform transform, float value)
        {
            transform.localScale = new Vector3(value, value, transform.localScale.z);
        }


        public static void SetScaleXY(this Transform transform, Transform target)
        {
            var v3 = target.localScale;
            v3.z = transform.localScale.z;
            transform.localScale = v3;
        }

        #endregion


        #region Scale XZ

        public static void GetScaleXZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localScale;
            v2.x = v3.x;
            v2.y = v3.z;
        }


        public static Vector2 GetScaleXZ(this Transform transform)
        {
            var v3 = transform.localScale;
            return new Vector2(v3.x, v3.z);
        }


        public static void SetScaleXZ(this Transform transform, in Vector2 v2)
        {
            transform.localScale = new Vector3(v2.x, transform.localScale.y, v2.y);
        }


        public static void SetScaleXZ(this Transform transform, float x, float z)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, z);
        }


        public static void SetScaleXZ(this Transform transform, float value)
        {
            transform.localScale = new Vector3(value, transform.localScale.y, value);
        }


        public static void SetScaleXZ(this Transform transform, Transform target)
        {
            var v3 = target.localScale;
            v3.y = transform.localScale.y;
            transform.localScale = v3;
        }

        #endregion


        #region Scale YZ

        public static void GetScaleYZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localScale;
            v2.x = v3.y;
            v2.y = v3.z;
        }


        public static Vector2 GetScaleYZ(this Transform transform)
        {
            var v3 = transform.localScale;
            return new Vector2(v3.y, v3.z);
        }


        public static void SetScaleYZ(this Transform transform, in Vector2 v2)
        {
            transform.localScale = new Vector3(transform.localScale.x, v2.x, v2.y);
        }


        public static void SetScaleYZ(this Transform transform, float y, float z)
        {
            transform.localScale = new Vector3(transform.localScale.x, y, z);
        }


        public static void SetScaleYZ(this Transform transform, float value)
        {
            transform.localScale = new Vector3(transform.localScale.x, value, value);
        }


        public static void SetScaleYZ(this Transform transform, Transform target)
        {
            var v3 = target.localScale;
            v3.x = transform.localScale.x;
            transform.localScale = v3;
        }

        #endregion


        #region Scale

        public static void SetScale(this Transform transform, float value)
        {
            transform.localScale = new Vector3(value, value, value);
        }

        #endregion


        #region Relative Scale X Y Z

        public static void SetRelativeScaleX(this Transform transform, float x)
        {
            var v3 = transform.localScale;
            v3.x += x;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleY(this Transform transform, float y)
        {
            var v3 = transform.localScale;
            v3.y += y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleZ(this Transform transform, float z)
        {
            var v3 = transform.localScale;
            v3.z += z;
            transform.localScale = v3;
        }

        #endregion


        #region Relative Scale XY

        public static void SetRelativeScaleXY(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localScale;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this Transform transform, float x, float y)
        {
            var v3 = transform.localScale;
            v3.x += x;
            v3.y += y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this Transform transform, float value)
        {
            var v3 = transform.localScale;
            v3.x += value;
            v3.y += value;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this Transform transform, Transform target)
        {
            var v3 = transform.localScale;
            var targetV3 = target.localScale;
            v3.x += targetV3.x;
            v3.y += targetV3.y;
            transform.localScale = v3;
        }

        #endregion


        #region Relative Scale XZ

        public static void SetRelativeScaleXZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localScale;
            v3.x += v2.x;
            v3.z += v2.y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXZ(this Transform transform, float x, float z)
        {
            var v3 = transform.localScale;
            v3.x += x;
            v3.z += z;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXZ(this Transform transform, float value)
        {
            var v3 = transform.localScale;
            v3.x += value;
            v3.z += value;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXZ(this Transform transform, Transform target)
        {
            var v3 = transform.localScale;
            var targetV3 = target.localScale;
            v3.x += targetV3.x;
            v3.z += targetV3.z;
            transform.localScale = v3;
        }

        #endregion


        #region Relative Scale YZ

        public static void SetRelativeScaleYZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localScale;
            v3.y += v2.x;
            v3.z += v2.y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleYZ(this Transform transform, float y, float z)
        {
            var v3 = transform.localScale;
            v3.y += y;
            v3.z += z;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleYZ(this Transform transform, float value)
        {
            var v3 = transform.localScale;
            v3.y += value;
            v3.z += value;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleYZ(this Transform transform, Transform target)
        {
            var v3 = transform.localScale;
            var targetV3 = target.localScale;
            v3.y += targetV3.y;
            v3.z += targetV3.z;
            transform.localScale = v3;
        }

        #endregion


        #region Relative Scale

        public static void SetRelativeScale(this Transform transform, float value)
        {
            transform.localScale += new Vector3(value, value, value);
        }

        #endregion


        #region Rotation X Y Z

        public static void SetRotationX(this Transform transform, float x)
        {
            var v3 = transform.eulerAngles;
            v3.x = x;
            transform.eulerAngles = v3;
        }


        public static void SetRotationY(this Transform transform, float y)
        {
            var v3 = transform.eulerAngles;
            v3.y = y;
            transform.eulerAngles = v3;
        }


        public static void SetRotationZ(this Transform transform, float z)
        {
            var v3 = transform.eulerAngles;
            v3.z = z;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Rotation XY

        public static void SetRotationXY(this Transform transform, in Vector2 v2)
        {
            transform.eulerAngles = new Vector3(v2.x, v2.y, transform.eulerAngles.z);
        }


        public static void SetRotationXY(this Transform transform, float x, float y)
        {
            transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
        }


        public static void SetRotationXY(this Transform transform, Transform target)
        {
            var v3 = target.eulerAngles;
            v3.z = transform.eulerAngles.z;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Rotation XZ

        public static void GetRotationXZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.eulerAngles;
            v2.x = v3.x;
            v2.y = v3.z;
        }


        public static Vector2 GetRotationXZ(this Transform transform)
        {
            var v3 = transform.eulerAngles;
            return new Vector2(v3.x, v3.z);
        }


        public static void SetRotationXZ(this Transform transform, in Vector2 v2)
        {
            transform.eulerAngles = new Vector3(v2.x, transform.eulerAngles.y, v2.y);
        }


        public static void SetRotationXZ(this Transform transform, float x, float z)
        {
            transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, z);
        }


        public static void SetRotationXZ(this Transform transform, Transform target)
        {
            var v3 = target.eulerAngles;
            v3.y = transform.eulerAngles.y;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Rotation YZ

        public static void GetRotationYZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.eulerAngles;
            v2.x = v3.y;
            v2.y = v3.z;
        }


        public static Vector2 GetRotationYZ(this Transform transform)
        {
            var v3 = transform.eulerAngles;
            return new Vector2(v3.y, v3.z);
        }


        public static void SetRotationYZ(this Transform transform, in Vector2 v2)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, v2.x, v2.y);
        }


        public static void SetRotationYZ(this Transform transform, float y, float z)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, z);
        }


        public static void SetRotationYZ(this Transform transform, Transform target)
        {
            var v3 = target.eulerAngles;
            v3.x = transform.eulerAngles.x;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Relative Rotation X Y Z

        public static void SetRelativeRotationX(this Transform transform, float x)
        {
            var v3 = transform.eulerAngles;
            v3.x += x;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationY(this Transform transform, float y)
        {
            var v3 = transform.eulerAngles;
            v3.y += y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationZ(this Transform transform, float z)
        {
            var v3 = transform.eulerAngles;
            v3.z += z;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Relative Rotation XY

        public static void SetRelativeRotationXY(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.eulerAngles;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationXY(this Transform transform, float x, float y)
        {
            var v3 = transform.eulerAngles;
            v3.x += x;
            v3.y += y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationXY(this Transform transform, Transform target)
        {
            var v3 = transform.eulerAngles;
            var targetV3 = target.eulerAngles;
            v3.x += targetV3.x;
            v3.y += targetV3.y;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Relative Rotation XZ

        public static void SetRelativeRotationXZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.eulerAngles;
            v3.x += v2.x;
            v3.z += v2.y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationXZ(this Transform transform, float x, float z)
        {
            var v3 = transform.eulerAngles;
            v3.x += x;
            v3.z += z;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationXZ(this Transform transform, Transform target)
        {
            var v3 = transform.eulerAngles;
            var targetV3 = target.eulerAngles;
            v3.x += targetV3.x;
            v3.z += targetV3.z;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Relative Rotation YZ

        public static void SetRelativeRotationYZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.eulerAngles;
            v3.y += v2.x;
            v3.z += v2.y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationYZ(this Transform transform, float y, float z)
        {
            var v3 = transform.eulerAngles;
            v3.y += y;
            v3.z += z;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationYZ(this Transform transform, Transform target)
        {
            var v3 = transform.eulerAngles;
            var targetV3 = target.eulerAngles;
            v3.y += targetV3.y;
            v3.z += targetV3.z;
            transform.eulerAngles = v3;
        }

        #endregion


        #region Local Rotation X Y Z

        public static void SetLocalRotationX(this Transform transform, float x)
        {
            var v3 = transform.localEulerAngles;
            v3.x = x;
            transform.localEulerAngles = v3;
        }


        public static void SetLocalRotationY(this Transform transform, float y)
        {
            var v3 = transform.localEulerAngles;
            v3.y = y;
            transform.localEulerAngles = v3;
        }


        public static void SetLocalRotationZ(this Transform transform, float z)
        {
            var v3 = transform.localEulerAngles;
            v3.z = z;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Local Rotation XY

        public static void SetLocalRotationXY(this Transform transform, in Vector2 v2)
        {
            transform.localEulerAngles = new Vector3(v2.x, v2.y, transform.localEulerAngles.z);
        }


        public static void SetLocalRotationXY(this Transform transform, float x, float y)
        {
            transform.localEulerAngles = new Vector3(x, y, transform.localEulerAngles.z);
        }


        public static void SetLocalRotationXY(this Transform transform, Transform target)
        {
            var v3 = target.localEulerAngles;
            v3.z = transform.localEulerAngles.z;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Local Rotation XZ

        public static void GetLocalRotationXZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localEulerAngles;
            v2.x = v3.x;
            v2.y = v3.z;
        }


        public static Vector2 GetLocalRotationXZ(this Transform transform)
        {
            var v3 = transform.localEulerAngles;
            return new Vector2(v3.x, v3.z);
        }


        public static void SetLocalRotationXZ(this Transform transform, in Vector2 v2)
        {
            transform.localEulerAngles = new Vector3(v2.x, transform.localEulerAngles.y, v2.y);
        }


        public static void SetLocalRotationXZ(this Transform transform, float x, float z)
        {
            transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, z);
        }


        public static void SetLocalRotationXZ(this Transform transform, Transform target)
        {
            var v3 = target.localEulerAngles;
            v3.y = transform.localEulerAngles.y;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Local Rotation YZ

        public static void GetLocalRotationYZ(this Transform transform, out Vector2 v2)
        {
            var v3 = transform.localEulerAngles;
            v2.x = v3.y;
            v2.y = v3.z;
        }


        public static Vector2 GetLocalRotationYZ(this Transform transform)
        {
            var v3 = transform.localEulerAngles;
            return new Vector2(v3.y, v3.z);
        }


        public static void SetLocalRotationYZ(this Transform transform, in Vector2 v2)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, v2.x, v2.y);
        }


        public static void SetLocalRotationYZ(this Transform transform, float y, float z)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, z);
        }


        public static void SetLocalRotationYZ(this Transform transform, Transform target)
        {
            var v3 = target.localEulerAngles;
            v3.x = transform.localEulerAngles.x;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Relative Local Rotation X Y Z

        public static void SetRelativeLocalRotationX(this Transform transform, float x)
        {
            var v3 = transform.localEulerAngles;
            v3.x += x;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationY(this Transform transform, float y)
        {
            var v3 = transform.localEulerAngles;
            v3.y += y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationZ(this Transform transform, float z)
        {
            var v3 = transform.localEulerAngles;
            v3.z += z;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Relative Local Rotation XY

        public static void SetRelativeLocalRotationXY(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localEulerAngles;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationXY(this Transform transform, float x, float y)
        {
            var v3 = transform.localEulerAngles;
            v3.x += x;
            v3.y += y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationXY(this Transform transform, Transform target)
        {
            var v3 = transform.localEulerAngles;
            var targetV3 = target.localEulerAngles;
            v3.x += targetV3.x;
            v3.y += targetV3.y;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Relative Local Rotation XZ

        public static void SetRelativeLocalRotationXZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localEulerAngles;
            v3.x += v2.x;
            v3.z += v2.y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationXZ(this Transform transform, float x, float z)
        {
            var v3 = transform.localEulerAngles;
            v3.x += x;
            v3.z += z;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationXZ(this Transform transform, Transform target)
        {
            var v3 = transform.localEulerAngles;
            var targetV3 = target.localEulerAngles;
            v3.x += targetV3.x;
            v3.z += targetV3.z;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Relative Local Rotation YZ

        public static void SetRelativeLocalRotationYZ(this Transform transform, in Vector2 v2)
        {
            var v3 = transform.localEulerAngles;
            v3.y += v2.x;
            v3.z += v2.y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationYZ(this Transform transform, float y, float z)
        {
            var v3 = transform.localEulerAngles;
            v3.y += y;
            v3.z += z;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationYZ(this Transform transform, Transform target)
        {
            var v3 = transform.localEulerAngles;
            var targetV3 = target.localEulerAngles;
            v3.y += targetV3.y;
            v3.z += targetV3.z;
            transform.localEulerAngles = v3;
        }

        #endregion


        #region Anchored Position X Y Z

        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            var v2 = rectTransform.anchoredPosition;
            v2.x = x;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            var v2 = rectTransform.anchoredPosition;
            v2.y = y;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetAnchoredPositionZ(this RectTransform rectTransform, float z)
        {
            var v3 = rectTransform.anchoredPosition3D;
            v3.z = z;
            rectTransform.anchoredPosition3D = v3;
        }

        #endregion


        #region Relative Anchored Position X Y Z

        public static void SetRelativeAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            var v2 = rectTransform.anchoredPosition;
            v2.x += x;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            var v2 = rectTransform.anchoredPosition;
            v2.y += y;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionZ(this RectTransform rectTransform, float z)
        {
            var v3 = rectTransform.anchoredPosition3D;
            v3.z += z;
            rectTransform.anchoredPosition3D = v3;
        }

        #endregion
    }
}