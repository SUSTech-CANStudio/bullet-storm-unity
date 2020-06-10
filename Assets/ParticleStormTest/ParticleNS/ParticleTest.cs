using NUnit.Framework;
using ParticleStorm.ParticleNS;

namespace ParticcleStormTest.ParticleNS
{
	public class ParticleTest
	{
		[Test]
		public void Test0_Instantiate()
		{
			var particle = new Particle();
			Assert.IsNotNull(particle.Origin);
		}

		[Test]
		public void Test1_FindParticle()
		{
			string name = "particle";
			new Particle(name);
			Assert.IsNotNull(Particle.Find(name));
			Assert.IsNull(Particle.Find("not exist"));
		}

		[Test]
		public void Test2_Emit()
		{
			new Particle().Origin.Emit(new EmitParams());
		}
	}
}
