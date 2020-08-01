using System;
using BulletStorm.Emission;
using UnityEngine;

namespace BulletStorm.BulletSystem
{
    public interface IBulletController
    {
        /// <summary>
        /// Changes all bullets' position in the controller.
        /// </summary>
        /// <param name="operation">Vector3 ChangedPosition(Vector3 oldPosition, Vector3 oldVelocity)</param>
        void ChangePosition(Func<Vector3, Vector3, Vector3> operation);
        
        /// <summary>
        /// Changes all bullets' velocity in the controller.
        /// </summary>
        /// <param name="operation">Vector3 ChangedVelocity(Vector3 oldPosition, Vector3 oldVelocity)</param>
        void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation);
        
        /// <summary>
        /// Emits a new bullet from the controller.
        /// </summary>
        /// <param name="emitParam">Bullet initial parameters.</param>
        /// <param name="emitter">Transform of the emitter.</param>
        void Emit(BulletEmitParam emitParam, Transform emitter);
        
        /// <summary>
        /// Set the parent transform of the controller.
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(Transform parent);

        void Destroy();
    }
}