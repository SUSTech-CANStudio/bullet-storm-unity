using NUnit.Framework;
using ParticleStorm.ParticleNS;
using UnityEngine;

namespace ParticcleStormTest.Core
{
	public class ParticleSystemControllerTest
	{
		[Test]
		public void Test0_InstantiateOrigin()
		{
			var origin = new ParticleSystemController();
			Assert.IsNotNull(origin.GameObject);
			Assert.IsNotNull(origin.ParticleSystem);
			Assert.IsTrue(origin.IsOrigin);
		}

		[Test]
		public void Test1_InstantiateOriginWithName()
		{
			string name = "name 1";
			var origin = new ParticleSystemController(name);
			Assert.IsNotNull(origin.GameObject);
			Assert.IsNotNull(origin.ParticleSystem);
			Assert.AreEqual(origin.GameObject.name, name);
			Assert.IsTrue(origin.IsOrigin);
		}

		[Test]
		public void Test2_InstantiateOriginWithPrefeb()
		{
			var prefeb = ScriptableObject.CreateInstance<ParticlePrefeb>();
			prefeb.name = "name 2";
			var origin = new ParticleSystemController(prefeb);
			Assert.IsNotNull(origin.GameObject);
			Assert.IsNotNull(origin.ParticleSystem);
			Assert.AreEqual(origin.GameObject.name, prefeb.name);
			Assert.IsTrue(origin.IsOrigin);
		}

		[Test]
		public void Test3_InstantiateOriginWithNameAndPrefeb()
		{
			string name = "name 3";
			var prefeb = ScriptableObject.CreateInstance<ParticlePrefeb>();
			prefeb.name = "name 4";
			var origin = new ParticleSystemController(name, prefeb);
			Assert.IsNotNull(origin.GameObject);
			Assert.IsNotNull(origin.ParticleSystem);
			Assert.AreEqual(origin.GameObject.name, name);
			Assert.IsTrue(origin.IsOrigin);
		}

		[Test]
		public void Test4_InstantiateCopy()
		{
			var origin = new ParticleSystemController();
			var copy = new ParticleSystemController(origin, new GameObject().transform);
			Assert.IsNotNull(copy.GameObject);
			Assert.IsNotNull(copy.ParticleSystem);
			Assert.IsFalse(copy.IsOrigin);
		}
	}
}
