using System;
using System.Collections;
using System.Threading.Tasks;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem
{
	/// <summary>
	/// A bullet system implemented using Unity <see cref="ParticleSystem"/>.
	/// </summary>
	/// This class derives from <see cref="MonoBehaviour"/>, so you can simply attach it onto a <see cref="GameObject"/>,
	/// and modify attributes in inspector.
	/// <para/>
	/// It works with a Unity <see cref="ParticleSystem"/> component, any settings on the component or on this script can work together.
	/// Normally you won't want a bullet emitter to emit bullet randomly like a traditional particle system, so the emission module is
	/// disabled by default (you can also enable it to preview the bullets in editor, remember to disable it before entering the game).
	/// Simulation space is set to 'World' by default (most users won't hope bullets move together with the emitter),
	/// you can find the setting in particle system main module.
	/// <para/>
	/// You can create a prefab with this script, and use it in your storms.
	[RequireComponent(typeof(ParticleSystem))]
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class ParticleBulletSystem : BulletSystemBase
	{
		[Tooltip("Enable custom speed over lifetime, this will override emit speed."), Label("Enable")]
		[SerializeField, BoxGroup("Speed over lifetime")]
		private bool enableSpeedOverLifetime;
		[Label("Detail")]
		[SerializeField, BoxGroup("Speed over lifetime"), EnableIf("enableSpeedOverLifetime")]
		private SpeedOverLifetimeModule speedOverLifetime;

		private ParticleSystem ps;
		private ParticleSystem.MainModule psm;
		private ParticleSystemRenderer psr;
		private ParticleSystem.Particle[] particles;
		private int particleCount;
		private bool particlesUpToDate;

		public override void ChangeVelocity(Func<Vector3, Vector3, Vector3> operation)
		{
			UpdateParticles();
			var renderMode = psr.renderMode;
			Parallel.For(0, particleCount, i =>
			{
				var oldVelocity = particles[i].velocity;
				particles[i].velocity = operation(particles[i].position, oldVelocity);
				// also change rotation if particle is mesh
				if (renderMode == ParticleSystemRenderMode.Mesh)
				{
					particles[i].rotation3D =
						(Quaternion.FromToRotation(oldVelocity == Vector3.zero ? Vector3.forward : oldVelocity,
							 particles[i].velocity == Vector3.zero ? Vector3.forward : particles[i].velocity) *
						 Quaternion.Euler(particles[i].rotation3D)).eulerAngles;
				}
			});
		}

		public override void ChangeParam(Func<BulletParam, BulletParam> operation)
		{
			UpdateParticles();
			var renderMode = psr.renderMode;
			Parallel.For(0, particleCount, i =>
			{
				var oldParam = new BulletParam(Quaternion.LookRotation(particles[i].velocity.Normalized()),
					particles[i].position,
					particles[i].velocity.magnitude,
					particles[i].startLifetime - particles[i].remainingLifetime);
				var newParam = operation(oldParam);
				particles[i].velocity = (newParam.rotation * Vector3.forward).SafeChangeMagnitude(newParam.speed);
				particles[i].position = newParam.position;
				// also change rotation if particle is mesh
				if (renderMode == ParticleSystemRenderMode.Mesh)
				{
					particles[i].rotation3D =
						(Quaternion.Inverse(oldParam.rotation) * newParam.rotation *
						 Quaternion.Euler(particles[i].rotation3D)).eulerAngles;
				}
			});
		}

		public override void ChangePosition(Func<Vector3, Vector3, Vector3> operation)
		{
			UpdateParticles();
			Parallel.For(0, particleCount, i =>
			{
				particles[i].position = operation(particles[i].position, particles[i].velocity);
			});
		}

		public override void Emit(BulletEmitParam relative, Transform emitter)
		{
			ParticleSystem.EmitParams ToEmitParams(in BulletEmitParam bulletEmitParam)
			{
				var result = new ParticleSystem.EmitParams();
				result.position = bulletEmitParam.position;
				result.velocity = bulletEmitParam.velocity;
				if (psr.renderMode == ParticleSystemRenderMode.Mesh)
				{
					var startRotation3D = psm.startRotation3D
						? new Vector3(
							psm.startRotationX.Evaluate(ps.time) * psm.startRotationXMultiplier,
							psm.startRotationY.Evaluate(ps.time) * psm.startRotationYMultiplier,
							psm.startRotationZ.Evaluate(ps.time) * psm.startRotationZMultiplier)
						: new Vector3(0, 0, psm.startRotation.Evaluate(ps.time) * psm.startRotationMultiplier);
					result.rotation3D =
						(Quaternion.LookRotation(result.velocity == Vector3.zero ? Vector3.forward : result.velocity) *
						 Quaternion.Euler(startRotation3D)).eulerAngles;
				}
				if (!bulletEmitParam.DefaultColor)
				{
					result.startColor = bulletEmitParam.color;
				}
				if (!bulletEmitParam.DefaultSize)
				{
					result.startSize3D = bulletEmitParam.size;
				}

				return result;
			}

			ps.Emit(
				psm.simulationSpace == ParticleSystemSimulationSpace.World
					? ToEmitParams(relative.RelativeTo(emitter))
					: ToEmitParams(relative), 1);
			
			PlayEmissionEffect(relative, emitter);
		}

		public override void Destroy()
		{
			if(this) StartCoroutine(WaitForDestroy());
		}

		private IEnumerator WaitForDestroy()
		{
			while (true)
			{
				if (ps.particleCount == 0) break;
				yield return null;
			}
			Destroy(this);
		}

		/// <summary>
		/// Updates cache in <see cref="particles"/> if needed.
		/// </summary>
		private void UpdateParticles()
		{
			if (particlesUpToDate) return;
			if (particles is null || particles.Length < psm.maxParticles) particles = new ParticleSystem.Particle[psm.maxParticles];
			particleCount = ps.GetParticles(particles);
			particlesUpToDate = true;
		}

		/// <summary>
		/// Writes particles into particle system if needed.
		/// </summary>
		private void WriteParticles()
		{
			if (!particlesUpToDate) return;
			ps.SetParticles(particles, particleCount);
			particlesUpToDate = false;
		}

		private void Awake()
		{
			ps = GetComponent<ParticleSystem>();
			psm = ps.main;
			psr = GetComponent<ParticleSystemRenderer>();

			// preset particle system in editor
#if UNITY_EDITOR
			psm.simulationSpace = ParticleSystemSimulationSpace.World;
			var emission = ps.emission;
			emission.enabled = false;
			var shape = ps.shape;
			shape.enabled = false;
#endif
		}

		protected override void Update()
		{
			base.Update();
			
			if (enableSpeedOverLifetime)
			{
				UpdateParticles();
				Parallel.For(0, particleCount, i =>
				{
					var speed = speedOverLifetime.GetSpeed(
						(particles[i].startLifetime - particles[i].remainingLifetime) / particles[i].startLifetime);
					particles[i].velocity = particles[i].velocity.SafeChangeMagnitude(Mathf.Max(speed, 0));
				});
			}
			
			WriteParticles();
		}
		
		[Serializable]
		private struct SpeedOverLifetimeModule
		{
			[Tooltip("y-axis is speed, x-axis from 0 to 1 represents lifetime."), CurveRange(0, 0, 1, 1)]
			public AnimationCurve speed;

			[Tooltip("Multiply speed by this value"), MinValue(1), AllowNesting]
			public float speedMultiplier;

			public float GetSpeed(float normalizedLifetime) =>
				speed.Evaluate(normalizedLifetime) * speedMultiplier;
		}
	}
}