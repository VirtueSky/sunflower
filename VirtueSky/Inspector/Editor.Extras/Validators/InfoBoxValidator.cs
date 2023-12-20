using VirtueSky.Inspector;
using VirtueSky.Inspector.Resolvers;
using VirtueSky.Inspector.Validators;

[assembly: RegisterTriAttributeValidator(typeof(InfoBoxValidator))]

namespace VirtueSky.Inspector.Validators
{
    public class InfoBoxValidator : TriAttributeValidator<InfoBoxAttribute>
    {
        private ValueResolver<string> _resolver;
        private ValueResolver<bool> _visibleIfResolver;

        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            _resolver = ValueResolver.ResolveString(propertyDefinition, Attribute.Text);
            _visibleIfResolver = Attribute.VisibleIf != null
                ? ValueResolver.Resolve<bool>(propertyDefinition, Attribute.VisibleIf)
                : null;

            if (ValueResolver.TryGetErrorString(_resolver, _visibleIfResolver, out var error))
            {
                return error;
            }

            return TriExtensionInitializationResult.Ok;
        }

        public override TriValidationResult Validate(TriProperty property)
        {
            if (_visibleIfResolver != null && !_visibleIfResolver.GetValue(property))
            {
                return TriValidationResult.Valid;
            }

            var message = _resolver.GetValue(property, "");
            return new TriValidationResult(false, message, Attribute.MessageType);
        }
    }
}