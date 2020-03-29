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
		internal BasicModule basicModule;

		internal ParticlePrefeb(BasicModule basic) => basicModule = basic;

		internal void Bind(IParticleSystem particleSystem)
		{
			particleSystem.AddModule(basicModule);
		}
	}
}
