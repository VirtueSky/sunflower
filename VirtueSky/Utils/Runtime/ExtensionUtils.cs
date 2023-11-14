using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace VirtueSky.Utils
{
    public static class MonoBehaviorExtension
    {
        public static Coroutine Delay(this MonoBehaviour mono, float time, bool realTime, System.Action callback)
        {
            return mono.StartCoroutine(WaitForExtraTime(time, realTime, callback));
        }

        static IEnumerator WaitForExtraTime(float time, bool realTime, System.Action callback)
        {
            if (realTime)
            {
                yield return new WaitForSecondsRealtime(time);
            }
            else
            {
                yield return new WaitForSeconds(time);
            }

            callback?.Invoke();
        }

        public static Coroutine Delay(this MonoBehaviour mono, int frame, System.Action callback)
        {
            return mono.StartCoroutine(WaitForExtraFrame(frame, callback));
        }

        static IEnumerator WaitForExtraFrame(int frame, System.Action callback)
        {
            for (var i = 0; i < frame; i++)
            {
                yield return null;
            }

            callback?.Invoke();
        }

        public static Coroutine WaitUntil(this MonoBehaviour mono, Func<bool> condition, Action callback)
        {
            return mono.StartCoroutine(WaitUntil(condition, callback));
        }

        static IEnumerator WaitUntil(Func<bool> condition, Action callback)
        {
            yield return new WaitUntil(condition);
            callback?.Invoke();
        }

        public static T GetAndCacheComponent<T>(this MonoBehaviour mono, ref T cache) where T : Component
        {
            return cache ? cache : (cache = mono.GetComponent<T>());
        }

        public static T GetAndCacheComponentInChildren<T>(this MonoBehaviour mono, ref T cache, bool includeInactive = false)
        {
            return cache ??= mono.GetComponentInChildren<T>(includeInactive);
        }

        public static T[] GetAndCacheComponents<T>(this MonoBehaviour mono, ref T[] cache) where T : Component
        {
            return cache ??= mono.GetComponents<T>();
        }

        public static T[] GetAndCacheComponentsInChildren<T>(this MonoBehaviour mono, ref T[] cache, bool includeInactive = false)
        {
            return cache ??= mono.GetComponentsInChildren<T>(includeInactive);
        }

        public static T GetAndCacheComponentInParent<T>(this MonoBehaviour mono, ref T cache)
        {
            return cache ??= mono.GetComponentInParent<T>();
        }
    }

    public static class GameObjectExtension
    {
        static List<Component> m_ComponentCache = new List<Component>();

        public static Component GetComponentNoAlloc(this GameObject go, System.Type componentType)
        {
            go.GetComponents(componentType, m_ComponentCache);
            var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
            m_ComponentCache.Clear();
            return component;
        }

        public static T GetComponentNoAlloc<T>(this GameObject go) where T : Component
        {
            go.GetComponents(typeof(T), m_ComponentCache);
            var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
            m_ComponentCache.Clear();
            return component as T;
        }

        public static T GetAndCacheComponent<T>(this GameObject go, ref T cache)
        {
            return cache ??= go.GetComponent<T>();
        }

        public static T GetAndCacheComponentInChildren<T>(this GameObject go, bool includeInactive, ref T cache)
        {
            return cache ??= go.GetComponentInChildren<T>(includeInactive);
        }
    }

    public static class TransformExtensions
    {
        public static void RandomRotation(this Transform transform, bool onlyY = false)
        {
            transform.rotation = SimpleMath.RandomRotation(onlyY);
        }

        public static void RandomLocalRotation(this Transform transform, bool onlyY = false)
        {
            transform.localRotation = SimpleMath.RandomRotation(onlyY);
        }

        public static void Copy(this Transform transform, Transform other, bool position, bool rotation, bool scale, bool otherLossyScale)
        {
            if (position) transform.position = other.transform.position;
            if (rotation) transform.rotation = other.transform.rotation;
            if (scale)
            {
                transform.localScale = otherLossyScale ? other.transform.lossyScale : other.transform.localScale;
            }
        }

        public static void ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        // public static Tween DORotateQuaternionXZ(this Transform transform, Vector3 pos, float duration)
        // {
        //     var dir = pos - transform.position;
        //     dir.y = 0;
        //     return transform.DORotateQuaternion(Quaternion.LookRotation(dir), duration);
        // }

        public static Vector2 position2D(this Transform transform)
        {
            return transform.position;
        }

        public static Vector2 localPosition2D(this Transform transform)
        {
            return transform.localPosition;
        }

        public static Bounds GetBounds(this Transform transform)
        {
            var bound = new Bounds();
            var renderers = transform.GetComponentsInChildren<Renderer>().Where(r => !(r is ParticleSystemRenderer)).ToArray();

            for (var j = 0; j < renderers.Length; j++)
            {
                if (j == 0) bound = renderers[j].bounds;
                else bound.Encapsulate(renderers[j].bounds);
            }

            return bound;
        }

#if UNITY_EDITOR
        [MenuItem("CONTEXT/Transform/ExpandX")]
        public static void ExpandX(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.x).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "ExpandX");
                var min = tfs[0].position.x;
                var max = tfs[tfs.Length - 1].position.x;
                var step = (max - min) / (tfs.Length - 1);
                for (var i = 0; i < tfs.Length; i++)
                {
                    var pos = tfs[i].position;
                    pos.x = min + step * i;
                    tfs[i].position = pos;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }

        [MenuItem("CONTEXT/Transform/ExpandY")]
        public static void ExpandY(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.y).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "ExpandY");
                var min = tfs[0].position.y;
                var max = tfs[tfs.Length - 1].position.y;
                var step = (max - min) / (tfs.Length - 1);
                for (var i = 0; i < tfs.Length; i++)
                {
                    var pos = tfs[i].position;
                    pos.y = min + step * i;
                    tfs[i].position = pos;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }

        [MenuItem("CONTEXT/Transform/SetX")]
        public static void SetX(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.x).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "SetX");
                var startX = tfs[0].position.x;
                var prevBound = tfs[0].GetBounds();
                for (var i = 0; i < tfs.Length; i++)
                {
                    var dist = 0f;
                    if (i > 0)
                    {
                        var bounds = tfs[i].GetBounds();
                        dist = prevBound.extents.x + bounds.extents.x;
                        prevBound = bounds;
                    }

                    var pos = tfs[i].position;
                    pos.x = startX + dist;
                    tfs[i].position = pos;
                    startX = pos.x;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }

        [MenuItem("CONTEXT/Transform/SetY")]
        public static void SetY(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.y).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "SetY");
                var startY = tfs[0].position.y;
                var prevBound = tfs[0].GetBounds();
                for (var i = 0; i < tfs.Length; i++)
                {
                    var dist = 0f;
                    if (i > 0)
                    {
                        var bounds = tfs[i].GetBounds();
                        dist = prevBound.extents.y + bounds.extents.y;
                        prevBound = bounds;
                    }

                    var pos = tfs[i].position;
                    pos.y = startY + dist;
                    tfs[i].position = pos;
                    startY = pos.y;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }

        [MenuItem("CONTEXT/Transform/AlignX")]
        public static void AlignX(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.x).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "AlignX");
                var x = tfs[0].position.x;
                for (var i = 0; i < tfs.Length; i++)
                {
                    var pos = tfs[i].position;
                    pos.x = x;
                    tfs[i].position = pos;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }

        [MenuItem("CONTEXT/Transform/AlignY")]
        public static void AlignY(MenuCommand command)
        {
            var tfs = Selection.gameObjects.Select(g => g.transform).OrderBy(t => t.position.y).ToArray();
            if (tfs.Length > 2)
            {
                Undo.RecordObjects(tfs, "AlignY");
                var y = tfs[0].position.y;
                for (var i = 0; i < tfs.Length; i++)
                {
                    var pos = tfs[i].position;
                    pos.y = y;
                    tfs[i].position = pos;
                    EditorUtility.SetDirty(tfs[i]);
                }
            }
        }
