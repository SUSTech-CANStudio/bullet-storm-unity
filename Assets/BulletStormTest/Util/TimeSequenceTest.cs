using NUnit.Framework;
using System;
using System.Collections.Generic;
using BulletStorm.Util;
using UnityEngine;

namespace ParticleStormTest.Util
{
	public class TimeSequenceTest
	{
		[Test]
		public void Test0_GetMarks()
		{
			TimeSequence sequence = new TimeSequence(
				new List<float> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9});
			Assert.AreEqual(sequence.GetMarks(0, 0f), new List<float> { 0 });
			Assert.AreEqual(sequence.GetMarks(0, 0.5f), new List<float> { 0 });
			Assert.AreEqual(sequence.GetMarks(1, 0.5f), new List<float> { });
		}
	}
}
