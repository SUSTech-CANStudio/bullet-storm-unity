namespace ParticleStorm.Core
{
	interface IParticleModule
	{
		/// <summary>
		/// Add the module on a <see cref="ParticleSystemController"/>.
		/// </summary>
		/// <param name="psc">The particle system controller</param>
		void ApplicateOn(ParticleSystemController psc);
	}
}
