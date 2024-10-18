using System;
using UnityEngine;

namespace VirtueSky.Core
{
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool enabled;
        [SerializeField] private T value;

        public bool Enabled => enabled;
        public T Value => value;

        public Optional(T value)
            : this()
        {
            enabled = true;
            this.value = value;
        }

        public Optional(bool enabled, T value)
        {
            this.enabled = enabled;
            this.value = value;
        }

        public static implicit operator Optional<T>(T v)
        {
            return new Optional<T>(v);
        }

        public static implicit operator T(Optional<T> o)
        {
            return o.Value;
        }

        public static implicit operator bool(Optional<T> o)
        {
            return o.enabled;
        }

        public static bool operator ==(Optional<T> lhs, Optional<T> rhs)
        {
            if (lhs.Value is null) return rhs.Value is null;
            return lhs.Value.Equals(rhs.Value);
        }

        public static bool operator !=(Optional<T> lhs, Optional<T> rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (Value is null) return obj is null;
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}