using System;
using CANStudio.BulletStorm.Emission;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    public interface IBulletController
    {
        Quaternion Rotation { get; set; }

        /// <summary>
        ///     Changes all bullets' parameters in the controller.
        ///     Notice that the readonly value <see cref="BulletParam.lifetime" /> won't be changed by this function.
        /// </summary>
        /// <param name="operation"></param>
        void ChangeParam(Func<BulletParam, BulletParam> operation);

        /// <summary>
        ///     Emits a new bullet from the controller.
        /// </summary>
        /// <param name="emitParam">Bullet initial parameters.</param>
        /// <param name="emitter">Transform of the emitter.</param>
        void Emit(BulletEmitParam emitParam, Transform emitter);

        /// <summary>
        ///     Destroy the controller when it contains no bullet.
        /// </summary>
        void Destroy();
    }
}