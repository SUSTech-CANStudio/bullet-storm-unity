using System;
using CANStudio.BulletStorm.Util;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    public struct AroundAxisModule
    {
        [Tooltip("Rotates around this axis.")] [SerializeField]
        private Vector3 axis;

        [Tooltip("If select 'self', use reference system set in emitter.")] [SerializeField]
        private Space space;

        [Tooltip("Per second rotation angle in degree.")] [SerializeField]
        private float anglePerSecond;

        public void OnUpdate(IBulletController controller)
        {
            if (axis == Vector3.zero)
            {
                BulletStormLogger.LogErrorOnce($"{controller}: In Around axis module, axis can't be zero!");
                return;
            }

            Vector3 axisInWorld;
            switch (space)
            {
                case Space.World:
                    axisInWorld = axis;
                    break;
                case Space.Self:
                    axisInWorld = controller.Rotation * axis;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var angle = anglePerSecond * Time.deltaTime;
            controller.ChangeParam(param =>
            {
                param.rotation = Quaternion.AngleAxis(angle, axisInWorld) * param.rotation;
                return param;
            });
        }
    }
}