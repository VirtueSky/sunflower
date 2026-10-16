using VirtueSky.Inspector;
using VirtueSky.Inspector.Drawers;
using VirtueSky.Inspector.Utilities;
using VirtueSky.InspectorUnityInternalBridge;
using UnityEditor;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(UnityDecoratorsDrawer), TriDrawerOrder.Inspector)]

namespace VirtueSky.Inspector.Drawers
{
    // Unity DecoratorDrawers ([Header], [Space], [HeaderLine], [TitleColor], ...) only run through Unity's PropertyHandler.
    // Arrays, lists, generic and reference properties are drawn by Tri elements, so decorators were lost there.
    public class UnityDecoratorsDrawer : TriAttributeDrawer<PropertyAttribute>
    {
        private DecoratorDrawer _decorator;

        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            base.Initialize(propertyDefinition);

            _decorator = ScriptAttributeUtilityProxy.CreateDecoratorDrawer(Attribute);

            return TriExtensionInitializationResult.Ok;
        }

        public override TriElement CreateElement(TriProperty property, TriElement next)
        {
            if (_decorator == null || !property.TryGetSerializedProperty(out var serializedProperty))
            {
                return next;
            }

            var handler = ScriptAttributeUtilityProxy.GetHandler(serializedProperty);

            // Must match the condition in CustomBuiltInDrawer: PropertyHandler already draws decorators there
            if (handler.hasPropertyDrawer ||
                property.PropertyType == TriPropertyType.Primitive ||
                TriUnityInspectorUtilities.MustDrawWithUnity(property))
            {
                return next;
            }

            return new UnityDecoratorElement(_decorator, next);
        }

        private class UnityDecoratorElement : TriElement
        {
            private readonly DecoratorDrawer _decorator;
            private readonly TriElement _next;

            public UnityDecoratorElement(DecoratorDrawer decorator, TriElement next)
            {
                _decorator = decorator;
                _next = next;

                AddChild(next);
            }

            public override float GetHeight(float width)
            {
                return _next.GetHeight(width) + _decorator.GetHeight();
            }

            public override void OnGUI(Rect position)
            {
                var decoratorRect = new Rect(position)
                {
                    height = _decorator.GetHeight(),
                };

                var contentRect = new Rect(position)
                {
                    yMin = decoratorRect.yMax,
                };

                _decorator.OnGUI(decoratorRect);

                _next.OnGUI(contentRect);
            }
        }
    }
}
