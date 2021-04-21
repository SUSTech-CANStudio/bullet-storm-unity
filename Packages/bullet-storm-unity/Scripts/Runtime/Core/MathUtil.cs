using System;
using System.Numerics;

namespace CANStudio.BulletStorm.Core
{
    internal static class MathUtil
    {
        public const float Rad2Deg = 180f / (float) Math.PI;

        public const float Deg2Rad = (float) Math.PI / 180f;

        /// <summary>
        ///     Angle in degree between two vectors.
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static float AngleBetween(Vector3 vec1, Vector3 vec2)
        {
            var topPart = Vector3.Dot(vec1, vec2);
            var bottomPart = (float) Math.Sqrt(Vector3.Dot(vec1, vec1) * Vector3.Dot(vec2, vec2));
            return (float) Math.Acos(topPart / bottomPart) * Rad2Deg;
        }
    }
}