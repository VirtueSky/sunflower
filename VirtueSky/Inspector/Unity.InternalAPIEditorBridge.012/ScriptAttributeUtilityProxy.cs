using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VirtueSky.InspectorUnityInternalBridge
{
    public static class ScriptAttributeUtilityProxy
    {
        public static PropertyHandlerProxy GetHandler(SerializedProperty property)
        {
            var handler = ScriptAttributeUtility.GetHandler(property);
            return new PropertyHandlerProxy(handler);
        }

        public static DecoratorDrawer CreateDecoratorDrawer(PropertyAttribute attribute)
        {
#if UNITY_2023_3_OR_NEWER
            var drawerType = ScriptAttributeUtility.GetDrawerTypeForType(attribute.GetType(), null);
#else
            var drawerType = ScriptAttributeUtility.GetDrawerTypeForType(attribute.GetType());
#endif
            if (drawerType == null || !typeof(DecoratorDrawer).IsAssignableFrom(drawerType))
            {
                return null;
            }

            var drawer = (DecoratorDrawer) System.Activator.CreateInstance(drawerType);
            drawer.m_Attribute = attribute;
            return drawer;
        }
    }

    public readonly struct PropertyHandlerProxy
    {
        private readonly PropertyHandler _handler;

        internal PropertyHandlerProxy(PropertyHandler handler)
        {
            _handler = handler;
        }

        // ReSharper disable once InconsistentNaming
        public bool hasPropertyDrawer => _handler.hasPropertyDrawer;

        public void SetPreferredLabel(string label)
        {
#if UNITY_2022_2_OR_NEWER
            if (_handler.propertyDrawer != null)
            {
                _handler.propertyDrawer.m_PreferredLabel = label;
            }
#endif
        }

        public VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return _handler.propertyDrawer?.CreatePropertyGUI(property);
        }

        public float GetHeight(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            return _handler.GetHeight(property, label, includeChildren);
        }

        public bool OnGUI(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            return _handler.OnGUI(position, property, label, includeChildren);
        }
    }
}