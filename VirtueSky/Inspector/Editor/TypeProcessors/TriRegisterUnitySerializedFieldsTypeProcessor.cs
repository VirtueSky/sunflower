using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VirtueSky.Inspector;
using VirtueSky.Inspector.TypeProcessors;
using VirtueSky.Inspector.Utilities;

[assembly: RegisterTriTypeProcessor(typeof(TriRegisterUnitySerializedFieldsTypeProcessor), 0)]

namespace VirtueSky.Inspector.TypeProcessors
{
    public class TriRegisterUnitySerializedFieldsTypeProcessor : TriTypeProcessor
    {
        public override void ProcessType(Type type, List<TriPropertyDefinition> properties)
        {
            const int fieldsOffset = 1;

            properties.AddRange(TriReflectionUtilities
                .GetAllInstanceFieldsInDeclarationOrder(type)
                .Where(IsSerialized)
                .Select((it, ind) => TriPropertyDefinition.CreateForFieldInfo(ind + fieldsOffset, it)));
        }

        private static bool IsSerialized(FieldInfo fieldInfo)
        {
            return TriUnitySerializationUtilities.IsSerializableByUnity(fieldInfo);
        }
    }
}