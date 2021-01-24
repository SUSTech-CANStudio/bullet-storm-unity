using System;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    public struct DeflectionModule
    {
        [Tooltip("Velocity deflection angle per second."), SerializeField]
        private Vector2 deflection;

        [Tooltip("Specific the space that deflection calculated in."), SerializeField]
        private Space space;

        [Tooltip("The target transform to be a space."), SerializeField, ShowIf(nameof(ShowTarget)), AllowNesting]
        private TargetWrapper target;

        [Space, Space] // fix the AllowNesting attribute's default behavior
        
        [Tooltip("The euler to describe rotation of a space."), SerializeField, ShowIf(nameof(ShowEuler)), AllowNesting]
        private Vector3 euler;

        private bool ShowEuler => space == Space.Fixed;
        private bool ShowTarget => space == Space.Dynamic;
        
        public void OnUpdate(IBulletController controller)
        {
            Quaternion rotation;

            var dEuler = new Vector3(-deflection.y * Time.deltaTime, deflection.x * Time.deltaTime);
            
            switch (space)
            {
                case Space.World:
                    rotation = Quaternion.Euler(dEuler);
                    break;
                case Space.Fixed:
                    rotation = Helpers.Euler(dEuler, Quaternion.Euler(euler));
                    break;
                case Space.Dynamic:
                    if (!target.target.Check())
                    {
                        BulletStormLogger.LogWarningOnce($"{controller}: Can't find space {target}, use world space by default.");
                        rotation = Quaternion.Euler(dEuler);
                    }
                    else
                    {
                        rotation = Helpers.Euler(dEuler, target.target.AsTransform.rotation);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            controller.ChangeParam(param =>
            {
                param.rotation = rotation * param.rotation;
                return param;
            });
        }
        
        [Serializable]
        private enum Space
        {
            [Tooltip("Simulates in world space.")]
            World,
            [Tooltip("Take given euler as simulating space.")]
            Fixed,
            [Tooltip("Take given target as simulating space.")]
            Dynamic
        }

        [Serializable]
        private struct TargetWrapper
        {
            public Target target;
        }
    }
}