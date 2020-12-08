using System;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Util
{
    /// <summary>
    /// Describes a transform in different ways.
    /// </summary>
    [Serializable]
    public class Target
    {
        [SerializeField] private FindMethod findBy;
        [SerializeField] private Transform transform;
        [SerializeField] private string info;

        /// <summary>
        /// Get the transform inside. Will always be null before calling <see cref="Check"/>.
        /// </summary>
        public Transform AsTransform { get; private set; }
        
        /// <summary>
        /// Initiate and check if target is valid.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool Check()
        {
            if (AsTransform) return true;
            switch (findBy)
            {
                case FindMethod.Transform:
                    AsTransform = transform;
                    return AsTransform;
                case FindMethod.Name:
                    var go = GameObject.Find(info);
                    if (!go) return false;
                    else
                    {
                        AsTransform = go.transform;
                        return true;
                    }
                case FindMethod.Tag:
                    go = GameObject.FindWithTag(info);
                    if (!go) return false;
                    else
                    {
                        AsTransform = go.transform;
                        return true;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            var s = findBy == FindMethod.Transform
                ? transform ? transform.name : "None"
                : info;
            return $"Target{{{findBy.ToString().ToLower()}: {s}}}";
        }

        /// <summary>
        /// The transform value. Will always be null before calling <see cref="Check"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static implicit operator Transform(Target target) => target.AsTransform;

        /// <summary>
        /// True if transform not null. Always return false before calling <see cref="Check"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static implicit operator bool(Target target) => target.AsTransform;
        
        private enum FindMethod
        {
            [Tooltip("Set the transform directly.")]
            Transform = 0,
            [Tooltip("Use game object name in scene to find the transform.")]
            Name = 1,
            [Tooltip("Use tag to find the transform.")]
            Tag = 2
        }
    }
}