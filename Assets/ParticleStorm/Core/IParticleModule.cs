namespace ParticleStorm.Core
{
	interface IParticleModule
	{
		/// <summary>
		/// Add the module on a <see cref="PSParticleSystem"/>.
		/// </summary>
		/// <param name="ps"></param>
		void ApplicateOn(PSParticleSystem ps);
	}
}
