#pragma warning disable 0649

using BulletStorm.BulletNS.Modules;
using UnityEngine;

namespace BulletStorm.BulletNS
{
	[CreateAssetMenu]
	public class BulletPrefeb : ScriptableObject
	{
		[SerializeField]
		private BasicModule basicModule = new BasicModule
		{
			defaultParams = new BasicModule.Parameters
			{
				startLifeTime = 10,
				startColor = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, 1)),
				startSize = 1
			}
		};
		[SerializeField]
		private ColorBySpeedModule colorBySpeedModule;
		[SerializeField]
		private ColorOverLifetimeModule colorOverLifetimeModule;
		[SerializeField]
		private RotationBySpeedModule rotationBySpeedModule = new RotationBySpeedModule
		{
			xMultiplier = 1,
			yMultiplier = 1,
			zMultiplier = 1
		};
		[SerializeField]
		private RotationOverLifetimeModule rotationOverLifetimeModule = new RotationOverLifetimeModule
		{
			xMultiplier = 1,
			yMultiplier = 1,
			zMultiplier = 1
		};
		[SerializeField]
		private ScriptModule scriptModule;
		[SerializeField]
		private SizeBySpeedModule sizeBySpeedModule = new SizeBySpeedModule
		{
			sizeMultiplier = 1
		};
		[SerializeField]
		private SizeOverLifetimeModule sizeOverLifetimeModule = new SizeOverLifetimeModule
		{
			sizeMultiplier = 1
		};
		[SerializeField]
		private EmitEffectModule emitEffectModule;
		//[SerializeField]
		//private TrailModule trailModule;
		[SerializeField]
		private CollisionModule collisionModule;
		[SerializeField]
		private VelocityOverLifetimeModule velocityOverLifetimeModule = new VelocityOverLifetimeModule
		{
			speedMultiplier = 1
		};

		internal void ApplicateOn(BulletSystemController psc)
		{
			basicModule.ApplicateOn(psc);
			colorBySpeedModule.ApplicateOn(psc);
			colorOverLifetimeModule.ApplicateOn(psc);
			rotationBySpeedModule.ApplicateOn(psc);
			rotationOverLifetimeModule.ApplicateOn(psc);
			sizeBySpeedModule.ApplicateOn(psc);
			sizeOverLifetimeModule.ApplicateOn(psc);
			collisionModule.ApplicateOn(psc);
			emitEffectModule.ApplicateOn(psc);
			velocityOverLifetimeModule.ApplicateOn(psc);
			scriptModule.ApplicateOn(psc);
		}
	}
}
