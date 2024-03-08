using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class StaticComponent: BaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private StaticEditorFlags staticFlags;
        private GameObject[] gameObjects;
        private Texture2D staticButton;
        private Color32[] staticButtonColors;

        // CONSTRUCTOR
        public StaticComponent()
        {
            rect.width = 11;
            rect.height = 10;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.StaticShow                , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.StaticShowDuringPlayMode  , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalActiveColor     , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor   , settingsChanged);

            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.StaticShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.StaticShowDuringPlayMode);
            activeColor                 = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalActiveColor);
            inactiveColor               = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 13)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 13;
                rect.x = curRect.x;
                rect.y = curRect.y + 4;
                staticFlags = GameObjectUtility.GetStaticEditorFlags(gameObject);
                return LayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            if (staticButton == null)
            {
                staticButton = new Texture2D(11, 10, TextureFormat.ARGB32, false, true);
                staticButtonColors = new Color32[11 * 10];
            }

            #if UNITY_4_6 || UNITY_4_7
            drawQuad(39, 5, 4, ((staticFlags & StaticEditorFlags.LightmapStatic       ) > 0));
            drawQuad(33, 5, 4, ((staticFlags & StaticEditorFlags.BatchingStatic       ) > 0));
            #else   
            drawQuad(37, 3, 4, ((staticFlags & StaticEditorFlags.ContributeGI       ) > 0));
            drawQuad(33, 3, 4, ((staticFlags & StaticEditorFlags.BatchingStatic       ) > 0));
            drawQuad(41, 3, 4, ((staticFlags & StaticEditorFlags.ReflectionProbeStatic) > 0));
            #endif
            drawQuad( 0, 5, 2, ((staticFlags & StaticEditorFlags.OccludeeStatic       ) > 0));
            drawQuad( 6, 5, 2, ((staticFlags & StaticEditorFlags.OccluderStatic       ) > 0));
            drawQuad(88, 5, 2, ((staticFlags & StaticEditorFlags.NavigationStatic     ) > 0));
            drawQuad(94, 5, 2, ((staticFlags & StaticEditorFlags.OffMeshLinkGeneration) > 0));

            staticButton.SetPixels32(staticButtonColors);
            staticButton.Apply();
            GUI.DrawTexture(rect, staticButton);
        }

        public override void eventHandler(GameObject gameObject, ObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();

                int intStaticFlags = (int)staticFlags;
                gameObjects = Selection.Contains(gameObject) ? Selection.gameObjects : new GameObject[] { gameObject };

                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Nothing"                   ), intStaticFlags == 0, staticChangeHandler, 0);
                menu.AddItem(new GUIContent("Everything"                ), intStaticFlags == -1, staticChangeHandler, -1);
                menu.AddItem(new GUIContent("Lightmap Static"           ), (intStaticFlags & (int)StaticEditorFlags.ContributeGI) > 0, staticChangeHandler, (int)StaticEditorFlags.ContributeGI);
                menu.AddItem(new GUIContent("Occluder Static"           ), (intStaticFlags & (int)StaticEditorFlags.OccluderStatic) > 0, staticChangeHandler, (int)StaticEditorFlags.OccluderStatic);
                menu.AddItem(new GUIContent("Batching Static"           ), (intStaticFlags & (int)StaticEditorFlags.BatchingStatic) > 0, staticChangeHandler, (int)StaticEditorFlags.BatchingStatic);
                menu.AddItem(new GUIContent("Navigation Static"         ), (intStaticFlags & (int)StaticEditorFlags.NavigationStatic) > 0, staticChangeHandler, (int)StaticEditorFlags.NavigationStatic);
                menu.AddItem(new GUIContent("Occludee Static"           ), (intStaticFlags & (int)StaticEditorFlags.OccludeeStatic) > 0, staticChangeHandler, (int)StaticEditorFlags.OccludeeStatic);
                menu.AddItem(new GUIContent("Off Mesh Link Generation"  ), (intStaticFlags & (int)StaticEditorFlags.OffMeshLinkGeneration) > 0, staticChangeHandler, (int)StaticEditorFlags.OffMeshLinkGeneration);
                #if UNITY_4_6 || UNITY_4_7
                #else
                menu.AddItem(new GUIContent("Reflection Probe Static"   ), (intStaticFlags & (int)StaticEditorFlags.ReflectionProbeStatic) > 0, staticChangeHandler, (int)StaticEditorFlags.ReflectionProbeStatic);
                #endif
                menu.ShowAsContext();
            }
        }

        // PRIVATE
        private void staticChangeHandler(object result)
        {
            int intResult = (int)result;
            StaticEditorFlags resultStaticFlags = (StaticEditorFlags)result;
            if (intResult != 0 && intResult != -1)
            {
                resultStaticFlags = staticFlags ^ resultStaticFlags;
            }

            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                Undo.RecordObject(gameObject, "Change Static Flags");            
                GameObjectUtility.SetStaticEditorFlags(gameObject, resultStaticFlags);
                EditorUtility.SetDirty(gameObject);
            }
        }

        private void drawQuad(int startPosition, int width, int height, bool isActiveColor)
        {
            Color32 color = isActiveColor ? activeColor : inactiveColor;
            for (int iy = 0; iy < height; iy++)
            {
                for (int ix = 0; ix < width; ix++)
                {
                    int pos = startPosition + ix + iy * 11;
                    staticButtonColors[pos].r = color.r;
                    staticButtonColors[pos].g = color.g;
                    staticButtonColors[pos].b = color.b;
                    staticButtonColors[pos].a = color.a;
                }
            }
        }
    }
}

