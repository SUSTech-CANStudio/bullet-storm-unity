using System;
using UnityEngine;

namespace CANStudio.BulletStorm.Emission
{
    [Serializable]
    public struct BulletEmitParam
    {
        [Tooltip("Start position relative to emitter.")]
        public Vector3 position;

        [Tooltip("Start velocity.")] public Vector3 velocity;

        [Tooltip("Color of the bullet, Color.clear (0, 0, 0, 0) will be override by bullet system default settings.")]
        public Color color;

        [Tooltip("Size of the bullet, Vector3.zero (0, 0, 0) will be override by bullet system default settings.")]
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
        ///     Is color a default value?
        /// </summary>
        public bool DefaultColor => color == Color.clear;

        /// <summary>
        ///     Is size a default value?
        /// </summary>
        public bool DefaultSize => size.x == 0 || size.y == 0 || size.z == 0;

        /// <summary>
        ///     Get relative parameters of a transform.
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