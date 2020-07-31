using System;
using BulletStorm.BulletSystem;
using UnityEngine;

namespace BulletStorm.Emission
{
    public struct BulletEmitParam
    {
        public Vector3 position;
        public Vector3 velocity;
        public Color color;
        public Vector3 size;

        public BulletEmitParam(Vector3 position)
        {
            this.position = position;
            velocity = Vector3.zero;
            color = Color.clear;
            size = Vector3.zero;
        }
        
        public BulletEmitParam(Vector3 position, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            color = Color.clear;
            size = Vector3.zero;
        }
        
        public BulletEmitParam(Vector3 position, Vector3 velocity, Color color, Vector3 size)
        {
            this.position = position;
            this.velocity = velocity;
            this.color = color;
            this.size = size;
        }
        
        /// <summary>
        /// Get relative parameters of a transform.
        /// </summary>
        /// <param name="parent">The parent transform.</param>
        public BulletEmitParam RelativeTo(Transform parent)
        {
            var parentRotation = parent.rotation;
            if (!(MemberwiseClone() is BulletEmitParam result)) throw new NullReferenceException();
            result.position = parentRotation * position + parent.position;
            result.velocity = parentRotation * velocity;
            return result;
        }
    }
}