using VirtueSky.Inspector;
using VirtueSky.Inspector.Validators;
using UnityEditor;
using UnityEngine;

[assembly: RegisterTriAttributeValidator(typeof(AssetsOnlyValidator))]

namespace VirtueSky.Inspector.Validators
{
    public class AssetsOnlyValidator : TriAttributeValidator<AssetsOnlyAttribute>
    {
        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            if (!typeof(Object).IsAssignableFrom(propertyDefinition.FieldType))
            {
                return "AssetsOnly attribute can be used only on Object fields";
            }

            return TriExtensionInitializationResult.Ok;
        }

        public override TriValidationResult Validate(TriProperty property)
        {
            var obj = property.TryGetSerializedProperty(out var serializedProperty)
                ? serializedProperty.objectReferenceValue
                : (Object) property.Value;

            if (obj == null || AssetDatabase.Contains(obj))
            {
                return TriValidationResult.Valid;
            }

            return TriValidationResult.Error($"{obj} is not as asset.");
        }
    }
}