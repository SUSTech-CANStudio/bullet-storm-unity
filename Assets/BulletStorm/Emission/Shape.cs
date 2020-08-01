using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletStorm.Emission
{
    /// <summary>
    /// Shape is a list of <see cref="BulletEmitParam"/>s.
    /// </summary>
    /// In shoot'em up games, enemy's bullets usually arranged as some beautiful patterns.
    /// This class provides some basic patterns like ring, line, sphere... and provides functions to transform
    /// them. You can use operator '+' to combine two <see cref="Shape"/>s, and call <see cref="AsReadOnly"/>
    /// to get the list.
    public class Shape : ScriptableObject
    {
        [SerializeField]
        private List<BulletEmitParam> emitParams;

        /// <summary>
        /// Total bullets count in the shape.
        /// </summary>
        public int Count => emitParams.Count;
        
        /// <summary>
        /// Create an empty shape.
        /// </summary>
        /// <param name="num">Number of bullets in the shape.</param>
        public Shape(int num)
        {
            emitParams = new List<BulletEmitParam>(num);
            for (var i = 0; i < num; i++)
            {
                emitParams.Add(new BulletEmitParam(Vector3.zero));
            }
        }
        
        private Shape([NotNull] List<BulletEmitParam> emitParams)
        {
            this.emitParams = emitParams;
        }

        public IReadOnlyList<BulletEmitParam> AsReadOnly() => emitParams.AsReadOnly();
        
        /// <summary>
        /// Use Fibonacci sphere algorithm to generate an roughly equal-distance point sphere.
        /// </summary>
        /// <param name="num">Number of bullets</param>
        /// <param name="radius">Radius of the sphere</param>
        public static Shape FibonacciSphere(int num, float radius)
        {
            const float ga = 2.39996322972865332f;  // golden angle = 2.39996322972865332
            
            var list = new List<BulletEmitParam>(num);
            
            for (var i = 0; i < num; i++)
            {
                var lat = Mathf.Asin(-1.0f + 2.0f * i / (num + 1));
                var lon = ga * i;

                var point = new Vector3(
                    Mathf.Cos(lon) * Mathf.Cos(lat),
                    Mathf.Sin(lon) * Mathf.Cos(lat),
                    Mathf.Sin(lat));

                list.Add(new BulletEmitParam(point * radius));
            }
            
            return new Shape(list);
        }

        /// <summary>
        /// Use random value to generate a sphere.
        /// </summary>
        /// <param name="num">Number of bullets</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <returns></returns>
        public static Shape RandomSphere(int num, float radius)
        {
            var list = new List<BulletEmitParam>(num);
            for (var i = 0; i < num; i++)
            {
                var point = Random.onUnitSphere;
                list.Add(new BulletEmitParam(point * radius));
            }

            return new Shape(list);
        }

        /// <summary>
        /// A ring on the z-x plane, positive z-axis is the first bullet, and rotates to positive x-axis.
        /// </summary>
        /// <param name="num">Number of bullets</param>
        /// <param name="radius">Radius of the ring</param>
        /// <returns></returns>
        public static Shape Ring(int num, float radius)
        {
            var list = new List<BulletEmitParam>(num);
            if (num <= 0) return new Shape(list);
            
            var deltaAngle = 2 * Mathf.PI / num;
            for (var i = 0; i < num; i++)
            {
                var angle = deltaAngle * i;
                var point = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                list.Add(new BulletEmitParam(point * radius));
            }

            return new Shape(list);
        }

        /// <summary>
        /// A line on x-axis, from left to right, origin is the middle point.
        /// </summary>
        /// <param name="num">Number of bullets</param>
        /// <param name="length">Length of the line</param>
        /// <returns></returns>
        public static Shape Line(int num, float length)
        {
            var list = new List<BulletEmitParam>(num);
            if (num <= 0) return new Shape(list);
            if (num == 1)
            {
                list.Add(new BulletEmitParam(Vector3.zero));
                return new Shape(list);
            }
            
            var deltaLength = 1 / (num - 1);
            for (var i = 0; i < num; i++)
            {
                var point = new Vector3(deltaLength * i - 0.5f, 0, 0);
                list.Add(new BulletEmitParam(point * length));
            }

            return new Shape(list);
        }

        /// <summary>
        /// An arc on z-x plane, from left to right, middle point on positive z-axis.
        /// </summary>
        /// <param name="num">Number of bullets</param>
        /// <param name="angle">From to angle of the arc in degree</param>
        /// <param name="radius">Radius of the arc</param>
        /// <returns></returns>
        public static Shape Arc(int num, float angle, float radius)
        {
            var list = new List<BulletEmitParam>();
            if (num <= 0) return new Shape(list);
            if (num == 1)
            {
                list.Add(new BulletEmitParam(Vector3.forward * radius));
                return new Shape(list);
            }

            var deltaAngle = angle * Mathf.Deg2Rad / (num - 1);
            var startAngle = -angle * Mathf.Deg2Rad / 2;
            for (var i = 0; i < num; i++)
            {
                var currentAngle = startAngle + deltaAngle * i;
                var point = new Vector3(Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle));
                list.Add(new BulletEmitParam(point * radius));
            }

            return new Shape(list);
        }

        /// <summary>
        /// Rotates the whole shape around a point.
        /// </summary>
        /// <param name="point">Center point of the rotation</param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Shape RotateAround(Vector3 point, Quaternion rotation)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.position = point + rotation * (emitParam.position - point);
                emitParam.velocity = rotation * emitParam.velocity;
                emitParams[i] = emitParam;
            });
            return this;
        }

        public Shape RotateAround(Vector3 point, float angle, Vector3 axis) =>
            RotateAround(point, Quaternion.AngleAxis(angle, axis));

        /// <summary>
        /// Rotates the whole shape.
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Shape Rotate(Quaternion rotation) => RotateAround(Vector3.zero, rotation);

        public Shape Rotate(float angle, Vector3 axis) => Rotate(Quaternion.AngleAxis(angle, axis));

        public Shape Rotate(Vector3 euler) => Rotate(Quaternion.Euler(euler));

        public Shape Rotate(float xAngle, float yAngle, float zAngle) =>
            Rotate(Quaternion.Euler(xAngle, yAngle, zAngle));

        /// <summary>
        /// Moves the whole shape.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Shape Move(Vector3 offset)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.position += offset;
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Sorts bullets in the shape.
        /// </summary>
        /// You mainly call this because you want to use some index-dependent functions later.
        /// <param name="comparer">Sorting method</param>
        /// <returns></returns>
        public Shape Sort(ParamComparer comparer)
        {
            emitParams.Sort(comparer);
            return this;
        }

        /// <summary>
        /// Adds two shapes together.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Shape operator +(Shape first, Shape second)
        {
            var list = new List<BulletEmitParam>(first.emitParams);
            list.AddRange(second.emitParams);
            return new Shape(list);
        }
    }
}