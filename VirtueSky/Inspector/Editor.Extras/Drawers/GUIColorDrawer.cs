using JetBrains.Annotations;
using VirtueSky.Inspector;
using VirtueSky.Inspector.Drawers;
using VirtueSky.Inspector.Resolvers;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(GUIColorDrawer), TriDrawerOrder.Decorator)]

namespace VirtueSky.Inspector.Drawers
{
    public class GUIColorDrawer : TriAttributeDrawer<GUIColorAttribute>
    {
        [CanBeNull] private ValueResolver<Color> _colorResolver;

        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            if (!string.IsNullOrEmpty(Attribute.GetColor))
            {
                _colorResolver = ValueResolver.Resolve<Color>(propertyDefinition, Attribute.GetColor);
            }

            if (_colorResolver != null && _colorResolver.TryGetErrorString(out var error))
            {
                return error;
            }

            return TriExtensionInitializationResult.Ok;
        }

        public override void OnGUI(Rect position, TriProperty property, TriElement next)
        {
            var oldColor = GUI.color;
            var newColor = _colorResolver?.GetValue(property, Color.white) ?? Attribute.Color;

            GUI.color = newColor;
            GUI.contentColor = newColor;

            next.OnGUI(position);

            GUI.color = oldColor;
            GUI.contentColor = oldColor;
        }
    }
}