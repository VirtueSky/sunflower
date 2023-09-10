using Newtonsoft.Json;
using UnityEngine;

namespace VirtueSky.DataType
{
    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public struct SVector3
    {
        [Newtonsoft.Json.JsonProperty] public float x;
        [Newtonsoft.Json.JsonProperty] public float y;
        [Newtonsoft.Json.JsonProperty] public float z;

        public SVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static implicit operator Vector3(SVector3 v) => new Vector3(v.x, v.y, v.z);
        public static explicit operator SVector3(Vector3 v) => new SVector3(v);
    }
}