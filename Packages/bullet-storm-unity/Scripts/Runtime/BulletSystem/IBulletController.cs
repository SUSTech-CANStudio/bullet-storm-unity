﻿using System;
using CANStudio.BulletStorm.Emission;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    public interface IBulletController
    {
        Quaternion Rotation { get; set; }
        
        /// <summary>
        /// Changes all bullets' position in the controller.
        /// </summary>
        /// <param name="operation">Vector3 ChangedPosition(Vector3 oldPosition, Vector3 oldVelocity)</param>
        [Obsolete("Use ChangeParam() instead")]
        void ChangePosition(Func<Vector3, Vector3, Vector3> operation);
        
        /// <summary>
        /// Changes all bullets' velocity in the controller.
        /// </summary>
        /// <param name="operation">Vector3 ChangedVelocity(Vector3 oldPosition, Vector3 oldVelocity)</param>
        [Obsolete("Use ChangeParam() instead")]
        void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation);

        /// <summary>
        /// Changes all bullets' parameters in the controller.
        /// Notice that the readonly value <see cref="BulletParam.lifetime"/> won't be changed by this function.
        /// </summary>
        /// <param name="operation"></param>
        void ChangeParam(Func<BulletParam, BulletParam> operation);
        
        /// <summary>
        /// Emits a new bullet from the controller.
        /// </summary>
        /// <param name="emitParam">Bullet initial parameters.</param>
        /// <param name="emitter">Transform of the emitter.</param>
        void Emit(BulletEmitParam emitParam, Transform emitter);
        
        /// <summary>
        /// Set the parent transform of the controller.
        /// </summary>
        /// <param name="parent">If the controller simulates in local space,
        /// this parameter preforms as the simulation space.</param>
        void SetParent(Transform parent);

        /// <summary>
        /// Destroy the controller when it contains no bullet.
        /// </summary>
        void Destroy();
    }
}