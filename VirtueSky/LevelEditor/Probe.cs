namespace VirtueSky.LevelEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public delegate void ProbeHitSelectHandler(bool add);

    public struct ProbeHit
    {
        public GameObject gameObject;

        public Vector3? point;

        public Vector3? normal;

        public float? distance;

        public string label;

        public ProbeHitSelectHandler selectHandler;

        public Action focusHandler;

        public Action lostFocusHandler;

        public Transform Transform
        {
            get => gameObject.transform;
            set => gameObject = value.gameObject;
        }

        public RectTransform RectTransform
        {
            get => gameObject.GetComponent<RectTransform>();
            set => gameObject = value.gameObject;
        }

        public GameObject groupGameObject;

        public double groupOrder;

        public ProbeHit(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.groupGameObject = gameObject;
            groupOrder = 0;
            point = default;
            normal = default;
            distance = default;
            label = default;
            selectHandler = default;
            focusHandler = default;
            lostFocusHandler = default;
        }

        public void Select(bool add)
        {
            if (selectHandler != null)
            {
                selectHandler(add);
            }
            else if (gameObject != null)
            {
                if (add)
                {
                    Selection.objects = Selection.objects.Append(gameObject).ToArray();
                }
                else
                {
                    Selection.activeGameObject = gameObject;
                }
            }
        }

        public void OnFocusEnter()
        {
            if (focusHandler != null)
            {
                focusHandler.Invoke();
            }
            else
            {
                Probe.Highlight(gameObject);
            }
        }

        public void OnFocusLeave()
        {
            if (lostFocusHandler != null)
            {
                lostFocusHandler.Invoke();
            }
            else
            {
                Probe.ClearHighlight();
            }
        }
    }

    public static class Probe
    {
        #region Object Picking

        private const int DEFAULT_LIMIT = 100;

        private static bool CanPickHandles =>
            E != null && (E.type == EventType.MouseMove || E.type == EventType.MouseDown ||
                          E.type == EventType.MouseUp || E.type == EventType.MouseDrag ||
                          E.type == EventType.MouseEnterWindow || E.type == EventType.MouseLeaveWindow);

        public static ProbeHit? Pick(ProbeFilter filter, SceneView sceneView, Vector2 guiPosition, out Vector3 point)
        {
            var results = new List<ProbeHit>();

            try
            {
                PickAllNonAlloc(results, filter, sceneView, guiPosition);

                foreach (var result in results)
                {
                    if (result.point.HasValue)
                    {
                        point = result.point.Value;
                        return result;
                    }
                }

                point = DefaultPoint(sceneView, guiPosition);
                return null;
            }
            finally
            {
                results = null;
            }
        }

        public static ProbeHit? Pick(ProbeFilter filter, SceneView sceneView, Vector2 guiPosition, out Vector3 point,
            out Vector3 normal)
        {
            var results = new List<ProbeHit>();

            try
            {
                PickAllNonAlloc(results, filter, sceneView, guiPosition);

                foreach (var result in results)
                {
                    if (result.point.HasValue && result.normal.HasValue)
                    {
                        point = result.point.Value;
                        normal = result.normal.Value;
                        return result;
                    }
                }

                point = DefaultPoint(sceneView, guiPosition);
                normal = Vector3.up;
                return null;
            }
            finally
            {
                results = null;
            }
        }


        public static ProbeHit[] PickAll(ProbeFilter filter, SceneView sceneView, Vector2 guiPosition,
            int limit = DEFAULT_LIMIT)
        {
            var results = new List<ProbeHit>();
            PickAllNonAlloc(results,
                filter,
                sceneView,
                guiPosition,
                limit);
            return results.ToArray();
        }

        private static void PickAllNonAlloc(List<ProbeHit> hits, ProbeFilter filter, SceneView sceneView,
            Vector2 guiPosition, int limit = DEFAULT_LIMIT)
        {
            var screenPosition = HandleUtility.GUIPointToScreenPixelCoordinate(guiPosition);
            var ray3D = HandleUtility.GUIPointToWorldRay(guiPosition);
            var worldPosition = sceneView.camera.ScreenToWorldPoint(screenPosition);
            var layerMask = Physics.DefaultRaycastLayers;

            var raycastHits = new RaycastHit[limit];
            var overlapHits = new Collider2D[limit];
            var handleHits = new HashSet<GameObject>();
            var ancestorHits = new HashSet<ProbeHit>();
            var gameObjectHits = new Dictionary<GameObject, ProbeHit>();

            try
            {
                // Raycast (3D)
                if (filter.Raycast)
                {
                    var raycastHitCount = Physics.RaycastNonAlloc(ray3D, raycastHits, Mathf.Infinity, layerMask);

                    for (var i = 0; i < raycastHitCount; i++)
                    {
                        var raycastHit = raycastHits[i];

#if UNITY_2019_2_OR_NEWER
                        if (SceneVisibilityManager.instance.IsHidden(raycastHit.transform.gameObject))
                        {
                            continue;
                        }
#endif

                        var gameObject = raycastHit.transform.gameObject;

                        if (!gameObjectHits.TryGetValue(gameObject, out var hit))
                        {
                            hit = new ProbeHit(gameObject);
                        }

                        hit.point = raycastHit.point;
                        hit.normal = raycastHit.normal;
                        hit.distance = raycastHit.distance;

                        gameObjectHits[gameObject] = hit;
                    }
                }

                // Overlap (2D)
                if (filter.Overlap)
                {
                    var overlapHitCount = Physics2D.OverlapPointNonAlloc(worldPosition, overlapHits, layerMask);

                    for (var i = 0; i < overlapHitCount; i++)
                    {
                        var overlapHit = overlapHits[i];

#if UNITY_2019_2_OR_NEWER
                        if (SceneVisibilityManager.instance.IsHidden(overlapHit.transform.gameObject))
                        {
                            continue;
                        }
#endif

                        var gameObject = overlapHit.transform.gameObject;

                        if (!gameObjectHits.TryGetValue(gameObject, out var hit))
                        {
                            hit = new ProbeHit(gameObject);
                        }

                        hit.distance = hit.distance ?? Vector3.Distance(overlapHit.transform.position, worldPosition);

                        gameObjectHits[gameObject] = hit;
                    }
                }

                // Handles (Editor Default)
                if (filter.Handles && CanPickHandles)
                {
                    PickAllHandlesNonAlloc(handleHits, guiPosition, limit);

                    foreach (var handleHit in handleHits)
                    {
                        var gameObject = handleHit;

                        if (!gameObjectHits.TryGetValue(gameObject, out var hit))
                        {
                            hit = new ProbeHit(gameObject);
                        }

                        hit.distance = hit.distance ?? Vector3.Distance(handleHit.transform.position, worldPosition);

                        gameObjectHits[gameObject] = hit;
                    }
                }

                // Ancestors
                foreach (var gameObjectHit in gameObjectHits)
                {
                    var gameObject = gameObjectHit.Key;
                    var hit = gameObjectHit.Value;

                    var parent = gameObject.transform.parent;

                    int depth = 0;

                    while (parent != null)
                    {
                        var parentGameObject = parent.gameObject;

                        var parentHit = new ProbeHit(parentGameObject);
                        parentHit.groupGameObject = gameObject;
                        parentHit.distance =
                            hit.distance ?? Vector3.Distance(parentHit.Transform.position, worldPosition);
                        parentHit.groupOrder = 1000 + depth;

                        ancestorHits.Add(parentHit);

                        parent = parent.parent;
                        depth++;
                    }
                }

                // Prepare final hits
                hits.Clear();

                // Add hits
                foreach (var gameObjectHit in gameObjectHits.Values)
                {
                    hits.Add(gameObjectHit);
                }

                foreach (var ancestorHit in ancestorHits)
                {
                    hits.Add(ancestorHit);
                }

                // Sort by distance
                hits.Sort(CompareHits);
            }
            finally
            {
                raycastHits = null;
                overlapHits = null;

                handleHits.Clear();
                ancestorHits.Clear();
                gameObjectHits.Clear();

                handleHits = null;
                ancestorHits = null;
                gameObjectHits = null;
            }
        }

        private static void PickAllHandlesNonAlloc(HashSet<GameObject> results, Vector2 position,
            int limit = DEFAULT_LIMIT)
        {
            if (!CanPickHandles)
            {
                // HandleUtility.PickGameObject is not supported in those contexts
                Debug.LogWarning($"Cannot pick game objects in the current event: {E?.ToString() ?? "null"}");
                return;
            }

            GameObject result;

            var count = 0;

            do
            {
                var ignored = results.ToArray();

                result = HandleUtility.PickGameObject(position, false, ignored);

                // Ignored doesn't seem very reliable. Sometimes, an item included
                // in ignored will still be returned. That's a sign we should stop.
                if (results.Contains(result))
                {
                    result = null;
                }

                if (result != null)
                {
                    results.Add(result);
                }
            } while (result != null && count++ < limit);
        }

        private static readonly Comparison<ProbeHit> CompareHits = CompareProbeHit;

        private static int CompareProbeHit(ProbeHit a, ProbeHit b)
        {
            var distanceA = a.distance ?? Mathf.Infinity;
            var distanceB = b.distance ?? Mathf.Infinity;
            return distanceA.CompareTo(distanceB);
        }

        private static Vector3 DefaultPoint(SceneView sceneView, Vector2 guiPosition)
        {
            var screenPosition = (Vector3)HandleUtility.GUIPointToScreenPixelCoordinate(guiPosition);
            screenPosition.z = sceneView.cameraDistance;
            return sceneView.camera.ScreenToWorldPoint(screenPosition);
        }

        #endregion

        #region Scene View Integration

        private static Event E => Event.current;

        private static Vector2? pressPosition;
        private static GameObject highlight;

        internal static void Highlight(GameObject selection)
        {
            highlight = selection;
            SceneView.RepaintAll();
        }

        internal static void ClearHighlight()
        {
            highlight = null;
            SceneView.RepaintAll();
        }

        #endregion
    }

    public struct ProbeFilter
    {
        public bool Raycast { get; set; }
        public bool Overlap { get; set; }
        public bool Handles { get; set; }

        public static ProbeFilter Default { get; } = new ProbeFilter { Raycast = true, Overlap = true, Handles = true };
    }
}