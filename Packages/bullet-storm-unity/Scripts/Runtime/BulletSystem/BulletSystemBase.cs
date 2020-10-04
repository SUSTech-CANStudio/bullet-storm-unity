using System;
using CANStudio.BulletStorm.BulletSystem.Modules;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util.EditorAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    /// A more convenient base class for <see cref="MonoBehaviour"/> based particle systems.
    /// </summary>
    public abstract class BulletSystemBase : MonoBehaviour, IBulletSystem, IBulletController
    {
        [Tooltip("Play particle effect when emitting.")]
        [SerializeField] private EmissionEffectModule emissionEffect;
        [Tooltip("Bullets trace a target.")]
        [SerializeField] private TracingModule tracing;
        
        public virtual string Name => name;
        public abstract void ChangePosition(Func<Vector3, Vector3, Vector3> operation);
        public abstract void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation);
        public abstract void Emit(BulletEmitParam emitParam, Transform emitter);
        public abstract void Destroy();
        public virtual IBulletController GetController() => Instantiate(this);
        public virtual void SetParent(Transform parent) => transform.SetParent(parent, false);

        /// <summary>
        /// Plays the emission effect. Call this when emitting a bullet.
        /// </summary>
        protected void PlayEmissionEffect(BulletEmitParam emitParam, Transform emitter) =>
            emissionEffect.OnEmit(emitParam, emitter);
        
        /// <summary>
        /// Executes tracing module.
        /// </summary>
        protected void Start()
        {
            tracing.OnStart();
        }

        /// <summary>
        /// Executes tracing module.
        /// </summary>
        protected void Update()
        {
            tracing.OnUpdate(this);
        }
    }
}