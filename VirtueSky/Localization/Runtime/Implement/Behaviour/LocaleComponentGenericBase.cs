using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    public abstract class LocaleComponentGenericBase : LocaleComponent
    {
        [SerializeField] private Component component;
        [SerializeField] private Optional<string> property;

        private PropertyInfo _propertyInfo;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_propertyInfo == null) TryInitProperty();
        }

        private bool TryInitProperty()
        {
            if (component != null)
            {
                _propertyInfo = FindProperty(component, property.Value);
                return _propertyInfo != null;
            }

            return false;
        }

        public bool TrySetComponentAndProperty<TComponent>(string propertyName) where TComponent : Component
        {
            component = GetComponent<TComponent>();
            if (component != null)
            {
                property = new Optional<string>(false, propertyName);

                if (!TryInitProperty())
                {
                    property = new Optional<string>(false, "");
                    return false;
                }

                return true;
            }

            return false;
        }

        public bool TrySetComponentAndPropertyIfNotSet<TComponent>(string propertyName)
            where TComponent : Component
        {
            return component == null && TrySetComponentAndProperty<TComponent>(propertyName);
        }

        private PropertyInfo FindProperty(Component c, string propertyName)
        {
            return c.GetType().GetProperty(propertyName, GetValueType());
        }

        /// <summary>
        /// Finds list of localizable properties of specified component.
        /// </summary>
        internal List<PropertyInfo> FindProperties(Component component)
        {
            var valueType = GetValueType();
            var allProperties = component.GetType().GetProperties();
            var properties = new List<PropertyInfo>();
            foreach (var p in allProperties)
            {
                if (p.CanWrite && valueType.IsAssignableFrom(p.PropertyType)) properties.Add(p);
            }

            return properties;
        }

        protected abstract Type GetValueType();
        protected abstract bool HasLocaleValue();
        protected abstract object GetLocaleValue();

        protected override bool TryUpdateComponentLocalization(bool isOnValidate)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) Init();
#endif

            if (HasLocaleValue() && _propertyInfo != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) UnityEditor.Undo.RecordObject(component, "locale value changed");
#endif
                _propertyInfo.SetValue(component, GetLocaleValue(), null);
                return true;
            }

            return false;
        }
    }
}