namespace ParticleStorm.Core
{
	interface IParticleModule
	{
		/// <summary>
		/// Add the module on a <see cref="PSParticleSystem"/>.
		/// </summary>
		/// <param name="ps"></param>
		void ApplicateOn(PSParticleSystem ps);
		/// <summary>
		/// Add the module on a <see cref="GOParticleSystem"/>.
		/// </summary>
		/// <param name="go"></param>
		void ApplicateOn(GOParticleSystem go);
	}
}
