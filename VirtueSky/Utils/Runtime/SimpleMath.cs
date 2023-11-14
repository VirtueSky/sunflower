using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VirtueSky.Utils
{
    public static class SimpleMath
    {
        public static bool InRange(Vector3 p, Vector3 c, float r, out float sqrDst, bool compareY = false)
        {
            if (!compareY) p.y = c.y;
            sqrDst = SqrDist(p, c);
            return sqrDst <= r * r;
        }

        public static bool InRange(Vector3 p, Vector3 c, float r, bool compareY = false)
        {
            if (!compareY) p.y = c.y;
            var sqrDst = SqrDist(p, c);
            return sqrDst <= r * r;
        }

        public static bool InRange2D(Vector2 p, Vector2 c, float r)
        {
            var sqrDst = SqrDist(p, c);
            return sqrDst <= r * r;
        }

        public static float SqrDist(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        public static Quaternion RandomRotation(bool onlyY = false)
        {
            if (onlyY) return Quaternion.Euler(0, Random.value * 360f, 0);
            return Quaternion.Euler(RandomVector3() * 180f);
        }

        public static Quaternion GetRotationXZ(Vector3 a, Vector3 b)
        {
            var dir = b - a;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                return Quaternion.LookRotation(dir);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public static Quaternion AngVelToDeriv(Quaternion current, Vector3 angVel)
        {
            var spin = new Quaternion(angVel.x, angVel.y, angVel.z, 0f);
            var result = spin * current;
            return new Quaternion(0.5f * result.x, 0.5f * result.y, 0.5f * result.z, 0.5f * result.w);
        }

        public static Vector3 DerivToAngVel(Quaternion current, Quaternion deriv)
        {
            var result = deriv * Quaternion.Inverse(current);
            return new Vector3(2f * result.x, 2f * result.y, 2f * result.z);
        }

        public static Quaternion IntegrateRotation(Quaternion rotation, Vector3 angularVelocity, float deltaTime)
        {
            if (deltaTime < Mathf.Epsilon) return rotation;
            var deriv = AngVelToDeriv(rotation, angularVelocity);
            var pred = new Vector4(
                rotation.x + deriv.x * deltaTime,
                rotation.y + deriv.y * deltaTime,
                rotation.z + deriv.z * deltaTime,
                rotation.w + deriv.w * deltaTime
            ).normalized;
            return new Quaternion(pred.x, pred.y, pred.z, pred.w);
        }

        public static Quaternion QuaternionSmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv,
            float time)
        {
            if (Time.deltaTime < Mathf.Epsilon) return rot;
            // account for double-cover
            var dot = Quaternion.Dot(rot, target);
            var multi = dot > 0f ? 1f : -1f;
            target.x *= multi;
            target.y *= multi;
            target.z *= multi;
            target.w *= multi;
            // smooth damp (nlerp approx)
            var result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;

            // ensure deriv is tangent
            var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;

            return new Quaternion(result.x, result.y, result.z, result.w);
        }

        public static Vector3 RandomVector3(bool zeroY = false)
        {
            var result = Random.insideUnitSphere;
            if (zeroY) result.y = 0;
            return result;
        }

        public static int GetNearestIndex(Vector3 p, float r, Vector3[] list, bool compareY = false)
        {
            var minDist = Mathf.Infinity;
            var index = -1;
            var dist = 0f;
            for (var i = 0; i < list.Length; i++)
            {
                if (InRange(p, list[i], r, out dist, compareY))
                {
                    if (dist <= minDist)
                    {
                        minDist = dist;
                        index = i;
                    }
                }
            }

            return index;
        }

        public static Vector3 RandomBetween(Vector3 a, Vector3 b)
        {
            return a + (b - a).normalized * Random.value * (b - a).magnitude;
        }

        public static string NewGuid()
        {
            var encoded = Convert.ToBase64String(System.Guid.NewGuid().ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
        }


        public static Vector3 DirectionFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public static T[] SelectRandomFromArray<T>(T[] items, int count)
        {
            if (items.Length < count)
            {
                return items;
            }

            var list = items.ToList();
            var result = new T[count];
            while (list.Count > 0 && count > 0)
            {
                count--;
                result[count] = list[Random.Range(0, list.Count)];
                list.Remove(result[count]);
            }

            return result;
        }

        public static Vector3[] GetCirclePoint(Vector3 center, float radius, float step = 0.1f)
        {
            var points = new List<Vector3>();
            var theta = 0f;
            var x = radius * Mathf.Cos(theta);
            var y = radius * Mathf.Sin(theta);
            points.Add(center + new Vector3(x, 0, y));
            for (theta = step; theta < Mathf.PI * 2; theta += step)
            {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                points.Add(center + new Vector3(x, 0, y));
            }

            return points.ToArray();
        }

        public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec,
            Vector3 planeNormal, Vector3 planePoint)
        {
            float length;
            float dotNumerator;
            float dotDenominator;
            Vector3 vector;
            intersection = Vector3.zero;

            //calculate the distance between the linePoint and the line-plane intersection point
            dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
            dotDenominator = Vector3.Dot(lineVec, planeNormal);

            //line and plane are not parallel
            if (dotDenominator != 0.0f)
            {
                length = dotNumerator / dotDenominator;

                //create a vector from the linePoint to the intersection point
                vector = SetVectorLength(lineVec, length);

                //get the coordinates of the line-plane intersection point
                intersection = linePoint + vector;

                return true;
            }
            //output not valid
            else
            {
                return false;
            }
        }

        public static bool AreLineSegmentsCrossing(Vector3 pointA1, Vector3 pointA2, Vector3 pointB1, Vector3 pointB2)
        {
            Vector3 closestPointA;
            Vector3 closestPointB;
            int sideA;
            int sideB;

            Vector3 lineVecA = pointA2 - pointA1;
            Vector3 lineVecB = pointB2 - pointB1;

            bool valid = ClosestPointsOnTwoLines(out closestPointA, out closestPointB, pointA1, lineVecA.normalized,
                pointB1, lineVecB.normalized);

            //lines are not parallel
            if (valid)
            {
                sideA = PointOnWhichSideOfLineSegment(pointA1, pointA2, closestPointA);
                sideB = PointOnWhichSideOfLineSegment(pointB1, pointB2, closestPointB);

                if ((sideA == 0) && (sideB == 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2,
            Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            closestPointLine1 = Vector3.zero;
            closestPointLine2 = Vector3.zero;

            float a = Vector3.Dot(lineVec1, lineVec1);
            float b = Vector3.Dot(lineVec1, lineVec2);
            float e = Vector3.Dot(lineVec2, lineVec2);

            float d = a * e - b * b;

            //lines are not parallel
            if (d != 0.0f)
            {
                Vector3 r = linePoint1 - linePoint2;
                float c = Vector3.Dot(lineVec1, r);
                float f = Vector3.Dot(lineVec2, r);

                float s = (b * f - c * e) / d;
                float t = (a * f - c * b) / d;

                closestPointLine1 = linePoint1 + lineVec1 * s;
                closestPointLine2 = linePoint2 + lineVec2 * t;

                return true;
            }

            else
            {
                return false;
            }
        }

        public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
        {
            Vector3 lineVec = linePoint2 - linePoint1;
            Vector3 pointVec = point - linePoint1;

            float dot = Vector3.Dot(pointVec, lineVec);

            //point is on side of linePoint2, compared to linePoint1
            if (dot > 0)
            {
                //point is on the line segment
                if (pointVec.magnitude <= lineVec.magnitude)
                {
                    return 0;
                }

                //point is not on the line segment and it is on the side of linePoint2
                else
                {
                    return 2;
                }
            }

            //Point is not on side of linePoint2, compared to linePoint1.
            //Point is not on the line segment and it is on the side of linePoint1.
            else
            {
                return 1;
            }
        }

        public static Vector3 SetVectorLength(Vector3 vector, float size)
        {
            //normalize the vector
            var vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return vectorNormalized *= size;
        }

        public static Color DotColor(Color a, Color b)
        {
            var vA = new Vector3(a.r, a.g, a.b);
            var vB = new Vector3(b.r, b.g, b.b);
            var vC = Vector3.Dot(vA, vB);
            return new Color(vC, vC, vC);
        }

        public static T GetRandomWithWeight<T>(T[] items, int[] weights)
        {
            var totalWeight = weights.Sum();
            var rd = Random.Range(0, totalWeight);
            var sumWeight = 0;
            var result = items[0];
            for (var i = 0; i < items.Length; i++)
            {
                sumWeight += weights[i];
                if (rd < sumWeight)
                {
                    result = items[i];
                    break;
                }
            }

            return result;
        }
    }
}