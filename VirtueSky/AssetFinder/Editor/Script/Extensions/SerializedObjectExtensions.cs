using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class SerializedObjectExtensions
    {
        internal static IEnumerable<UnityObject> GetAllObjectReferences(this SerializedObject serializedObject)
        {
            SerializedProperty iterator = serializedObject.GetIterator().Copy();
            while (iterator.NextVisible(true))
            {
                if (iterator.propertyType != SerializedPropertyType.ObjectReference) continue;
                if (iterator.objectReferenceValue == null) continue;
                
                yield return iterator.objectReferenceValue;
            }
        }
        
        internal static IEnumerable<UnityObject> GetAllObjectReferences(this Component component)
        {
            if (component == null) yield break;
            
            var serializedObject = new SerializedObject(component);
            foreach (var obj in serializedObject.GetAllObjectReferences())
            {
                yield return obj;
            }
        }
        
        internal static IEnumerable<UnityObject> GetAllObjectReferences(this GameObject gameObject)
        {
            if (gameObject == null) yield break;
            
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                foreach (var obj in component.GetAllObjectReferences())
                {
                    yield return obj;
                }
            }
        }
        
        internal static SerializedProperty[] GetAllProperties(this SerializedObject serializedObject, bool processArrays = false)
        {
            serializedObject.Update();
            var result = new List<SerializedProperty>();
            
            SerializedProperty iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                SerializedProperty copy = iterator.Copy();
                
                if (processArrays && iterator.isArray)
                {
                    result.AddRange(GetArrayProperties(copy));
                }
                else
                {
                    result.Add(copy);
                }
            }
            
            return result.ToArray();
        }
        
        private static List<SerializedProperty> GetArrayProperties(SerializedProperty arrayProperty)
        {
            var result = new List<SerializedProperty>();
            int size = arrayProperty.arraySize;
            
            for (int i = 0; i < size; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                
                if (element.isArray)
                {
                    result.AddRange(GetArrayProperties(element.Copy()));
                }
                else
                {
                    result.Add(element.Copy());
                }
            }
            
            return result;
        }
    }
} 