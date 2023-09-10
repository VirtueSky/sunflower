using UnityEngine;
using Newtonsoft.Json;

namespace VirtueSky.DataType
{
    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct SVector2
    {
        [JsonProperty] public float x;
        [JsonProperty] public float y;

        public SVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public SVector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public static implicit operator Vector2(SVector2 v) => new Vector2(v.x, v.y);
        public static explicit operator SVector2(Vector2 v) => new SVector2(v);
    }
}