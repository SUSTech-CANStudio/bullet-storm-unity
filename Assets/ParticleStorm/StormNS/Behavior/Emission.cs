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
		public override float StartTime => startTime;

		/// <summary>
		/// Behavior finish time relative to storm time.
		/// </summary>
		public override float FinishTime => startTime + sequence.Length;

		/// <summary>
		/// Behavior total time.
		/// </summary>
		public float TotalTime => sequence.Length;

		/// <summary>
		/// The particle to emit.
		/// </summary>
		public override Particle Referenced { get => referenced; }

		private readonly float startTime;
		private readonly List<EmitParams> emitParams;
		private readonly TimeSequence sequence;
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
			this.startTime = startTime;
			sequence = TimeSequence.EqualSpace(emits.Count, 0);
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
			this.startTime = startTime;
			sequence = TimeSequence.EqualSpace(emits.Count, gap);
		}

		public override IEnumerator Execute(ParticleSystemController psc, Transform transform)
		{
			float start = Time.fixedTime;
			int index = 0;
			List<float> marks;
			while (Time.fixedTime - start - Time.fixedDeltaTime <= sequence.Length)
			{
				marks = sequence.GetMarks(Time.fixedTime - start, Time.fixedDeltaTime);
				foreach (float i in marks) { Debug.Log(i); }
				for (int i = 0; i < marks.Count; i++)
				{
					if (psc.IsOrigin)
					{
						psc.Emit(emitParams[i + index].RelativeParams(transform));
					}
					else
					{
						psc.Emit(emitParams[i + index]);
					}
				}
				index += marks.Count;
				yield return new WaitForFixedUpdate();
			}
		}
	}
}
