namespace BulletStorm.BulletNS.Modules
{
	internal interface IParticleModule
	{
		/// <summary>
		/// Add the module on a <see cref="BulletSystemController"/>.
		/// </summary>
		/// <param name="psc">The particle system controller</param>
		void ApplicateOn(BulletSystemController psc);
	}
}
