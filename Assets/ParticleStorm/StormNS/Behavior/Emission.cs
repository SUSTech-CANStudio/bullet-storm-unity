using ParticleStorm.ParticleNS;
using ParticleStorm.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm.StormNS.Behavior
{
	public class Emission : StormBehavior
	{
		/// <summary>
		/// Behavior start time relative to storm time.
		/// </summary>
		public override float StartTime { get => duration.Start; }
		/// <summary>
		/// Behavior finish time relative to storm time.
		/// </summary>
		public override float FinishTime { get => duration.End; }
		/// <summary>
		/// Behavior total time.
		/// </summary>
		public float TotalTime { get => duration.Total; set => duration.Total = value; }
		/// <summary>
		/// Time between two emissions.
		/// </summary>
		public float EmitGap { get => duration.Gap; set => duration.Gap = value; }
		/// <summary>
		/// The particle to emit.
		/// </summary>
		public override Particle Referenced { get => referenced; }

		private readonly List<EmitParams> emitParams;
		private readonly Duration duration;
		private readonly Particle referenced;

		/// <summary>
		/// Create a storm behavior.
		/// </summary>
		/// <param name="emits">The emit list for particle emissions.</param>
		/// <param name="particle">The particle to emit.</param>
		/// <param name="startTime">Start time of the behavior, relative to the storm time.</param>
		public Emission(EmitList emits, Particle particle, float startTime)
		{
			emitParams = emits.List;
			referenced = particle;
			duration = new Duration(startTime, emits.Count);
		}

		/// <summary>
		/// Create a storm behavior.
		/// </summary>
		/// <param name="emits">The emit list for particle emissions.</param>
		/// <param name="particle">The particle to emit.</param>
		/// <param name="startTime">Start time of the behavior, relative to the storm time.</param>
		/// <param name="gap">Time between two emissions.</param>
		public Emission(EmitList emits, Particle particle, float startTime, float gap)
		{
			emitParams = emits.List;
			referenced = particle;
			duration = new Duration(startTime, emits.Count);
			EmitGap = gap;
		}

		public override IEnumerator Execute(ParticleSystemController psc, Transform transform)
		{
			Duration executing = new Duration(duration);
			int begin, end;
			float stormStartTime = Time.time - executing.Start;
			if (psc.IsOrigin)
			{
				while (!executing.Finished)
				{
					begin = executing.PastEventCount;
					end = executing.GetHappenedEventCount(Time.time - stormStartTime);
					for (int i = begin; i < end; i++)
					{
						psc.Emit(emitParams[i].RelativeParams(transform));
					}
					yield return new WaitForFixedUpdate();
				}
			}
			else
			{
				while (!executing.Finished)
				{
					begin = executing.PastEventCount;
					end = executing.GetHappenedEventCount(Time.time - stormStartTime);
					for (int i = begin; i < end; i++)
					{
						psc.Emit(emitParams[i]);
					}
					yield return new WaitForFixedUpdate();
				}
			}
		}
	}
}
