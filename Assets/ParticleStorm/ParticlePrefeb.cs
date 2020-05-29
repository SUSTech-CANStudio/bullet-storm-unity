#pragma warning disable 0649

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
		private BasicModule basicModule = new BasicModule() { 
			defaultParams = new BasicModule.Parameters() {
				startLifeTime = 10,
				startColor = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, 1)),
				startSize = 1
		} };
		[SerializeField]
		private ColorBySpeedModule colorBySpeedModule;
		[SerializeField]
		private ColorOverLifetimeModule colorOverLifetimeModule;
		[SerializeField]
		private RotationBySpeedModule rotationBySpeedModule = new RotationBySpeedModule()
		{
			xMultiplier = 1,
			yMultiplier = 1,
			zMultiplier = 1
		};
		[SerializeField]
		private RotationOverLifetimeModule rotationOverLifetimeModule = new RotationOverLifetimeModule()
		{
			xMultiplier = 1,
			yMultiplier = 1,
			zMultiplier = 1
		};
		[SerializeField]
		private ScriptModule scriptModule;
		[SerializeField]
		private SizeBySpeedModule sizeBySpeedModule = new SizeBySpeedModule()
		{
			sizeMultiplier = 1
		};
		[SerializeField]
		private SizeOverLifetimeModule sizeOverLifetimeModule = new SizeOverLifetimeModule()
		{
			sizeMultiplier = 1
		};
		//[SerializeField]
		//private TrailModule trailModule;
		[SerializeField]
		private CollisionModule collisionModule;
		[SerializeField]
		private VelocityOverLifetimeModule velocityOverLifetimeModule = new VelocityOverLifetimeModule()
		{
			speedMultiplier = 1
		};

		internal void Bind(IParticleSystem particleSystem)
		{
			particleSystem.ApplicateModule(basicModule);
			particleSystem.ApplicateModule(colorBySpeedModule);
			particleSystem.ApplicateModule(colorOverLifetimeModule);
			particleSystem.ApplicateModule(rotationBySpeedModule);
			particleSystem.ApplicateModule(rotationOverLifetimeModule);
			particleSystem.ApplicateModule(sizeBySpeedModule);
			particleSystem.ApplicateModule(sizeOverLifetimeModule);
			//particleSystem.ApplicateModule(trailModule);
			particleSystem.ApplicateModule(collisionModule);
			particleSystem.ApplicateModule(velocityOverLifetimeModule);
			particleSystem.ApplicateModule(scriptModule);
		}
	}
}
