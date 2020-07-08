#pragma warning disable 0649

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BulletStorm.BulletNS.Script;

namespace BulletStorm.BulletNS.Modules
{
	/// <summary>
	/// Effect on emission.
	/// </summary>
	[Serializable]
	internal struct EmitEffectModule : IParticleModule
	{
		[Tooltip("Enable effect on emission")]
		public bool enabled;

		[Tooltip("The particle effect to play")]
		public ParticleSystem particleSystem;

		[Tooltip("Position of the effect")]
		[SerializeField]
		private EffectPosition position;

		[Tooltip("Rotation of the effect")]
		[SerializeField]
		private EffectRotation rotation;

		public void ApplicateOn(BulletSystemController psc)
		{
			if (enabled)
			{
				psc.EmissionEvent =
					new EmissionEvent { OnParticleEmission = PlayEffect };
				var main = particleSystem.GetComponent<ParticleSystem>().main;
				main.stopAction = ParticleSystemStopAction.Destroy;
			}
			else { psc.EmissionEvent = null; }
		}

		private void PlayEffect(Transform transform, EmitParams emitParams)
		{
			Vector3 p = Vector3.zero;
			Quaternion r = Quaternion.identity;
			if (position == EffectPosition.Particle) { p = emitParams.Position; }
			if (rotation == EffectRotation.TowardsParticlePosition)
			{
				r = Quaternion.LookRotation(emitParams.Position);
			}
			else if (rotation == EffectRotation.TowardsParticleVelocity)
			{
				r = Quaternion.Euler(emitParams.Rotation3D);
			}
			var go = GameObject.Instantiate(particleSystem, transform, false);
			go.transform.position += p;
			go.transform.rotation = r * go.transform.rotation;
		}

		enum EffectRotation { None, TowardsParticlePosition, TowardsParticleVelocity }
		enum EffectPosition { Emitter, Particle }
	}
}
