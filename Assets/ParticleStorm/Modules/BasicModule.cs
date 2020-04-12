using ParticleStorm.Core;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	/// <summary>
	/// Basic attributes of particle.
	/// </summary>
	[Serializable]
	internal sealed class BasicModule : IParticleModule
	{
		public ParticleSystemRenderMode renderMode;
		public float lifetime = 10;
		public Material material;
		public Material[] materials;
		public Mesh mesh;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var psr = ps.GetComponent<ParticleSystemRenderer>();
			var main = ps.main;
			main.startLifetime = lifetime;
			psr.renderMode = renderMode;
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
