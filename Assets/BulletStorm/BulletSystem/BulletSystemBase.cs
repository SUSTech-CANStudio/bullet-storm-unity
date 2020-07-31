using System;
using BulletStorm.Emission;
using UnityEngine;

namespace BulletStorm.BulletSystem
{
    public abstract class BulletSystemBase : MonoBehaviour, IOriginBulletSystem, ICopiedBulletSystem
    {
        public string Name => name;
        public abstract void ChangePosition(Func<Vector3, Vector3, Vector3> operation);
        public abstract void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation);
        public abstract void Emit(BulletEmitParam emitParam, Transform emitter);
        public void SetParent(Transform parent) => transform.SetParent(parent, false);
        public ICopiedBulletSystem Copy() => Instantiate(this);
    }
}