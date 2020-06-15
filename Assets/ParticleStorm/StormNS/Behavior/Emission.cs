using ParticleStorm.ParticleNS;
using ParticleStorm.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm.StormNS.Behavior
{
	/// <summary>
	/// Emit particles.
	/// </summary>
	public class Emission : StormBehavior
	{
		public override float StartTime => startTime;
		public override float FinishTime => startTime + sequence.Length;
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
			EmitParams accurate;
			while (index < sequence.Count)
			{
				marks = sequence.GetMarks(index, Time.fixedTime - start);
				for (int i = 0; i < marks.Count; i++)
				{
					accurate = emitParams[i + index].Lag(Time.fixedTime - marks[i]);
					if (psc.IsOrigin)
					{
						psc.Emit(accurate.RelativeParams(transform));
					}
					else
					{
						psc.Emit(accurate);
					}
					if (psc.EmissionEvent != null)
					{
						psc.EmissionEvent.OnParticleEmission(transform, accurate);
					}
				}
				index += marks.Count;
				yield return new WaitForFixedUpdate();
			}
		}
	}
}
