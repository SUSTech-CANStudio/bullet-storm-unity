using System.Linq;
using UnityEngine;

namespace CANStudio.BulletStorm.Util
{
    internal static class Helpers
    {
        private const int Accuracy = 8;
        
        /// <summary>
        ///     Returns an approximate zero vector, but contains the original direction.
        ///     To recover the vector for calculation, call <see cref="Normalized"/>
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3 Minimized(this in Vector3 vector3)
        {
            var values = new IeeeFloat[] {vector3.x, vector3.y, vector3.z};
            
            var maxExp = byte.MinValue;
            var minExp = byte.MaxValue;
            for (var i = 0; i < 3; i++)
            {
                var exp = values[i].Exponent;
                if (exp > maxExp) maxExp = exp;
                if (exp < minExp && exp >= maxExp - Accuracy) minExp = exp;
            }
            
            for (var i = 0; i < 3; i++)
            {
                if (values[i].Exponent < minExp) values[i] = 0;
                else values[i].Exponent -= minExp;
            }
            return new Vector3(values[0], values[1], values[2]);
        }

        /// <summary>
        ///     Returns the normalized vector of a minimized vector.
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3 Normalized(this in Vector3 vector3)
        {
            var values = new IeeeFloat[] {vector3.x, vector3.y, vector3.z};
            var maxExp = values.Aggregate(byte.MinValue, (b, f) => f.Exponent > b ? f.Exponent : b);
            var grow = 127 - maxExp;
            if (grow <= 0) return new Vector3(values[0], values[1], values[2]).normalized;
            for (var i = 0; i < 3; i++)
            {
                if (values[i].Equals(IeeeFloat.Zero)) continue;
                values[i].Exponent += (byte)grow;
            }
            return new Vector3(values[0], values[1], values[2]).normalized;
        }

        /// <summary>
        ///     Returns a vector that magnitude is changed but direction not,
        ///     even if magnitude is set to zero.
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="value">The magnitude (negative magnitude will still inverse the direction, so please avoid using negative magnitude if you don't want).</param>
        /// <returns></returns>
        public static Vector3 SafeChangeMagnitude(this in Vector3 vector3, float value)
        {
            var calculated = vector3.Normalized() * value;
            
            var maxExp = byte.MinValue;
            foreach (var f in new IeeeFloat[] {calculated.x, calculated.y, calculated.z})
            {
                var exp = f.Exponent;
                if (exp > maxExp) maxExp = exp;
            }

            return maxExp < Accuracy ? vector3.Minimized() : calculated;
        }
    }
}