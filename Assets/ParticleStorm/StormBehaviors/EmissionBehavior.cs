using ParticleStorm.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm.Core;

namespace ParticleStorm.StormBehaviors
{
	public class EmissionBehavior : IStormBehavior
	{
		/// <summary>
		/// Behavior start time relative to storm time.
		/// </summary>
		public float StartTime { get => duration.Start; }
		/// <summary>
		/// Behavior end time relative to storm time.
		/// </summary>
		public float EndTime { get => duration.End; set => duration.End = value; }
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
		public Particle Referenced { get; private set; }

		private readonly List<EmitParams> emitParams;
		private readonly Duration duration;

		/// <summary>
		/// Create a storm behavior.
		/// </summary>
		/// <param name="emits">The emit list for particle emissions.</param>
		/// <param name="particle">The particle to emit.</param>
		/// <param name="startTime">Start time of the behavior, relative to the storm time.</param>
		public EmissionBehavior(EmitList emits, Particle particle, float startTime)
		{
			emitParams = emits.List;
			Referenced = particle;
			duration = new Duration(startTime, emits.Count);
		}

		/// <summary>
		/// Create a storm behavior.
		/// </summary>
		/// <param name="emits">The emit list for particle emissions.</param>
		/// <param name="particle">The particle to emit.</param>
		/// <param name="startTime">Start time of the behavior, relative to the storm time.</param>
		/// <param name="gap">Time between two emissions.</param>
		public EmissionBehavior(EmitList emits, Particle particle, float startTime, float gap)
		{
			emitParams = emits.List;
			Referenced = particle;
			duration = new Duration(startTime, emits.Count);
			EmitGap = gap;
		}

		public IEnumerator Execute(ParticleSystemController psc, Transform transform, float stormStartTime)
		{
			Duration duration = new Duration(this.duration);
			int begin, end;
			if (psc.IsOrigin)
			{
				while (!duration.Finished)
				{
					begin = duration.PastEventCount;
					end = duration.GetHappenedEventCount(Time.time - stormStartTime);
					for (int i = begin; i < end; i++)
					{
						psc.Emit(emitParams[i].RelativeParams(transform));
					}
					yield return null;
				}
			}
			else
			{
				while (!duration.Finished)
				{
					begin = duration.PastEventCount;
					end = duration.GetHappenedEventCount(Time.time - stormStartTime);
					for (int i = begin; i < end; i++)
					{
						psc.Emit(emitParams[i]);
					}
					yield return null;
				}
			}
		}

		public int CompareTo(IStormBehavior other)
		{
			if (StartTime < other.StartTime)
				return -1;
			else
				return 1;
		}
	}
}
