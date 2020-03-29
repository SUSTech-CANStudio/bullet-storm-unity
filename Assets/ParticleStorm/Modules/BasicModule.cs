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
		public Material material;
		public Material[] materials;
		public Mesh mesh;

		public BasicModule(ParticleSystemRenderMode renderMode, Material material)
		{
			this.renderMode = renderMode;
			this.material = material;
		}

		public BasicModule(ParticleSystemRenderMode renderMode, Material material, Mesh mesh, Material[] materials = null)
		{
			this.renderMode = renderMode;
			this.material = material;
			this.mesh = mesh;
			this.materials = materials;
		}

		public BasicModule(ParticleSystemRenderMode renderMode, Material material, Material[] materials)
		{
			this.renderMode = renderMode;
			this.material = material;
			this.materials = materials;
		}

		public BasicModule(Material material, Mesh mesh, Material[] materials = null)
		{
			this.renderMode = ParticleSystemRenderMode.Mesh;
			this.material = material;
			this.mesh = mesh;
			this.materials = materials;
		}

		public void AddOn(PSParticleSystem psp)
		{
			var psr = psp.GetComponent<ParticleSystemRenderer>();
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

		public void AddOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}

		public void DeleteFrom(PSParticleSystem psp) { }

		public void DeleteFrom(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
