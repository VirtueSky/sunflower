using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.Inspector
{
    public static class UtilityDraw
    {
        /// <summary>
        /// Return if the object is enum and not null
        /// </summary>
        public static bool IsEnum(object obj)
        {
            return obj != null && obj.GetType().IsEnum;
        }

        /// <summary>
        /// Return if all the objects are enums and not null
        /// </summary>
        public static bool IsEnum(object[] obj)
        {
            return obj != null && obj.All(o => o.GetType().IsEnum);
        }

        /// <summary>
        /// Check if the field with name "fieldName" has the same class as the "checkTypes" classes through reflection
        /// </summary>
        public static bool CheckSameEnumType(IEnumerable<Type> checkTypes, Type classType, string fieldName)
        {
            string[] fieldNames = fieldName.Split('.');
            Type currentType = classType;

            foreach (var name in fieldNames)
            {
                FieldInfo field = currentType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                PropertyInfo property = currentType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null)
                {
                    currentType = field.FieldType;
                }
                else if (property != null)
                {
                    currentType = property.PropertyType;
                }
                else
                {
                    return false;
                }
            }

            return checkTypes.All(x => x == currentType);
        }

        /// <summary>
        /// Return the float value in the content string removing the remove string
        /// </summary>
        public static float? GetValue(string content, string remove)
        {
            string removed = content.Replace(remove, "");
            try
            {
                return float.Parse(removed);
            }
            catch
            {
                return null;
            }
        }

        public static void CreateLineSpacer(Rect _rect, Color _color, float _height = 2)
        {
            _rect.height = _height;

            Color oldColour = GUI.color;

            GUI.color = _color;
            EditorGUI.DrawRect(_rect, _color);
            GUI.color = oldColour;
        }
    }
}