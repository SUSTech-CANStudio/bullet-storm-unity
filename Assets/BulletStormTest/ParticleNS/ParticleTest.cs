using NUnit.Framework;
using BulletStorm.BulletNS;

namespace ParticcleStormTest.ParticleNS
{
	public class ParticleTest
	{
		[Test]
		public void Test0_Instantiate()
		{
			var particle = new Bullet();
			Assert.IsNotNull(particle.Origin);
		}

		[Test]
		public void Test1_FindParticle()
		{
			string name = "particle";
			new Bullet(name);
			Assert.IsNotNull(Bullet.Find(name));
			Assert.IsNull(Bullet.Find("not exist"));
		}

		[Test]
		public void Test2_Emit()
		{
			new Bullet().Origin.Emit(new EmitParams());
		}
	}
}