#endif
    }

    public static class LayerMaskExtension
    {
        public static int ToGameObjectLayer(this LayerMask layerMask)
        {
            return (int)Mathf.Log(layerMask.value, 2);
        }
    }

    public static class NavmeshExtension
    {
        public static bool IsReachDestination(this NavMeshAgent agent, float threshold = 0.1f)
        {
            return !agent.pathPending && agent.remainingDistance < agent.stoppingDistance + threshold;
        }
    }

    public static class EnumerationExtensions
    {
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static T Add<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not append value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }

        public static T Remove<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not remove value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }
    }

    public static class MathExtension
    {
        public static System.Numerics.Vector3 ToSysVector3(this Vector3 v)
        {
            return new System.Numerics.Vector3(v.x, v.y, v.z);
        }

        public static System.Numerics.Quaternion ToSysQuaternion(this Quaternion v)
        {
            return new System.Numerics.Quaternion(v.x, v.y, v.z, v.w);
        }
    }

    public static class ParticleExtension
    {
        public static void SetSortingOrder(this ParticleSystem particle, int sortingOrder)
        {
            var ps = particle.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in ps)
            {
                var r = p.GetComponent<Renderer>();
                r.sortingOrder += sortingOrder;
            }
        }

        public static void SetStopAction(this ParticleSystem particle, ParticleSystemStopAction stopAction)
        {
            var main = particle.main;
            main.stopAction = stopAction;
        }
    }

    public static class FlipXExtension
    {
        public static void FlipX(this Transform target)
        {
            var pos = target.localPosition;
            pos.x = -pos.x;
            target.localPosition = pos;

            var rot = target.localRotation;
            rot *= Quaternion.Euler(0, 0, rot.y * 2);
            target.localRotation = rot;
        }

        public static void FlipX(this PolygonCollider2D target)
        {
            var points = target.points;
            for (var i = 0; i < points.Length; i++)
            {
                points[i].x = -points[i].x;
            }

            target.points = points;
        }

        public static void FlipX(this HingeJoint2D target)
        {
            var motor = target.motor;
            motor.motorSpeed = -motor.motorSpeed;
            target.motor = motor;
        }

        public static void FlipX(this BoxCollider2D target)
        {
            var offset = target.offset;
            offset.x = -offset.x;
            target.offset = offset;
        }

        public static void FlipX(this WheelJoint2D target)
        {
            var motor = target.motor;
            motor.motorSpeed = -motor.motorSpeed;
            target.motor = motor;
        }

#if UNITY_EDITOR
        [MenuItem("CONTEXT/PolygonCollider2D/FlipX")]
        static void PolygonCollider2DFlipX(MenuCommand command)
        {
            var target = (PolygonCollider2D)command.context;
            target.FlipX();

            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/Transform/FlipX")]
        static void TransformFlipX(MenuCommand command)
        {
            var target = (Transform)command.context;
            target.FlipX();

            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/HingeJoint2D/FlipX")]
        static void HingeJoint2DFlipX(MenuCommand command)
        {
            var target = (HingeJoint2D)command.context;
            target.FlipX();

            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/BoxCollider2D/FlipX")]
        static void BoxCollider2DFlipX(MenuCommand command)
        {
            var target = (BoxCollider2D)command.context;
            target.FlipX();

            EditorUtility.SetDirty(target);
        }

        [MenuItem("CONTEXT/WheelJoint2D/FlipX")]
        static void WheelJoint2DFlipX(MenuCommand command)
        {
            var target = (WheelJoint2D)command.context;
            target.FlipX();

            EditorUtility.SetDirty(target);
        }
#endif
    }

    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        UnityWebRequestAsyncOperation ao;
        Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation ao)
        {
            this.ao = ao;
            ao.completed += OnRequestCompleted;
        }

        public bool IsCompleted => ao.isDone;

        public void GetResult()
        {
        }

        public void OnCompleted(Action c)
        {
            continuation = c;
        }

        void OnRequestCompleted(AsyncOperation obj)
        {
            continuation();
        }
    }

    public static class UnityWebRequestExtensions
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }

    public static class DictionaryExtension
    {
        public static Dictionary<T, K> Clone<T, K>(this Dictionary<T, K> dict)
        {
            if (dict == null)
            {
                return null;
            }

            return dict.ToDictionary(i => i.Key, i => i.Value);
        }

        public static string ToString<T, K>(this Dictionary<T, K> dict)
        {
            if (dict == null)
            {
                return string.Empty;
            }

            return string.Join(", ", dict);
        }
    }
}