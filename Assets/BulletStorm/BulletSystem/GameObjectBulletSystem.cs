using System;
using BulletStorm.Emission;
using UnityEngine;

namespace BulletStorm.BulletSystem
{
    public class GameObjectBulletSystem : IBulletSystem
    {
        public void ChangePosition(Func<Vector3, Vector3, Vector3> operation)
        {
            throw new NotImplementedException();
        }

        public void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation)
        {
            throw new NotImplementedException();
        }

        public void Emit(BulletEmitParam emitParam, Transform emitter)
        {
            throw new NotImplementedException();
        }
    }
}