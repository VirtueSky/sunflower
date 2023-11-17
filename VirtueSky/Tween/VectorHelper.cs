namespace VirtueSky.Tween
{
    using UnityEngine;
    using System;


    public static class VectorHelper
    {
        public static Vector2 EaseVector2(Func<float, float, float, float> easingFunction, Vector2 from, Vector2 to, float t)
        {
            float newX = easingFunction(from.x, to.x, t);
            float newY = easingFunction(from.y, to.y, t);

            return new Vector2(newX, newY);
        }

        public static Vector3 EaseVector3(Func<float, float, float, float> easingFunction, Vector3 from, Vector3 to, float t)
        {
            float newX = easingFunction(from.x, to.x, t);
            float newY = easingFunction(from.y, to.y, t);
            float newZ = easingFunction(from.z, to.z, t);

            return new Vector3(newX, newY, newZ);
        }

        public static Quaternion EaseQuaternion(Func<float, float, float, float> easingFunction, Quaternion from, Quaternion to, float t)
        {
            float newX = easingFunction(from.x, to.x, t);
            float newY = easingFunction(from.y, to.y, t);
            float newZ = easingFunction(from.z, to.z, t);
            float newW = easingFunction(from.w, to.w, t);

            return new Quaternion(newX, newY, newZ, newW);
        }

        public static Color EaseColor(Func<float, float, float, float> easingFunction, Color from, Color to, float t)
        {
            float newR = easingFunction(from.r, to.r, t);
            float newG = easingFunction(from.g, to.g, t);
            float newB = easingFunction(from.b, to.b, t);
            float newA = easingFunction(from.a, to.a, t);

            return new Color(newR, newG, newB, newA);
        }
    }
}