using ParticleStorm.Core;
using ParticleStorm.Modules;
using UnityEngine;

namespace ParticleStorm
{
	[CreateAssetMenu]
	public class ParticlePrefeb : ScriptableObject
	{
		[SerializeField]
		[Tooltip("Use particle system to generate particle is faster.\nUse game object may have more options.")]
		internal bool useParticleSystem = true;
		[SerializeField]
		private BasicModule basicModule;
		[SerializeField]
		private ScriptModule scriptModule;

		internal void Bind(IParticleSystem particleSystem)
		{
			particleSystem.ApplicateModule(basicModule);
			particleSystem.ApplicateModule(scriptModule);
		}
	}
}
