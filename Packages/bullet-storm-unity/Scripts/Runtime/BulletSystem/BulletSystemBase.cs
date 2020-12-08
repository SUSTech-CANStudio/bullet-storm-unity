using System;
using CANStudio.BulletStorm.BulletSystem.Modules;
using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    /// A more convenient base class for <see cref="MonoBehaviour"/> based particle systems.
    /// </summary>
    public abstract class BulletSystemBase : MonoBehaviour, IBulletSystem, IBulletController
    {
        [Tooltip("Enable playing effect when emitting bullets."), Label("Enable")]
        [SerializeField, BoxGroup("Emission effect")]
        private bool enableEmissionEffect;
        
        [Tooltip("Play particle effect when emitting."), Label("Detail")]
        [SerializeField, EnableIf(nameof(enableEmissionEffect)), BoxGroup("Emission effect")]
        private EmissionEffectModule emissionEffect;
        
        [Tooltip("Enable bullets tracing some game object."), Label("Enable")]
        [SerializeField, BoxGroup("Tracing")]
        private bool enableTracing;
        
        [Tooltip("Bullets trace a target."), Label("Detail")]
        [SerializeField, EnableIf(nameof(enableTracing)), BoxGroup("Tracing")]
        private TracingModule tracing;

        [Tooltip("Enable bullets accelerate."), Label("Enable")]
        [SerializeField, BoxGroup("Acceleration")]
        private bool enableAcceleration;

        [Tooltip("Bullets accelerate during whole lifetime."), Label("Detail")]
        [SerializeField, EnableIf(nameof(enableAcceleration)), BoxGroup("Acceleration")]
        private AccelerationModule acceleration;

        [Tooltip("Enable bullets velocity deflection."), Label("Enable")]
        [SerializeField, BoxGroup("Deflection")]
        private bool enableDeflection;

        [Tooltip("Bullets velocity deflection during whole lifetime."), Label("Detail")]
        [SerializeField, EnableIf(nameof(enableDeflection)), BoxGroup("Deflection")]
        private DeflectionModule deflection;
        
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
        protected void PlayEmissionEffect(BulletEmitParam emitParam, Transform emitter)
        {
            if (enableEmissionEffect) emissionEffect.OnEmit(emitParam, emitter);
        }

        /// <summary>
        /// Executes tracing module.
        /// </summary>
        protected virtual void Update()
        {
            if (enableTracing) tracing.OnUpdate(this);
            if (enableAcceleration) acceleration.OnUpdate(this);
            if (enableDeflection) deflection.OnUpdate(this);
        }
    }
}