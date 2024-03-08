using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using VirtueSky.Hierarchy.Data;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;
using System.Text;

namespace VirtueSky.Hierarchy.HComponent
{
    public class ErrorComponent : BaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Texture2D errorIconTexture;
        private bool showErrorOfChildren;
        private bool showErrorTypeReferenceIsNull;
        private bool showErrorTypeReferenceIsMissing;
        private bool showErrorTypeStringIsEmpty;
        private bool showErrorIconScriptIsMissing;
        private bool showErrorIconWhenTagIsUndefined;
        private bool showErrorForDisabledComponents;
        private bool showErrorIconMissingEventMethod;
        private bool showErrorForDisabledGameObjects;
        private List<string> ignoreErrorOfMonoBehaviours;
        private StringBuilder errorStringBuilder;
        private int errorCount;

        // CONSTRUCTOR
        public ErrorComponent()
        {
            rect.width = 7;

            errorIconTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyErrorIcon);

            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowIconOnParent, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowReferenceIsNull, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowReferenceIsMissing, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowStringIsEmpty, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowScriptIsMissing, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowForDisabledComponents, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowForDisabledGameObjects, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowMissingEventMethod, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowWhenTagOrLayerIsUndefined, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShow, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ErrorIgnoreString, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalActiveColor, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor, settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            showErrorOfChildren = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowIconOnParent);
            showErrorTypeReferenceIsNull = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowReferenceIsNull);
            showErrorTypeReferenceIsMissing = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowReferenceIsMissing);
            showErrorTypeStringIsEmpty = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowStringIsEmpty);
            showErrorIconScriptIsMissing = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowScriptIsMissing);
            showErrorForDisabledComponents = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowForDisabledComponents);
            showErrorForDisabledGameObjects =
                HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowForDisabledGameObjects);
            showErrorIconMissingEventMethod = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowMissingEventMethod);
            showErrorIconWhenTagIsUndefined =
                HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowWhenTagOrLayerIsUndefined);
            activeColor = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalActiveColor);
            inactiveColor = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
            enabled = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.ErrorShowDuringPlayMode);

            string ignoreErrorOfMonoBehavioursString = HierarchySettings.getInstance().get<string>(HierarchySetting.ErrorIgnoreString);
            if (ignoreErrorOfMonoBehavioursString != "")
            {
                ignoreErrorOfMonoBehaviours =
                    new List<string>(ignoreErrorOfMonoBehavioursString.Split(new char[] { ',', ';', '.', ' ' }));
                ignoreErrorOfMonoBehaviours.RemoveAll(item => item == "");
            }
            else ignoreErrorOfMonoBehaviours = null;
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect,
            ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 7)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 7;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return LayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            bool errorFound = findError(gameObject, gameObject.GetComponents<MonoBehaviour>());

            if (errorFound)
            {
                HierarchyColorUtils.setColor(activeColor);
                GUI.DrawTexture(rect, errorIconTexture);
                HierarchyColorUtils.clearColor();
            }
            else if (showErrorOfChildren)
            {
                errorFound = findError(gameObject, gameObject.GetComponentsInChildren<MonoBehaviour>(true));
                if (errorFound)
                {
                    HierarchyColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, errorIconTexture);
                    HierarchyColorUtils.clearColor();
                }
            }
        }

        public override void eventHandler(GameObject gameObject, ObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 &&
                rect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();

                errorCount = 0;
                errorStringBuilder = new StringBuilder();
                findError(gameObject, gameObject.GetComponents<MonoBehaviour>(), true);

                if (errorCount > 0)
                {
                    EditorUtility.DisplayDialog(
                        errorCount + (errorCount == 1 ? " error was found" : " errors were found"),
                        errorStringBuilder.ToString(), "OK");
                }
            }
        }

        // PRIVATE
        private bool findError(GameObject gameObject, MonoBehaviour[] components, bool printError = false)
        {
            if (showErrorIconWhenTagIsUndefined)
            {
                try
                {
                    gameObject.tag.CompareTo(null);
                }
                catch
                {
                    if (printError)
                    {
                        appendErrorLine("Tag is undefined");
                    }
                    else
                    {
                        return true;
                    }
                }

                if (LayerMask.LayerToName(gameObject.layer).Equals(""))
                {
                    if (printError)
                    {
                        appendErrorLine("Layer is undefined");
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            for (int i = 0; i < components.Length; i++)
            {
                MonoBehaviour monoBehaviour = components[i];
                if (monoBehaviour == null)
                {
                    if (showErrorIconScriptIsMissing)
                    {
                        if (printError)
                        {
                            appendErrorLine("Component #" + i + " is missing");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (ignoreErrorOfMonoBehaviours != null)
                    {
                        for (int j = ignoreErrorOfMonoBehaviours.Count - 1; j >= 0; j--)
                        {
                            if (monoBehaviour.GetType().FullName.Contains(ignoreErrorOfMonoBehaviours[j]))
                            {
                                return false;
                            }
                        }
                    }

                    if (showErrorIconMissingEventMethod)
                    {
                        if (monoBehaviour.gameObject.activeSelf || showErrorForDisabledComponents)
                        {
                            try
                            {
                                if (isUnityEventsNullOrMissing(monoBehaviour, printError))
                                {
                                    if (!printError)
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (showErrorTypeReferenceIsNull || showErrorTypeStringIsEmpty || showErrorTypeReferenceIsMissing)
                    {
                        if (!monoBehaviour.enabled && !showErrorForDisabledComponents) continue;
                        if (!monoBehaviour.gameObject.activeSelf && !showErrorForDisabledGameObjects) continue;

                        Type type = monoBehaviour.GetType();

                        while (type != null)
                        {
                            BindingFlags bf = BindingFlags.Instance | BindingFlags.Public;
                            if (!type.FullName.Contains("UnityEngine"))
                                bf |= BindingFlags.NonPublic;
                            FieldInfo[] fieldArray = type.GetFields(bf);

                            for (int j = 0; j < fieldArray.Length; j++)
                            {
                                FieldInfo field = fieldArray[j];

                                if (System.Attribute.IsDefined(field, typeof(HideInInspector)) ||
                                    System.Attribute.IsDefined(field, typeof(HierarchyNullableAttribute)) ||
                                    System.Attribute.IsDefined(field, typeof(NonSerializedAttribute)) ||
                                    field.IsStatic) continue;

                                if (field.IsPrivate || !field.IsPublic)
                                {
                                    if (!Attribute.IsDefined(field, typeof(SerializeField)))
                                    {
                                        continue;
                                    }
                                }

                                object value = field.GetValue(monoBehaviour);

                                try
                                {
                                    if (showErrorTypeStringIsEmpty && field.FieldType == typeof(string) &&
                                        value != null && ((string)value).Equals(""))
                                    {
                                        if (printError)
                                        {
                                            appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name +
                                                            ": String value is empty");
                                            continue;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (showErrorTypeReferenceIsMissing && value != null && value is Component &&
                                        (Component)value == null)
                                    {
                                        if (printError)
                                        {
                                            appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name +
                                                            ": Reference is missing");
                                            continue;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (showErrorTypeReferenceIsNull && (value == null || value.Equals(null)))
                                    {
                                        if (printError)
                                        {
                                            appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name +
                                                            ": Reference is null");
                                            continue;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (showErrorTypeReferenceIsNull && value != null && (value is IEnumerable))
                                    {
                                        foreach (var item in (IEnumerable)value)
                                        {
                                            if (item == null || item.Equals(null))
                                            {
                                                if (printError)
                                                {
                                                    appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name +
                                                                    ": IEnumerable has value with null reference");
                                                    continue;
                                                }
                                                else
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }

                            type = type.BaseType;
                        }
                    }
                }
            }

            return false;
        }

        private List<string> targetPropertiesNames = new List<string>(10);

        private bool isUnityEventsNullOrMissing(MonoBehaviour monoBehaviour, bool printError)
        {
            targetPropertiesNames.Clear();
            FieldInfo[] fieldArray = monoBehaviour.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = fieldArray.Length - 1; i >= 0; i--)
            {
                FieldInfo field = fieldArray[i];
                if (field.FieldType == typeof(UnityEventBase) || field.FieldType.IsSubclassOf(typeof(UnityEventBase)))
                {
                    targetPropertiesNames.Add(field.Name);
                }
            }

            if (targetPropertiesNames.Count > 0)
            {
                SerializedObject serializedMonoBehaviour = new SerializedObject(monoBehaviour);
                for (int i = targetPropertiesNames.Count - 1; i >= 0; i--)
                {
                    string targetProperty = targetPropertiesNames[i];

                    SerializedProperty property = serializedMonoBehaviour.FindProperty(targetProperty);
                    SerializedProperty propertyRelativeArrray =
                        property.FindPropertyRelative("m_PersistentCalls.m_Calls");

                    for (int j = propertyRelativeArrray.arraySize - 1; j >= 0; j--)
                    {
                        SerializedProperty arrayElementAtIndex = propertyRelativeArrray.GetArrayElementAtIndex(j);

                        SerializedProperty propertyTarget = arrayElementAtIndex.FindPropertyRelative("m_Target");
                        if (propertyTarget.objectReferenceValue == null)
                        {
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name + ": Event object reference is null");
                            }
                            else
                            {
                                return true;
                            }
                        }

                        SerializedProperty propertyMethodName =
                            arrayElementAtIndex.FindPropertyRelative("m_MethodName");
                        if (string.IsNullOrEmpty(propertyMethodName.stringValue))
                        {
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name +
                                                ": Event handler function is not selected");
                                continue;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        string argumentAssemblyTypeName = arrayElementAtIndex.FindPropertyRelative("m_Arguments")
                            .FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue;
                        System.Type argumentAssemblyType;
                        if (!string.IsNullOrEmpty(argumentAssemblyTypeName))
                            argumentAssemblyType = System.Type.GetType(argumentAssemblyTypeName, false) ??
                                                   typeof(UnityEngine.Object);
                        else argumentAssemblyType = typeof(UnityEngine.Object);

                        UnityEventBase dummyEvent;
                        System.Type propertyTypeName =
                            System.Type.GetType(property.FindPropertyRelative("m_TypeName").stringValue, false);
                        if (propertyTypeName == null) dummyEvent = (UnityEventBase)new UnityEvent();
                        else dummyEvent = Activator.CreateInstance(propertyTypeName) as UnityEventBase;

                        if (!UnityEventDrawer.IsPersistantListenerValid(dummyEvent, propertyMethodName.stringValue,
                                propertyTarget.objectReferenceValue,
                                (PersistentListenerMode)arrayElementAtIndex.FindPropertyRelative("m_Mode")
                                    .enumValueIndex, argumentAssemblyType))
                        {
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name + ": Event handler function is missing");
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void appendErrorLine(string error)
        {
            errorCount++;
            errorStringBuilder.Append(errorCount.ToString());
            errorStringBuilder.Append(") ");
            errorStringBuilder.AppendLine(error);
        }
    }
}