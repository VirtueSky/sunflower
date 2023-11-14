using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VirtueSky.UtilsEditor
{
    public static class EditorExtend
    {
        public static Rect GetInnerGuiPosition(SceneView sceneView)
        {
            var position = sceneView.position;
            position.x = position.y = 0;
            position.height -= EditorStyles.toolbar.fixedHeight;
            return position;
        }

        public static bool Get2DMouseScenePosition(out Vector2 result)
        {
            result = Vector2.zero;

            var cam = Camera.current;
            var mp = Event.current.mousePosition;
            mp.y = cam.pixelHeight - mp.y;
            var ray = cam.ScreenPointToRay(mp);
            if (ray.direction != Vector3.forward) return false;

            result = ray.origin;
            return true;
        }

        /// <summary>
        /// Render an object on sceneView using sprite renderers
        /// </summary>
        public static void FakeRenderSprite(GameObject obs, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            var rends = obs.GetComponentsInChildren<SpriteRenderer>();
            foreach (var rend in rends)
            {
                var bounds = rend.bounds;
                var pos = rend.transform.position - obs.transform.position + position;
                DrawSprite(rend.sprite, pos, Vector3.Scale(bounds.size, scale));
            }
        }

        private static void DrawSprite(Sprite sprite, Vector3 worldSpace, Vector3 size)
        {
            if (sprite == null) return;
            Rect spriteTextureRect = LocalTextureRect(sprite);

            Handles.BeginGUI();

            Vector2 v0 = HandleUtility.WorldToGUIPoint(worldSpace - size / 2f);
            Vector2 v1 = HandleUtility.WorldToGUIPoint(worldSpace + size / 2f);
            Vector2 vMin = new Vector2(Mathf.Min(v0.x, v1.x), Mathf.Min(v0.y, v1.y));
            Vector2 vMax = new Vector2(Mathf.Max(v0.x, v1.x), Mathf.Max(v0.y, v1.y));
            Rect r = new Rect(vMin, vMax - vMin);
            GUI.DrawTextureWithTexCoords(r, sprite.texture, spriteTextureRect);

            Handles.EndGUI();
        }

        /// <summary>
        /// Calculate normalized texturerect of a sprite (0->1)
        /// </summary>
        private static Rect LocalTextureRect(Sprite sprite)
        {
            var texturePosition = sprite.textureRect.position;
            var textureSize = sprite.textureRect.size;
            texturePosition.x /= sprite.texture.width;
            texturePosition.y /= sprite.texture.height;
            textureSize.x /= sprite.texture.width;
            textureSize.y /= sprite.texture.height;
            return new Rect(texturePosition, textureSize);
        }

        public static void SkipEvent()
        {
            int id = GUIUtility.GetControlID(FocusType.Passive);
            GUIUtility.hotControl = id;
            HandleUtility.AddDefaultControl(id);
            Event.current.Use();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="bounds"></param>
        /// <param name="space"></param>
        /// <param name="renderers"></param>
        /// <param name="colliders"></param>
        /// <param name="meshes"></param>
        /// <param name="graphics"></param>
        /// <param name="particles"></param>
        /// <returns></returns>
        public static bool CalculateBounds(
            this GameObject root,
            out Bounds bounds,
            Space space,
            bool renderers = true,
            bool colliders = true,
            bool meshes = false,
            bool graphics = true,
            bool particles = false)
        {
            bounds = new Bounds();

            var first = true;

            if (space == Space.Self)
            {
                if (renderers)
                {
                    var results = new List<Renderer>();
                    root.GetComponentsInChildren(results);

                    foreach (var renderer in results)
                    {
                        if (!renderer.enabled)
                        {
                            continue;
                        }

                        if (!particles && renderer is ParticleSystemRenderer)
                        {
                            continue;
                        }

                        var rendererBounds = renderer.bounds;

                        rendererBounds.SetMinMax(root.transform.InverseTransformPoint(rendererBounds.min), root.transform.InverseTransformPoint(rendererBounds.max));

                        if (first)
                        {
                            bounds = rendererBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(rendererBounds);
                        }
                    }

                    results = null;
                }

                if (meshes)
                {
                    var meshFilters = new List<MeshFilter>();
                    root.GetComponentsInChildren(meshFilters);

                    foreach (var meshFilter in meshFilters)
                    {
                        var mesh = Application.isPlaying ? meshFilter.mesh : meshFilter.sharedMesh;

                        if (mesh == null)
                        {
                            continue;
                        }

                        var meshBounds = mesh.bounds;

                        meshBounds.SetMinMax(root.transform.InverseTransformPoint(meshFilter.transform.TransformPoint(meshBounds.min)),
                            root.transform.InverseTransformPoint(meshFilter.transform.TransformPoint(meshBounds.max)));

                        if (first)
                        {
                            bounds = meshBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(meshBounds);
                        }
                    }

                    meshFilters = null;
                }

                if (graphics)
                {
                    var results = new List<Graphic>();
                    root.GetComponentsInChildren(results);

                    foreach (var graphic in results)
                    {
                        if (!graphic.enabled)
                        {
                            continue;
                        }

                        var graphicCorners = new Vector3[4] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
                        graphic.rectTransform.GetLocalCorners(graphicCorners);
                        var graphicsBounds = BoundsFromCorners(graphicCorners);
                        graphicCorners = null;

                        if (first)
                        {
                            bounds = graphicsBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(graphicsBounds);
                        }
                    }

                    results = null;
                }

                if (colliders && first)
                {
                    var results = new List<Collider>();
                    root.GetComponentsInChildren(results);

                    foreach (var collider in results)
                    {
                        if (!collider.enabled)
                        {
                            continue;
                        }

                        var colliderBounds = collider.bounds;

                        colliderBounds.SetMinMax(root.transform.InverseTransformPoint(colliderBounds.min), root.transform.InverseTransformPoint(colliderBounds.max));

                        if (first)
                        {
                            bounds = colliderBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(colliderBounds);
                        }
                    }

                    results = null;
                }

                return !first;
            }
            else // if (space == Space.World)
            {
                if (renderers)
                {
                    var results = new List<Renderer>();
                    root.GetComponentsInChildren(results);

                    foreach (var renderer in results)
                    {
                        if (!renderer.enabled)
                        {
                            continue;
                        }

                        if (!particles && renderer is ParticleSystemRenderer)
                        {
                            continue;
                        }

                        if (first)
                        {
                            bounds = renderer.bounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(renderer.bounds);
                        }
                    }

                    results = null;
                }

                if (meshes)
                {
                    var filters = new List<MeshFilter>();
                    root.GetComponentsInChildren(filters);

                    foreach (var meshFilter in filters)
                    {
                        var mesh = (Application.isPlaying ? meshFilter.mesh : meshFilter.sharedMesh);

                        if (mesh == null)
                        {
                            continue;
                        }

                        var meshBounds = mesh.bounds;

                        meshBounds.SetMinMax(root.transform.TransformPoint(meshBounds.min), root.transform.TransformPoint(meshBounds.max));

                        if (first)
                        {
                            bounds = meshBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(meshBounds);
                        }
                    }

                    filters = null;
                }

                if (graphics)
                {
                    var results = new List<Graphic>();
                    root.GetComponentsInChildren(results);

                    foreach (var graphic in results)
                    {
                        if (!graphic.enabled)
                        {
                            continue;
                        }

                        var graphicCorners = new Vector3[4] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
                        graphic.rectTransform.GetWorldCorners(graphicCorners);
                        var graphicsBounds = BoundsFromCorners(graphicCorners);
                        graphicCorners = null;

                        if (first)
                        {
                            bounds = graphicsBounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(graphicsBounds);
                        }
                    }

                    results = null;
                }

                if (colliders && first)
                {
                    var results = new List<Collider>();
                    root.GetComponentsInChildren(results);

                    foreach (var collider in results)
                    {
                        if (!collider.enabled)
                        {
                            continue;
                        }

                        if (first)
                        {
                            bounds = collider.bounds;
                            first = false;
                        }
                        else
                        {
                            bounds.Encapsulate(collider.bounds);
                        }
                    }

                    results = null;
                }
            }

            return !first;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="corners"></param>
        /// <returns></returns>
        private static Bounds BoundsFromCorners(Vector3[] corners)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var minZ = float.MaxValue;

            var maxX = float.MinValue;
            var maxY = float.MinValue;
            var maxZ = float.MinValue;

            foreach (var corner in corners)
            {
                if (corner.x < minX)
                {
                    minX = corner.x;
                }

                if (corner.y < minY)
                {
                    minY = corner.y;
                }

                if (corner.z < minZ)
                {
                    minZ = corner.z;
                }

                if (corner.x > minX)
                {
                    maxX = corner.x;
                }

                if (corner.y > minY)
                {
                    maxY = corner.y;
                }

                if (corner.z > minZ)
                {
                    maxZ = corner.z;
                }
            }

            return new Bounds() { min = new Vector3(minX, minY, minZ), max = new Vector3(maxX, maxY, maxZ) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string source)
        {
            return new CamelCaseNamingStrategy().GetPropertyName(source, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSnackCase(this string source)
        {
            return new SnakeCaseNamingStrategy().GetPropertyName(source, false);
        }

        public static bool IsSerializable(Type type)
        {
            var isSerializable = false;
            isSerializable |= type.IsSerializable;
            isSerializable |= type.Namespace == "UnityEngine";
            isSerializable |= type.IsSubclassOf(typeof(MonoBehaviour));
            return isSerializable;
        }

        public static void DrawSerializationError(Type type, Rect position = default)
        {
            if (position == default)
            {
                EditorGUILayout.HelpBox($"{type} is not marked as Serializable," + "\n Add [System.Serializable] attribute.", MessageType.Error);
            }
            else
            {
                var icon = EditorGUIUtility.IconContent("Error").image;
                GUI.DrawTexture(position, icon, ScaleMode.ScaleToFit);
            }
        }

        /// <summary>
        /// check if given type is array or list
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollectionType(this Type type)
        {
            return type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static Type GetCorrectElementType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }
    }
}