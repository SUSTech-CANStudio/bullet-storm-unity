﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CANStudio.BulletStorm.Emission
{
    /// <summary>
    /// Shape is a list of <see cref="BulletEmitParam"/>s.
    /// </summary>
    /// In shoot'em up games, enemy's bullets usually arranged as some beautiful patterns.
    /// This class provides some basic patterns like ring, line, sphere... and provides functions to transform
    /// them. You can use operator '+' to combine two <see cref="Shape"/>s.
    [Serializable]
    public class Shape : IReadOnlyList<BulletEmitParam>
    {
        [SerializeField]
        private List<BulletEmitParam> emitParams;

        /// <summary>
        /// Total bullets count in the shape.
        /// </summary>
        public int Count => emitParams?.Count ?? 0;

        /// <summary>
        /// Creates an empty shape.
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

        /// <summary>
        /// Copies a shape.
        /// </summary>
        /// <param name="shape">The original shape</param>
        public Shape(Shape shape)
        {
            emitParams = new List<BulletEmitParam>(shape);
        }
        
        private Shape([NotNull] List<BulletEmitParam> emitParams)
        {
            this.emitParams = emitParams;
        }

        /// <summary>
        /// Copies this shape.
        /// </summary>
        /// <returns></returns>
        public Shape Copy() => new Shape(this);

        // Shape generators, static functions that create a basic shape.
        #region Generator

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
                var lat = Mathf.Asin(-1.0f + 2.0f * i / num);
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
        /// <param name="seed">Seed to generate random positions.</param>
        /// <returns></returns>
        public static Shape RandomSphere(int num, float radius, int seed = 0)
        {
            var lastState = Random.state;
            Random.InitState(seed);
            
            var list = new List<BulletEmitParam>(num);
            for (var i = 0; i < num; i++)
            {
                var point = Random.onUnitSphere;
                list.Add(new BulletEmitParam(point * radius));
            }

            Random.state = lastState;
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
            
            var deltaLength = 1f / (num - 1);
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

        #endregion

        // Operations that modifies bullet positions.
        #region Position operations

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

        #endregion

        // Operations that modifies bullet velocities.
        #region Velocity operations

        /// <summary>
        /// Set velocity to all bullets.
        /// </summary>
        /// <param name="velocity">Given velocity.</param>
        /// <returns></returns>
        public Shape SetVelocity(Vector3 velocity)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.velocity = velocity;
                emitParams[i] = emitParam;
            });
            return this;
        }
        
        /// <summary>
        /// Add velocity to all bullets.
        /// </summary>
        /// <param name="velocity">Velocity increment.</param>
        /// <returns></returns>
        public Shape AddVelocity(Vector3 velocity)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.velocity += velocity;
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Set speed to all bullets, direction is original direction.
        /// <para/>
        /// This requires bullets to have speed formally, if original speed is 0,
        /// this function won't change the speed.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public Shape SetSpeed(float speed)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                if (!Mathf.Approximately(emitParam.velocity.magnitude, 0f))
                {
                    emitParam.velocity = emitParam.velocity.normalized * speed;
                }
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Add speed to all bullets, direction is from origin to the bullet.
        /// </summary>
        /// <param name="speed">Speed increment.</param>
        /// <returns></returns>
        public Shape AddSpeed(float speed)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.velocity += emitParam.position.normalized * speed;
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Add speed to bullets by their indexes, direction is from origin to the bullet.
        /// </summary>
        /// <param name="curve">X-axis 0~1 represents all indexes, y-axis is speed.</param>
        /// <param name="multiplier">Multiplier for curve y-axis.</param>
        /// <returns></returns>
        public Shape AddSpeedByIndex(AnimationCurve curve, float multiplier)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.velocity += emitParam.position.normalized * (curve.Evaluate((float) i / Count) * multiplier);
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Add speed to all bullets, direction is from bullet to target.
        /// </summary>
        /// <param name="speed">Speed increment.</param>
        /// <param name="target">Target position.</param>
        /// <returns></returns>
        public Shape AddSpeedTowards(float speed, Vector3 target)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.velocity += (target - emitParam.position).normalized * speed;
                emitParams[i] = emitParam;
            });
            return this;
        }
        
        #endregion

        // Operations that modifies bullet colors.
        #region Color operations

        /// <summary>
        /// Set color for all bullets.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Shape SetColor(Color color)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.color = color;
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Set bullet color according to bullet index.
        /// </summary>
        /// <param name="gradient">Color gradient.</param>
        /// <returns></returns>
        public Shape SetColorByIndex(Gradient gradient)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.color = gradient.Evaluate((float)i / Count);
                emitParams[i] = emitParam;
            });
            return this;
        }

        #endregion

        // Operations that modifies bullet sizes.
        #region Size operations

        /// <summary>
        /// Set size for all bullets.
        /// </summary>
        /// <param name="size">Bullet size.</param>
        /// <returns></returns>
        public Shape SetSize(Vector3 size)
        {
            Parallel.For(0, Count, i =>
            {
                var emitParam = emitParams[i];
                emitParam.size = size;
                emitParams[i] = emitParam;
            });
            return this;
        }

        /// <summary>
        /// Set size for all bullets.
        /// </summary>
        /// <param name="x">X scale.</param>
        /// <param name="y">Y scale.</param>
        /// <param name="z">z scale.</param>
        /// <returns></returns>
        public Shape SetSize(float x, float y, float z) => SetSize(new Vector3(x, y, z));

        /// <summary>
        /// Set size for all bullets.
        /// </summary>
        /// <param name="size">Bullet size.</param>
        /// <returns></returns>
        public Shape SetSize(float size) => SetSize(Vector3.one * size);
        
        #endregion
        
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

        public IEnumerator<BulletEmitParam> GetEnumerator()
        {
            return emitParams.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) emitParams).GetEnumerator();
        }

        public BulletEmitParam this[int index] => emitParams[index];
    }
}