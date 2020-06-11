using NUnit.Framework;
using System;
using System.Collections.Generic;
using ParticleStorm.Util;
using UnityEngine;

namespace ParticleStormTest.Util
{
	public class TimeSequenceTest
	{
		[Test]
		public void Test0_GetMarks()
		{
			TimeSequence sequence = new TimeSequence(
				new List<float> { 0, 1, 2, 3, 4, 4, 4, 4, 4, 4, 5, 6, 7, 8, 9});
			Assert.AreEqual(sequence.Length, 9.0f);
			Assert.AreEqual(sequence.GetMarksBetween(0, 1), new List<float> { 0 });
			Assert.AreEqual(sequence.GetMarksBetween(0.5f, 1), new List<float>());
			Assert.AreEqual(sequence.GetMarksBetween(0.5f, 1.5f), new List<float> { 1.0f });
			Assert.AreEqual(sequence.GetMarksBetween(4, 4.1f), new List<float> { 4, 4, 4, 4, 4, 4 });
			Assert.AreEqual(sequence.GetMarksBetween(8.5f, 9.5f), new List<float> { 9.0f });
			Assert.IsNull(sequence.GetMarksBetween(9.1f, 10.5f));
		}
	}
}
