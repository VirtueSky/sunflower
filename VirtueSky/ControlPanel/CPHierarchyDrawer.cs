using System;
using System.Collections.Generic;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;
using UnityEditor;
using UnityEngine;


namespace VirtueSky.ControlPanel.Editor
{
    public class CPHierarchyDrawer
    {
        public static void OnDrawQHierarchyEvent(Rect posittion, EditorWindow window)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("HIERARCHY", EditorStyles.boldLabel);
            GUILayout.Space(10);
            OnGUI(posittion, window);
            GUILayout.EndVertical();
        }

        private static void init(EditorWindow window)
        {
            HierarchySettings.getInstance().inited = true;
            HierarchySettings.getInstance().isProSkin = EditorGUIUtility.isProSkin;
            HierarchySettings.getInstance().separatorColor =
                HierarchySettings.getInstance().isProSkin
                    ? new Color(0.18f, 0.18f, 0.18f)
                    : new Color(0.59f, 0.59f, 0.59f);
            HierarchySettings.getInstance().yellowColor =
                HierarchySettings.getInstance().isProSkin
                    ? new Color(1.00f, 0.90f, 0.40f)
                    : new Color(0.31f, 0.31f, 0.31f);
            HierarchySettings.getInstance().checkBoxChecked = HierarchyResources.getInstance()
                .getTexture(HierarchyTexture.HierarchyCheckBoxChecked);
            HierarchySettings.getInstance().checkBoxUnchecked =
                HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyCheckBoxUnchecked);
            HierarchySettings.getInstance().restoreButtonTexture = HierarchyResources.getInstance()
                .getTexture(HierarchyTexture.HierarchyRestoreButton);
            HierarchySettings.getInstance().componentsOrderList = new HierarchyComponentsOrderList(window);
        }

        // GUI
        public static void OnGUI(Rect position, EditorWindow window)
        {
            if (!HierarchySettings.getInstance().inited ||
                HierarchySettings.getInstance().isProSkin != EditorGUIUtility.isProSkin)
                init(window);

            HierarchySettings.getInstance().indentLevel = 8;
            HierarchySettings.getInstance().scrollPosition =
                EditorGUILayout.BeginScrollView(HierarchySettings.getInstance().scrollPosition);
            {
                Rect targetRect = EditorGUILayout.GetControlRect(GUILayout.Height(0));
                if (Event.current.type == EventType.Repaint)
                    HierarchySettings.getInstance().totalWidth = targetRect.width + 8;

                HierarchySettings.getInstance().lastRect = new Rect(0, 1, 0, 0);

                // COMPONENTS
                drawSection("COMPONENTS SETTINGS");
                float sectionStartY = HierarchySettings.getInstance().lastRect.y +
                                      HierarchySettings.getInstance().lastRect.height;

                drawTreeMapComponentSettings();
                drawSeparator();
                drawMonoBehaviourIconComponentSettings();
                drawSeparator();
                drawSeparatorComponentSettings();
                drawSeparator();
                drawVisibilityComponentSettings();
                drawSeparator();
                drawLockComponentSettings();
                drawSeparator();
                drawStaticComponentSettings();
                drawSeparator();
                drawErrorComponentSettings();
                drawSeparator();
                drawRendererComponentSettings();
                drawSeparator();
                drawPrefabComponentSettings();
                drawSeparator();
                drawTagLayerComponentSettings();
                drawSeparator();
                drawColorComponentSettings();
                drawSeparator();
                drawGameObjectIconComponentSettings();
                drawSeparator();
                drawTagIconComponentSettings();
                drawSeparator();
                drawLayerIconComponentSettings();
                drawSeparator();
                drawChildrenCountComponentSettings();
                drawSeparator();
                drawVerticesAndTrianglesCountComponentSettings();
                drawSeparator();
                drawComponentsComponentSettings();
                drawLeftLine(sectionStartY,
                    HierarchySettings.getInstance().lastRect.y + HierarchySettings.getInstance().lastRect.height,
                    HierarchySettings.getInstance().separatorColor);

                // ORDER
                drawSection("ORDER OF COMPONENTS");
                sectionStartY = HierarchySettings.getInstance().lastRect.y +
                                HierarchySettings.getInstance().lastRect.height;
                drawSpace(8);
                drawOrderSettings(position, window);
                drawSpace(6);
                drawLeftLine(sectionStartY,
                    HierarchySettings.getInstance().lastRect.y + HierarchySettings.getInstance().lastRect.height,
                    HierarchySettings.getInstance().separatorColor);

                // ADDITIONAL
                drawSection("ADDITIONAL SETTINGS");
                sectionStartY = HierarchySettings.getInstance().lastRect.y +
                                HierarchySettings.getInstance().lastRect.height;
                drawSpace(3);
                drawAdditionalSettings();
                drawLeftLine(sectionStartY,
                    HierarchySettings.getInstance().lastRect.y + HierarchySettings.getInstance().lastRect.height + 4,
                    HierarchySettings.getInstance().separatorColor);

                HierarchySettings.getInstance().indentLevel -= 8;
            }

            EditorGUILayout.EndScrollView();
        }

        // COMPONENTS
        private static void drawTreeMapComponentSettings()
        {
            if (drawComponentCheckBox("Hierarchy Tree", HierarchySetting.TreeMapShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.TreeMapColor);
                    HierarchySettings.getInstance().restore(HierarchySetting.TreeMapEnhanced);
                    HierarchySettings.getInstance().restore(HierarchySetting.TreeMapTransparentBackground);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawColorPicker("Tree color", HierarchySetting.TreeMapColor);
                drawCheckBoxRight("Transparent background", HierarchySetting.TreeMapTransparentBackground);
                drawCheckBoxRight("Enhanced (\"Transform Sort\" only)", HierarchySetting.TreeMapEnhanced);
                drawSpace(1);
            }
        }

        private static void drawMonoBehaviourIconComponentSettings()
        {
            if (drawComponentCheckBox("MonoBehaviour Icon", HierarchySetting.MonoBehaviourIconShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.MonoBehaviourIconShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.MonoBehaviourIconColor);
                    HierarchySettings.getInstance()
                        .restore(HierarchySetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.MonoBehaviourIconShowDuringPlayMode);
                drawColorPicker("Icon color", HierarchySetting.MonoBehaviourIconColor);
                drawCheckBoxRight("Ignore Unity MonoBehaviours",
                    HierarchySetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
                drawSpace(1);
            }
        }

        private static void drawSeparatorComponentSettings()
        {
            if (drawComponentCheckBox("Separator", HierarchySetting.SeparatorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.SeparatorColor);
                    HierarchySettings.getInstance().restore(HierarchySetting.SeparatorShowRowShading);
                    HierarchySettings.getInstance().restore(HierarchySetting.SeparatorOddRowShadingColor);
                    HierarchySettings.getInstance().restore(HierarchySetting.SeparatorEvenRowShadingColor);
                }

                bool rowShading =
                    HierarchySettings.getInstance().get<bool>(HierarchySetting.SeparatorShowRowShading);

                drawBackground(rect.x, rect.y, rect.width, 18 * (rowShading ? 4 : 2) + 5);
                drawSpace(4);
                drawColorPicker("Separator Color", HierarchySetting.SeparatorColor);
                drawCheckBoxRight("Row shading", HierarchySetting.SeparatorShowRowShading);
                if (rowShading)
                {
                    drawColorPicker("Even row shading color",
                        HierarchySetting.SeparatorEvenRowShadingColor);
                    drawColorPicker("Odd row shading color", HierarchySetting.SeparatorOddRowShadingColor);
                }

                drawSpace(1);
            }
        }

        private static void drawVisibilityComponentSettings()
        {
            if (drawComponentCheckBox("Visibility", HierarchySetting.VisibilityShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.VisibilityShowDuringPlayMode);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.VisibilityShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private static void drawLockComponentSettings()
        {
            if (drawComponentCheckBox("Lock", HierarchySetting.LockShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.LockShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.LockPreventSelectionOfLockedObjects);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 2 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.LockShowDuringPlayMode);
                drawCheckBoxRight("Prevent selection of locked objects",
                    HierarchySetting.LockPreventSelectionOfLockedObjects);
                drawSpace(1);
            }
        }

        private static void drawStaticComponentSettings()
        {
            if (drawComponentCheckBox("Static", HierarchySetting.StaticShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.StaticShowDuringPlayMode);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.StaticShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private static void drawErrorComponentSettings()
        {
            if (drawComponentCheckBox("Error", HierarchySetting.ErrorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowIconOnParent);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowForDisabledComponents);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowForDisabledGameObjects);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowScriptIsMissing);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowReferenceIsMissing);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowReferenceIsNull);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowStringIsEmpty);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowMissingEventMethod);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorShowWhenTagOrLayerIsUndefined);
                    HierarchySettings.getInstance().restore(HierarchySetting.ErrorIgnoreString);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 12 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.ErrorShowDuringPlayMode);
                drawCheckBoxRight("Show error icon up of hierarchy (very slow)",
                    HierarchySetting.ErrorShowIconOnParent);
                drawCheckBoxRight("Show error icon for disabled components",
                    HierarchySetting.ErrorShowForDisabledComponents);
                drawCheckBoxRight("Show error icon for disabled GameObjects",
                    HierarchySetting.ErrorShowForDisabledGameObjects);
                drawLabel("Show error icon for the following:");
                HierarchySettings.getInstance().indentLevel += 16;
                drawCheckBoxRight("- script is missing", HierarchySetting.ErrorShowScriptIsMissing);
                drawCheckBoxRight("- reference is missing", HierarchySetting.ErrorShowReferenceIsMissing);
                drawCheckBoxRight("- reference is null", HierarchySetting.ErrorShowReferenceIsNull);
                drawCheckBoxRight("- string is empty", HierarchySetting.ErrorShowStringIsEmpty);
                drawCheckBoxRight("- callback of event is missing (very slow)",
                    HierarchySetting.ErrorShowMissingEventMethod);
                drawCheckBoxRight("- tag or layer is undefined",
                    HierarchySetting.ErrorShowWhenTagOrLayerIsUndefined);
                HierarchySettings.getInstance().indentLevel -= 16;
                drawTextField("Ignore packages/classes", HierarchySetting.ErrorIgnoreString);
                drawSpace(1);
            }
        }

        private static void drawRendererComponentSettings()
        {
            if (drawComponentCheckBox("Renderer", HierarchySetting.RendererShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.RendererShowDuringPlayMode);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.RendererShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private static void drawPrefabComponentSettings()
        {
            if (drawComponentCheckBox("Prefab", HierarchySetting.PrefabShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.PrefabShowBreakedPrefabsOnly);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show icon for broken prefabs only",
                    HierarchySetting.PrefabShowBreakedPrefabsOnly);
                drawSpace(1);
            }
        }

        private static void drawTagLayerComponentSettings()
        {
            if (drawComponentCheckBox("Tag And Layer", HierarchySetting.TagAndLayerShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerSizeShowType);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerType);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerSizeValueType);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerSizeValuePixel);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerSizeValuePercent);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerAligment);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerLabelSize);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerLabelAlpha);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerTagLabelColor);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagAndLayerLayerLabelColor);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 10 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.TagAndLayerShowDuringPlayMode);
                drawEnum("Show", HierarchySetting.TagAndLayerSizeShowType,
                    typeof(HierarchyTagAndLayerShowType));
                drawEnum("Show tag and layer", HierarchySetting.TagAndLayerType,
                    typeof(HierarchyTagAndLayerType));

                HierarchyTagAndLayerSizeType newTagAndLayerSizeValueType =
                    (HierarchyTagAndLayerSizeType)drawEnum("Unit of width",
                        HierarchySetting.TagAndLayerSizeValueType, typeof(HierarchyTagAndLayerSizeType));

                if (newTagAndLayerSizeValueType == HierarchyTagAndLayerSizeType.Pixel)
                    drawIntSlider("Width in pixels", HierarchySetting.TagAndLayerSizeValuePixel, 5, 250);
                else
                    drawFloatSlider("Percentage width", HierarchySetting.TagAndLayerSizeValuePercent, 0,
                        0.5f);

                drawEnum("Alignment", HierarchySetting.TagAndLayerAligment,
                    typeof(HierarchyTagAndLayerAligment));
                drawEnum("Label size", HierarchySetting.TagAndLayerLabelSize,
                    typeof(HierarchyTagAndLayerLabelSize));
                drawFloatSlider("Label alpha if default", HierarchySetting.TagAndLayerLabelAlpha, 0, 1.0f);
                drawColorPicker("Tag label color", HierarchySetting.TagAndLayerTagLabelColor);
                drawColorPicker("Layer label color", HierarchySetting.TagAndLayerLayerLabelColor);
                drawSpace(1);
            }
        }

        private static void drawColorComponentSettings()
        {
            if (drawComponentCheckBox("Color", HierarchySetting.ColorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.ColorShowDuringPlayMode);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.ColorShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private static void drawGameObjectIconComponentSettings()
        {
            if (drawComponentCheckBox("GameObject Icon", HierarchySetting.GameObjectIconShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.GameObjectIconShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.GameObjectIconSize);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 2 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.GameObjectIconShowDuringPlayMode);
                drawEnum("Icon size", HierarchySetting.GameObjectIconSize, typeof(HierarchySizeAll));
                drawSpace(1);
            }
        }

        private static void drawTagIconComponentSettings()
        {
            if (drawComponentCheckBox("Tag Icon", HierarchySetting.TagIconShow))
            {
                string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

                bool showTagIconList =
                    HierarchySettings.getInstance().get<bool>(HierarchySetting.TagIconListFoldout);

                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.TagIconShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.TagIconSize);
                }

                drawBackground(rect.x, rect.y, rect.width,
                    18 * 3 + (showTagIconList ? 18 * tags.Length : 0) + 4 + 5);

                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.TagIconShowDuringPlayMode);
                drawEnum("Icon size", HierarchySetting.TagIconSize, typeof(HierarchySizeAll));
                if (drawFoldout("Tag icon list", HierarchySetting.TagIconListFoldout))
                {
                    List<TagTexture> tagTextureList = TagTexture.loadTagTextureList();

                    bool changed = false;
                    for (int i = 0; i < tags.Length; i++)
                    {
                        string tag = tags[i];
                        TagTexture tagTexture = tagTextureList.Find(t => t.tag == tag);
                        Texture2D newTexture = (Texture2D)EditorGUI.ObjectField(
                            getControlRect(0, 16, 34 + 16, 6),
                            tag, tagTexture == null ? null : tagTexture.texture, typeof(Texture2D),
                            false);
                        if (newTexture != null && tagTexture == null)
                        {
                            TagTexture newTagTexture = new TagTexture(tag, newTexture);
                            tagTextureList.Add(newTagTexture);

                            changed = true;
                        }
                        else if (newTexture == null && tagTexture != null)
                        {
                            tagTextureList.Remove(tagTexture);
                            changed = true;
                        }
                        else if (tagTexture != null && tagTexture.texture != newTexture)
                        {
                            tagTexture.texture = newTexture;
                            changed = true;
                        }

                        drawSpace(i == tags.Length - 1 ? 2 : 2);
                    }

                    if (changed)
                    {
                        TagTexture.saveTagTextureList(HierarchySetting.TagIconList, tagTextureList);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }

                drawSpace(1);
            }
        }

        private static void drawLayerIconComponentSettings()
        {
            if (drawComponentCheckBox("Layer Icon", HierarchySetting.LayerIconShow))
            {
                string[] layers = UnityEditorInternal.InternalEditorUtility.layers;

                bool showLayerIconList =
                    HierarchySettings.getInstance().get<bool>(HierarchySetting.LayerIconListFoldout);

                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.LayerIconShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.LayerIconSize);
                }

                drawBackground(rect.x, rect.y, rect.width,
                    18 * 3 + (showLayerIconList ? 18 * layers.Length : 0) + 4 + 5);

                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.LayerIconShowDuringPlayMode);
                drawEnum("Icon size", HierarchySetting.LayerIconSize, typeof(HierarchySizeAll));
                if (drawFoldout("Layer icon list", HierarchySetting.LayerIconListFoldout))
                {
                    List<LayerTexture> layerTextureList = LayerTexture.loadLayerTextureList();

                    bool changed = false;
                    for (int i = 0; i < layers.Length; i++)
                    {
                        string layer = layers[i];
                        LayerTexture layerTexture = layerTextureList.Find(t => t.layer == layer);
                        Texture2D newTexture = (Texture2D)EditorGUI.ObjectField(
                            getControlRect(0, 16, 34 + 16, 6),
                            layer, layerTexture == null ? null : layerTexture.texture,
                            typeof(Texture2D), false);
                        if (newTexture != null && layerTexture == null)
                        {
                            LayerTexture newLayerTexture = new LayerTexture(layer, newTexture);
                            layerTextureList.Add(newLayerTexture);

                            changed = true;
                        }
                        else if (newTexture == null && layerTexture != null)
                        {
                            layerTextureList.Remove(layerTexture);
                            changed = true;
                        }
                        else if (layerTexture != null && layerTexture.texture != newTexture)
                        {
                            layerTexture.texture = newTexture;
                            changed = true;
                        }

                        drawSpace(i == layers.Length - 1 ? 2 : 2);
                    }

                    if (changed)
                    {
                        LayerTexture.saveLayerTextureList(HierarchySetting.LayerIconList,
                            layerTextureList);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }

                drawSpace(1);
            }
        }

        private static void drawChildrenCountComponentSettings()
        {
            if (drawComponentCheckBox("Children Count", HierarchySetting.ChildrenCountShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.ChildrenCountShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.ChildrenCountLabelSize);
                    HierarchySettings.getInstance().restore(HierarchySetting.ChildrenCountLabelColor);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.ChildrenCountShowDuringPlayMode);
                drawEnum("Label size", HierarchySetting.ChildrenCountLabelSize, typeof(HierarchySize));
                drawColorPicker("Label color", HierarchySetting.ChildrenCountLabelColor);
                drawSpace(1);
            }
        }

        private static void drawVerticesAndTrianglesCountComponentSettings()
        {
            if (drawComponentCheckBox("Vertices And Triangles Count",
                    HierarchySetting.VerticesAndTrianglesShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance()
                        .restore(HierarchySetting.VerticesAndTrianglesShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.VerticesAndTrianglesShowVertices);
                    HierarchySettings.getInstance().restore(HierarchySetting.VerticesAndTrianglesShowTriangles);
                    HierarchySettings.getInstance()
                        .restore(HierarchySetting.VerticesAndTrianglesCalculateTotalCount);
                    HierarchySettings.getInstance().restore(HierarchySetting.VerticesAndTrianglesLabelSize);
                    HierarchySettings.getInstance()
                        .restore(HierarchySetting.VerticesAndTrianglesVerticesLabelColor);
                    HierarchySettings.getInstance()
                        .restore(HierarchySetting.VerticesAndTrianglesTrianglesLabelColor);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 7 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.VerticesAndTrianglesShowDuringPlayMode);
                if (drawCheckBoxRight("Show vertices count",
                        HierarchySetting.VerticesAndTrianglesShowVertices))
                {
                    if (HierarchySettings.getInstance()
                            .get<bool>(HierarchySetting.VerticesAndTrianglesShowVertices) == false &&
                        HierarchySettings.getInstance()
                            .get<bool>(HierarchySetting.VerticesAndTrianglesShowTriangles) == false)
                        HierarchySettings.getInstance()
                            .set(HierarchySetting.VerticesAndTrianglesShowTriangles, true);
                }

                if (drawCheckBoxRight("Show triangles count (very slow)",
                        HierarchySetting.VerticesAndTrianglesShowTriangles))
                {
                    if (HierarchySettings.getInstance()
                            .get<bool>(HierarchySetting.VerticesAndTrianglesShowVertices) == false &&
                        HierarchySettings.getInstance()
                            .get<bool>(HierarchySetting.VerticesAndTrianglesShowTriangles) == false)
                        HierarchySettings.getInstance()
                            .set(HierarchySetting.VerticesAndTrianglesShowVertices, true);
                }

                drawCheckBoxRight("Calculate the count including children (very slow)",
                    HierarchySetting.VerticesAndTrianglesCalculateTotalCount);
                drawEnum("Label size", HierarchySetting.VerticesAndTrianglesLabelSize,
                    typeof(HierarchySize));
                drawColorPicker("Vertices label color",
                    HierarchySetting.VerticesAndTrianglesVerticesLabelColor);
                drawColorPicker("Triangles label color",
                    HierarchySetting.VerticesAndTrianglesTrianglesLabelColor);
                drawSpace(1);
            }
        }

        private static void drawComponentsComponentSettings()
        {
            if (drawComponentCheckBox("Components", HierarchySetting.ComponentsShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    HierarchySettings.getInstance().restore(HierarchySetting.ComponentsShowDuringPlayMode);
                    HierarchySettings.getInstance().restore(HierarchySetting.ComponentsIconSize);
                }

                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 6);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode",
                    HierarchySetting.ComponentsShowDuringPlayMode);
                drawEnum("Icon size", HierarchySetting.ComponentsIconSize, typeof(HierarchySizeAll));
                drawTextField("Ignore packages/classes", HierarchySetting.ComponentsIgnore);
                drawSpace(2);
            }
        }

        // COMPONENTS ORDER
        private static void drawOrderSettings(Rect position, EditorWindow window)
        {
            if (drawRestore())
            {
                HierarchySettings.getInstance().restore(HierarchySetting.ComponentsOrder);
            }

            HierarchySettings.getInstance().indentLevel += 4;

            string componentOrder = HierarchySettings.getInstance().get<string>(HierarchySetting.ComponentsOrder);
            string[] componentIds = componentOrder.Split(';');

            Rect rect = getControlRect(position.width, 17 * componentIds.Length + 10, 0, 0);
            if (HierarchySettings.getInstance().componentsOrderList == null)
                HierarchySettings.getInstance().componentsOrderList = new HierarchyComponentsOrderList(window);
            HierarchySettings.getInstance().componentsOrderList.draw(rect, componentIds);

            HierarchySettings.getInstance().indentLevel -= 4;
        }

        // ADDITIONAL SETTINGS
        private static void drawAdditionalSettings()
        {
            if (drawRestore())
            {
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalShowHiddenQHierarchyObjectList);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalHideIconsIfNotFit);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalIdentation);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalShowModifierWarning);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalBackgroundColor);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalActiveColor);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalInactiveColor);
                HierarchySettings.getInstance().restore(HierarchySetting.AdditionalSpecialColor);
            }

            drawSpace(4);
            drawCheckBoxRight("Show QHierarchyObjectList GameObject",
                HierarchySetting.AdditionalShowHiddenQHierarchyObjectList);
            drawCheckBoxRight("Hide icons if not fit", HierarchySetting.AdditionalHideIconsIfNotFit);
            drawIntSlider("Right indent", HierarchySetting.AdditionalIdentation, 0, 500);
            drawCheckBoxRight("Show warning when using modifiers + click",
                HierarchySetting.AdditionalShowModifierWarning);
            drawColorPicker("Background color", HierarchySetting.AdditionalBackgroundColor);
            drawColorPicker("Active color", HierarchySetting.AdditionalActiveColor);
            drawColorPicker("Inactive color", HierarchySetting.AdditionalInactiveColor);
            drawColorPicker("Special color", HierarchySetting.AdditionalSpecialColor);
            drawSpace(1);
        }

        // PRIVATE
        private static void drawSection(string title)
        {
            Rect rect = getControlRect(0, 24, -3, 0);
            rect.width *= 2;
            rect.x = 0;
            GUI.Box(rect, "");

            drawLeftLine(rect.y, rect.y + 24, HierarchySettings.getInstance().yellowColor);

            rect.x = HierarchySettings.getInstance().lastRect.x + 8;
            rect.y += 4;
            EditorGUI.LabelField(rect, title);
        }

        private static void drawSeparator(int spaceBefore = 0, int spaceAfter = 0, int height = 1)
        {
            if (spaceBefore > 0) drawSpace(spaceBefore);
            Rect rect = getControlRect(0, height, 0, 0);
            rect.width += 8;
            EditorGUI.DrawRect(rect, HierarchySettings.getInstance().separatorColor);
            if (spaceAfter > 0) drawSpace(spaceAfter);
        }

        private static bool drawComponentCheckBox(string label, HierarchySetting setting)
        {
            HierarchySettings.getInstance().indentLevel += 8;

            Rect rect = getControlRect(0, 28, 0, 0);

            float rectWidth = rect.width;
            bool isChecked = HierarchySettings.getInstance().get<bool>(setting);

            rect.x -= 1;
            rect.y += 7;
            rect.width = 14;
            rect.height = 14;

            if (GUI.Button(rect,
                    isChecked
                        ? HierarchySettings.getInstance().checkBoxChecked
                        : HierarchySettings.getInstance().checkBoxUnchecked,
                    GUIStyle.none))
            {
                HierarchySettings.getInstance().set(setting, !isChecked);
            }

            rect.x += 14 + 10;
            rect.width = rectWidth - 14 - 8;
            rect.y -= (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
            rect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(rect, label);

            HierarchySettings.getInstance().indentLevel -= 8;

            return isChecked;
        }

        private static bool drawCheckBoxRight(string label, HierarchySetting setting)
        {
            Rect rect = getControlRect(0, 18, 34, 6);
            bool result = false;
            bool isChecked = HierarchySettings.getInstance().get<bool>(setting);

            float tempX = rect.x;
            rect.x += rect.width - 14;
            rect.y += 1;
            rect.width = 14;
            rect.height = 14;

            if (GUI.Button(rect,
                    isChecked
                        ? HierarchySettings.getInstance().checkBoxChecked
                        : HierarchySettings.getInstance().checkBoxUnchecked,
                    GUIStyle.none))
            {
                HierarchySettings.getInstance().set(setting, !isChecked);
                result = true;
            }

            rect.width = rect.x - tempX - 4;
            rect.x = tempX;
            rect.y -= (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
            rect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(rect, label);

            return result;
        }

        private static void drawSpace(int value)
        {
            getControlRect(0, value, 0, 0);
        }

        private static void drawBackground(float x, float y, float width, float height)
        {
            EditorGUI.DrawRect(new Rect(x, y, width, height), HierarchySettings.getInstance().separatorColor);
        }

        private static void drawLeftLine(float fromY, float toY, Color color, float width = 0)
        {
            EditorGUI.DrawRect(
                new Rect(0, fromY, width == 0 ? HierarchySettings.getInstance().indentLevel : width, toY - fromY),
                color);
        }

        private static Rect getControlRect(float width, float height, float addIndent = 0,
            float remWidth = 0)
        {
            EditorGUILayout.GetControlRect(false, height, GUIStyle.none,
                GUILayout.ExpandWidth(true));
            Rect rect = new Rect(HierarchySettings.getInstance().indentLevel + addIndent,
                HierarchySettings.getInstance().lastRect.y + HierarchySettings.getInstance().lastRect.height,
                (width == 0
                    ? HierarchySettings.getInstance().totalWidth - HierarchySettings.getInstance().indentLevel -
                      addIndent - remWidth
                    : width), height);
            HierarchySettings.getInstance().lastRect = rect;
            return rect;
        }

        private static bool drawRestore()
        {
            if (GUI.Button(
                    new Rect(
                        HierarchySettings.getInstance().lastRect.x + HierarchySettings.getInstance().lastRect.width -
                        16 - 5,
                        HierarchySettings.getInstance().lastRect.y - 20, 16, 16),
                    HierarchySettings.getInstance().restoreButtonTexture, GUIStyle.none))
            {
                if (EditorUtility.DisplayDialog("Restore", "Restore default settings?", "Ok",
                        "Cancel"))
                {
                    return true;
                }
            }

            return false;
        }

        // GUI COMPONENTS
        private static void drawLabel(string label)
        {
            Rect rect = getControlRect(0, 16, 34, 6);
            rect.y -= (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
            EditorGUI.LabelField(rect, label);
            drawSpace(2);
        }

        private static void drawTextField(string label, HierarchySetting setting)
        {
            string currentValue = HierarchySettings.getInstance().get<string>(setting);
            string newValue =
                EditorGUI.TextField(getControlRect(0, 16, 34, 6), label, currentValue);
            if (!currentValue.Equals(newValue)) HierarchySettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }

        private static bool drawFoldout(string label, HierarchySetting setting)
        {
#if UNITY_2019_1_OR_NEWER
            Rect foldoutRect = getControlRect(0, 16, 19, 6);
#else
                Rect foldoutRect = getControlRect(0, 16, 22, 6);
#endif
            bool foldoutValue = HierarchySettings.getInstance().get<bool>(setting);
            bool newFoldoutValue = EditorGUI.Foldout(foldoutRect, foldoutValue, label);
            if (foldoutValue != newFoldoutValue)
                HierarchySettings.getInstance().set(setting, newFoldoutValue);
            drawSpace(2);
            return newFoldoutValue;
        }

        private static void drawColorPicker(string label, HierarchySetting setting)
        {
            Color currentColor = HierarchySettings.getInstance().getColor(setting);
            Color newColor =
                EditorGUI.ColorField(getControlRect(0, 16, 34, 6), label, currentColor);
            if (!currentColor.Equals(newColor)) HierarchySettings.getInstance().setColor(setting, newColor);
            drawSpace(2);
        }

        private static Enum drawEnum(string label, HierarchySetting setting, Type enumType)
        {
            Enum currentEnum =
                (Enum)Enum.ToObject(enumType, HierarchySettings.getInstance().get<int>(setting));
            Enum newEnumValue;
            if (!(newEnumValue =
                    EditorGUI.EnumPopup(getControlRect(0, 16, 34, 6), label, currentEnum))
                .Equals(currentEnum))
                HierarchySettings.getInstance().set(setting, (int)(object)newEnumValue);
            drawSpace(2);
            return newEnumValue;
        }

        private static void drawIntSlider(string label, HierarchySetting setting, int minValue, int maxValue)
        {
            Rect rect = getControlRect(0, 16, 34, 4);
            int currentValue = HierarchySettings.getInstance().get<int>(setting);
            int newValue = EditorGUI.IntSlider(rect, label, currentValue, minValue, maxValue);
            if (currentValue != newValue) HierarchySettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }

        private static void drawFloatSlider(string label, HierarchySetting setting, float minValue, float maxValue)
        {
            Rect rect = getControlRect(0, 16, 34, 4);
            float currentValue = HierarchySettings.getInstance().get<float>(setting);
            float newValue = EditorGUI.Slider(rect, label, currentValue, minValue, maxValue);
            if (currentValue != newValue) HierarchySettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }
    }
}