using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;

namespace VirtueSky.Hierarchy.HComponent
{
    public class VerticesAndTrianglesCountComponent: BaseComponent
    {
        // PRIVATE
        private GUIStyle labelStyle;
        private Color verticesLabelColor;
        private Color trianglesLabelColor;
        private bool calculateTotalCount;
        private bool showTrianglesCount;
        private bool showVerticesCount;
        private HierarchySize labelSize;

        // CONSTRUCTOR
        public VerticesAndTrianglesCountComponent ()
        {
            labelStyle = new GUIStyle();            
            labelStyle.clipping = TextClipping.Clip;  
            labelStyle.alignment = TextAnchor.MiddleRight;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesShow                  , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesShowDuringPlayMode    , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesCalculateTotalCount   , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesShowTriangles         , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesShowVertices          , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesLabelSize             , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesVerticesLabelColor    , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.VerticesAndTrianglesTrianglesLabelColor   , settingsChanged);

            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.VerticesAndTrianglesShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.VerticesAndTrianglesShowDuringPlayMode);
            calculateTotalCount         = HierarchySettings.getInstance().get<bool>(HierarchySetting.VerticesAndTrianglesCalculateTotalCount);
            showTrianglesCount          = HierarchySettings.getInstance().get<bool>(HierarchySetting.VerticesAndTrianglesShowTriangles);
            showVerticesCount           = HierarchySettings.getInstance().get<bool>(HierarchySetting.VerticesAndTrianglesShowVertices);
            verticesLabelColor          = HierarchySettings.getInstance().getColor(HierarchySetting.VerticesAndTrianglesVerticesLabelColor);
            trianglesLabelColor         = HierarchySettings.getInstance().getColor(HierarchySetting.VerticesAndTrianglesTrianglesLabelColor);
            labelSize                   = (HierarchySize)HierarchySettings.getInstance().get<int>(HierarchySetting.VerticesAndTrianglesLabelSize);

            #if UNITY_2019_1_OR_NEWER
                labelStyle.fontSize = labelSize == HierarchySize.Big ? 7 : 6;
                rect.width = labelSize == HierarchySize.Big ? 24 : 22;
            #else
                labelStyle.fontSize = labelSize == QHierarchySize.Big ? 9 : 8;
                rect.width = labelSize == QHierarchySize.Big ? 33 : 25;
            #endif
        }   

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;
                #if UNITY_2019_1_OR_NEWER                
                    rect.y += labelSize == HierarchySize.Big ? 2 : 1;
                #endif
                return LayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {  
            int vertexCount = 0;
            int triangleCount = 0;

            MeshFilter[] meshFilterArray = calculateTotalCount ? gameObject.GetComponentsInChildren<MeshFilter>(true) : gameObject.GetComponents<MeshFilter>();
            for (int i = 0; i < meshFilterArray.Length; i++)
            {
                Mesh sharedMesh = meshFilterArray[i].sharedMesh;
                if (sharedMesh != null)
                {
                    if (showVerticesCount) vertexCount += sharedMesh.vertexCount;
                    if (showTrianglesCount) triangleCount += sharedMesh.triangles.Length;
                }
            }

            SkinnedMeshRenderer[] skinnedMeshRendererArray = calculateTotalCount ? gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true) : gameObject.GetComponents<SkinnedMeshRenderer>();
            for (int i = 0; i < skinnedMeshRendererArray.Length; i++)
            {
                Mesh sharedMesh = skinnedMeshRendererArray[i].sharedMesh;
                if (sharedMesh != null)
                {   
                    if (showVerticesCount) vertexCount += sharedMesh.vertexCount;
                    if (showTrianglesCount) triangleCount += sharedMesh.triangles.Length;
                }
            }

            triangleCount /= 3;

            if (vertexCount > 0 || triangleCount > 0)
            {
                if (showTrianglesCount && showVerticesCount) 
                {
                    rect.y -= 4;
                    labelStyle.normal.textColor = verticesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(vertexCount), labelStyle);

                    rect.y += 8;
                    labelStyle.normal.textColor = trianglesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(triangleCount), labelStyle);
                }
                else if (showVerticesCount)
                {
                    labelStyle.normal.textColor = verticesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(vertexCount), labelStyle);
                }
                else
                {
                    labelStyle.normal.textColor = trianglesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(triangleCount), labelStyle);
                }
            }
        }

        // PRIVATE
        private string getCountString(int count)
        {
            if (count < 1000) return count.ToString();
            else if (count < 1000000) 
            {
                if (count > 100000) return (count / 1000.0f).ToString("0") + "k";
                else return (count / 1000.0f).ToString("0.0") + "k";
            }
            else 
            {
                if (count > 10000000) return (count / 1000.0f).ToString("0") + "M";
                else return (count / 1000000.0f).ToString("0.0") + "M";
            }
        }
    }
}

