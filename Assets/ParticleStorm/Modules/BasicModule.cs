#pragma warning disable 0649

using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	/// <summary>
	/// Basic attributes of particle.
	/// </summary>
	[Serializable]
	internal struct BasicModule : IParticleModule
	{
		[Serializable]
		public struct Parameters
		{
			public float startLifeTime;
			public ParticleSystem.MinMaxGradient startColor;
			public ParticleSystem.MinMaxCurve startSize;
		}

		[Tooltip("Render mode of particles.")]
		public ParticleSystemRenderMode renderMode;
		[Tooltip("Material of particles.")]
		public Material material;
		[Tooltip("Materials of particles, material 0 is the default material.")]
		public Material[] materials;
		[Tooltip("Mesh of particles.")]
		public Mesh mesh;
		[Tooltip("Default parameters of the particle.")]
		public Parameters defaultParams;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var psr = ps.GetComponent<ParticleSystemRenderer>();
			var main = ps.Main;
			psr.renderMode = renderMode;
			main.startColor = defaultParams.startColor;
			main.startLifetime = defaultParams.startLifeTime;
			main.startSize = defaultParams.startSize;
			if (material == null)
				Debug.LogWarning("Material is null");
			else
				psr.material = material;
			if (materials != null && materials.Length > 0)
				psr.materials = materials;
			if (psr.renderMode == ParticleSystemRenderMode.Mesh)
			{
				if (mesh == null)
					Debug.LogWarning("Mesh is null");
				psr.mesh = mesh;
			}
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
