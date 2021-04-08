using System;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    public struct DeflectionModule
    {
        [Tooltip("Velocity deflection angle per second.")] [SerializeField]
        private Vector2 deflection;

        public void OnUpdate(IBulletController controller)
        {
            var dEuler = new Vector3(-deflection.y * Time.deltaTime, deflection.x * Time.deltaTime);
            var rotation = Quaternion.Euler(dEuler);

            controller.ChangeParam(param =>
            {
                param.rotation = rotation * param.rotation;
                return param;
            });
        }
    }
}