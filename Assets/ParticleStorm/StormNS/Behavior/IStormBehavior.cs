using ParticleStorm.ParticleNS;
using System;
using System.Collections;
using UnityEngine;

namespace ParticleStorm.StormNS.Behavior
{
	public interface IStormBehavior : IComparable<IStormBehavior>
	{
		/// <summary>
		/// Start time of the behavior (related to storm start time).
		/// </summary>
		float StartTime { get; }

		/// <summary>
		/// End time of the behavior (related to storm start time).
		/// </summary>
		float EndTime { get; }

		/// <summary>
		/// Referenced particle of this behavior.
		/// </summary>
		ParticleNS.Particle Referenced { get; }

		/// <summary>
		/// Execute the behavior.
		/// </summary>
		/// <param name="psc"></param>
		/// <param name="transform"></param>
		/// <param name="stormStartTime"></param>
		/// <returns></returns>
		IEnumerator Execute(ParticleSystemController psc, Transform transform, float stormStartTime);
	}
}
