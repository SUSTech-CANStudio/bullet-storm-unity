#pragma warning disable 0649

using ParticleStorm.Core;
using System;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct TrailModule : IParticleModule
	{

		public void ApplicateOn(PSParticleSystem ps)
		{
			throw new NotImplementedException();
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
