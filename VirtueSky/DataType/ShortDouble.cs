using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.DataType
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public partial struct ShortDouble : IFormattable, IComparable<ShortDouble>, IEquatable<ShortDouble>,
        IComparable
    {
        static Func<double, Unit> _unitFinder;

        static ShortDouble()
        {
            _unitFinder = Unit0.Find;
        }

        [SerializeField] double value;

        public ShortDouble(double value = 0)
        {
            this.value = value;
        }

        public double Value => value;

        // Implicit convert double to SecuredDouble
        public static implicit operator ShortDouble(double value) => new ShortDouble(value);

        // Implicit convert SecuredDouble to double
        public static implicit operator double(ShortDouble value) => value.Value;

        public static ShortDouble operator +(ShortDouble a, ShortDouble b) =>
            new ShortDouble(a.Value + b.Value);

        public static ShortDouble operator -(ShortDouble a, ShortDouble b) =>
            new ShortDouble(a.Value - b.Value);

        public static ShortDouble operator *(ShortDouble a, ShortDouble b) =>
            new ShortDouble(a.Value * b.Value);

        public static ShortDouble operator /(ShortDouble a, ShortDouble b) =>
            new ShortDouble(a.Value / b.Value);

        public static bool operator >(ShortDouble a, ShortDouble b) => a.Value > b.Value;
        public static bool operator >=(ShortDouble a, ShortDouble b) => a.Value >= b.Value;
        public static bool operator <(ShortDouble a, ShortDouble b) => a.Value < b.Value;
        public static bool operator <=(ShortDouble a, ShortDouble b) => a.Value <= b.Value;

        public ShortDouble Floor => new ShortDouble(Math.Floor(Value));
        public ShortDouble Ceiling => new ShortDouble(Math.Ceiling(Value));
        public ShortDouble Round => new ShortDouble(Math.Round(Value));

        public float AsFloat() => (float)Value;
        public long AsLong() => (long)Value;
        public bool AsBool(float eps = 0.3f) => Value > eps;
        public int AsInt() => (int)Value;
        public bool True => AsBool();

        public ShortDouble Pow(double p) => new ShortDouble(Math.Pow(Value, p));
        public static ShortDouble Max(ShortDouble a, ShortDouble b) => a > b ? a : b;
        public static ShortDouble Min(ShortDouble a, ShortDouble b) => a > b ? b : a;

        public static ShortDouble Clamp(ShortDouble value, ShortDouble min, ShortDouble max) =>
            value < min ? min : (value > max ? max : value);

        public int CompareTo(ShortDouble other) => Value.CompareTo(other.Value);
        public int CompareTo(object obj) => Value.CompareTo(obj);

        public bool Equals(ShortDouble other) => Value.Equals(other.Value);

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType()) return false;
            return Value.Equals(((ShortDouble)other).Value);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => ToString(FindUnit(Value), "0.#");
        public string ToString(string format) => ToString(FindUnit(Value), format);
        public string ToString(IFormatProvider provider) => Value.ToString(provider);
        public string ToString(string format, IFormatProvider provider) => ToString(FindUnit(Value), format, provider);

        string ToString(Unit unit, string format = "0.##")
        {
            if (double.IsInfinity(Value) || double.IsNaN(Value))
            {
                return "Infinity or NaN";
            }

            return (Value / System.Math.Pow(10, unit.exponent)).ToString(format) + unit.name;
        }

        string ToString(Unit unit, string format, IFormatProvider provider)
        {
            if (double.IsInfinity(Value) || double.IsNaN(Value))
            {
                return "Infinity or NaN";
            }

            return (Value / System.Math.Pow(10, unit.exponent)).ToString(format, provider) + unit.name;
        }

        public static void SetUnit(int u)
        {
            if (u == 0)
            {
                _unitFinder = Unit0.Find;
            }
            else if (u == 1)
            {
                _unitFinder = Unit1.Find;
            }
            else
            {
                _unitFinder = Unit2.Find;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShortDouble))]
    public class ShortDoubleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), GUIContent.none);
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif
}