using System;
using System.Reflection;

namespace VirtueSky.Utils
{
    public static class ReflectionUtils
    {
        public static FieldInfo GetFieldRecursive(this Type type, string fieldName, BindingFlags bindingFlags)
        {
            var t = type;
            FieldInfo field = null;
            while (t != null)
            {
                field = t.GetField(fieldName, bindingFlags);
                if (field != null) break;
                t = t.BaseType;
            }

            return field;
        }
    }
}