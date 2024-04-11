using VirtueSky.Inspector;
using VirtueSky.Inspector.Drawers;
using VirtueSky.Inspector.Editors;
using VirtueSky.Inspector.Elements;
using VirtueSky.Inspector.Utilities;
using VirtueSky.InspectorUnityInternalBridge;

[assembly: RegisterTriValueDrawer(typeof(CustomBuiltInDrawer), TriDrawerOrder.Fallback - 999)]

namespace VirtueSky.Inspector.Drawers
{
    public class CustomBuiltInDrawer : TriValueDrawer<object>
    {
        public override TriElement CreateElement(TriValue<object> propertyValue, TriElement next)
        {
            var property = propertyValue.Property;

            if (property.TryGetSerializedProperty(out var serializedProperty))
            {
                var handler = ScriptAttributeUtilityProxy.GetHandler(serializedProperty);

                var drawWithHandler = handler.hasPropertyDrawer ||
                                      property.PropertyType == TriPropertyType.Primitive ||
                                      TriUnityInspectorUtilities.MustDrawWithUnity(property);

                if (drawWithHandler)
                {
                    if (property.TryGetAttribute(out DrawWithUnityAttribute withUnityAttribute) &&
                        withUnityAttribute.WithUiToolkit)
                    {
                        handler.SetPreferredLabel(property.DisplayName);

                        var visualElement = handler.CreatePropertyGUI(serializedProperty);

                        if (visualElement != null &&
                            TriEditorCore.UiElementsRoots.TryGetValue(property.PropertyTree, out var rootElement))
                        {
                            return new TriUiToolkitPropertyElement(property, serializedProperty,
                                visualElement, rootElement);
                        }
                    }

                    return new TriBuiltInPropertyElement(property, serializedProperty, handler);
                }
            }

            return base.CreateElement(propertyValue, next);
        }
    }
}