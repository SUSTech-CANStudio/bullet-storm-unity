using System;
using CANStudio.BulletStorm.Emission;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem.Modules
{
    [Serializable]
    internal struct EmissionEffectModule
    {
        [Tooltip("Enable playing effect when emitting bullets.")]
        [SerializeField] private bool enabled;
        [Tooltip("Particle effect to play when bullets emitted.")]
        [SerializeField] private ParticleSystem effect;
        [Tooltip("Where to play the effect.")]
        [SerializeField] private EffectPosition position;
        [Tooltip("How to rotate the effect.")]
        [SerializeField] private EffectRotation rotation;
        
        public void OnEmit(BulletEmitParam relative, Transform emitter)
        {
            if (!enabled) return;
            Quaternion lookRotation;
            Vector3 absolutePosition;
            switch (position)
            {
                case EffectPosition.OnEmitter:
                    absolutePosition = emitter.position;
                    break;
                case EffectPosition.OnParticle:
                    absolutePosition = emitter.position + relative.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (rotation)
            {
                case EffectRotation.TowardsParticlePosition:
                    lookRotation = Quaternion.LookRotation(relative.position - emitter.position);
                    break;
                case EffectRotation.TowardsParticleVelocity:
                    lookRotation = Quaternion.LookRotation(relative.velocity);
                    break;
                case EffectRotation.None:
                    lookRotation = Quaternion.identity;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var ps = Object.Instantiate(effect, absolutePosition, lookRotation, emitter);
            var psm = ps.main;
            psm.stopAction = ParticleSystemStopAction.Destroy;
            ps.Play();
        }

        private enum EffectPosition
        {
            /** Generate effect on emitter */
            OnEmitter,
            /** Generate effect on particle */
            OnParticle
        }
        
        private enum EffectRotation
        {
            /** Don't rotate */
            None,
            /** From emitter to particle position */
            TowardsParticlePosition,
            /** Same direction as particle velocity */
            TowardsParticleVelocity
        }
    }
}