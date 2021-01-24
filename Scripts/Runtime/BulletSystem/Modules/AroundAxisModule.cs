using System;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    public struct AroundAxisModule
    {
        [InfoBox("This module is experimental, your configure may loss when updating to next version.", EInfoBoxType.Warning)]
        
        [Tooltip("Rotates around this axis."), SerializeField]
        private Vector3 axis;

        [Tooltip("Per second rotation angle in degree."), SerializeField]
        private float anglePerSecond;

        public void OnUpdate(IBulletController controller)
        {
            if (axis == Vector3.zero)
            {
                BulletStormLogger.LogErrorOnce($"{controller}: In Around axis module, axis can't be zero!");
                return;
            }
            var ax = axis;
            var angle = anglePerSecond * Time.deltaTime;
            controller.ChangeParam(param =>
            {
                param.rotation = Quaternion.AngleAxis(angle, ax) * param.rotation;
                return param;
            });
        }
    }
}