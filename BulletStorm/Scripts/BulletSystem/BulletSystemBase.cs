using System;
using BulletStorm.Emission;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BulletStorm.BulletSystem
{
    /// <summary>
    /// A more convenient base class for <see cref="MonoBehaviour"/> based particle systems.
    /// </summary>
    public abstract class BulletSystemBase : MonoBehaviour, IBulletSystem, IBulletController
    {
        public string Name => name;
        public abstract void ChangePosition(Func<Vector3, Vector3, Vector3> operation);
        public abstract void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation);
        public abstract void Emit(BulletEmitParam emitParam, Transform emitter);
        public abstract void Destroy();
        public IBulletController GetController() => Instantiate(this);
        public void SetParent(Transform parent) => transform.SetParent(parent, false);
    }
}