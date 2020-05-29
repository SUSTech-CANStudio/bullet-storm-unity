using ParticleStorm.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm.Core
{
	class StormBehavior : IStormBehavior
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

		private readonly List<EmitParams> emitParams;
		private readonly IParticle particle;
		private readonly Duration duration;

		/// <summary>
		/// Create a storm behavior.
		/// </summary>
		/// <param name="emits">The emit parameters for particle emissions.</param>
		/// <param name="particle">The particle to emit.</param>
		/// <param name="startTime">Start time of the behavior, relative to the storm time.</param>
		public StormBehavior(List<EmitParams> emits, IParticle particle, float startTime)
		{
			emitParams = emits;
			this.particle = particle;
			duration = new Duration(startTime, emits.Count);
		}

		/// <summary>
		/// Execute the storm behavior.
		/// </summary>
		/// <param name="transform">Storm generator transform.</param>
		/// <param name="stormStartTime">Start time of the whole storm.</param>
		/// <returns></returns>
		public IEnumerator Execute(Transform transform, float stormStartTime)
		{
			Duration duration = new Duration(this.duration);
			while (!duration.Finished)
			{
				int begin = duration.PastEventCount;
				int end = duration.GetHappenedEventCount(Time.time - stormStartTime);
				for (int i = begin; i < end; i++)
				{
					particle.Emit(emitParams[i].RelativeParams(transform), 1);
				}
				yield return null;
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
