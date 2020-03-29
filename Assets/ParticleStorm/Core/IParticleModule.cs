namespace ParticleStorm.Core
{
	interface IParticleModule
	{
		/// <summary>
		/// Add the module on a <see cref="PSParticleSystem"/>.
		/// </summary>
		/// <param name="ps"></param>
		void AddOn(PSParticleSystem ps);
		/// <summary>
		/// Add the module on a <see cref="GOParticleSystem"/>.
		/// </summary>
		/// <param name="go"></param>
		void AddOn(GOParticleSystem go);
		/// <summary>
		/// Delete the module from a <see cref="PSParticleSystem"/>.
		/// </summary>
		/// <param name="ps"></param>
		void DeleteFrom(PSParticleSystem ps);
		/// <summary>
		/// Delete the module from a <see cref="GOParticleSystem"/>.
		/// </summary>
		/// <param name="go"></param>
		void DeleteFrom(GOParticleSystem go);
	}
}
