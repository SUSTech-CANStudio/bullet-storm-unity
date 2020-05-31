using ParticleStorm.Core;
using ParticleStorm.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm
{
	/// <summary>
	/// Storm is a series behaviors of particle.
	/// A storm is consist of multiple <see cref="StormBehavior"/>s,
	/// and each behavior can emit a series of <see cref="Particle"/>s,
	/// whose initial parameters described by <see cref="EmitList"/>.
	/// </summary>
	public class Storm : Named<Storm>
	{
		/// <summary>
		/// True if the behaviors of the storm already sorted.
		/// </summary>
		public bool Sorted { get; private set; }

		/// <summary>
		/// Create a storm without name.
		/// </summary>
		public Storm()
		{
			behaviors = new List<IStormBehavior>();
			Sorted = false;
		}

		/// <summary>
		/// Create a storm.
		/// </summary>
		/// <param name="name">Set a name, and you can always find it by <see cref="Find(string)"/></param>
		public Storm(string name)
		{
			behaviors = new List<IStormBehavior>();
			Sorted = false;
			Name = name;
		}

		/// <summary>
		/// Add a new behavior for the storm.
		/// </summary>
		/// <param name="startTime">Behavior start time.</param>
		/// <param name="emitList">Particles' status when emitting.</param>
		/// <param name="particle">The partical to emit.</param>
		/// <param name="gap">Time between two emissions.</param>
		/// <returns></returns>
		public Storm AddBehavior(float startTime, EmitList emitList, Particle particle, float gap = 0)
		{
			if (emitList == null)
				throw new ArgumentNullException(nameof(emitList));
			StormBehavior behavior = new StormBehavior(emitList.List, particle, startTime);
			if (gap > 0)
			{
				behavior.EmitGap = gap;
			}
			behaviors.Add(behavior);
			Sorted = false;
			return this;
		}

		/// <summary>
		/// Add a new behavior for the storm.
		/// </summary>
		/// <param name="startTime">Behavior start time.</param>
		/// <param name="emitParams">Emissions' parameters during the behavior.</param>
		/// <param name="particle">The partical to emit.</param>
		/// <param name="gap">Time between two emissions.</param>
		/// <returns>The storm itself.</returns>
		[Obsolete("Use EmitList instead of List<EmitParams>")]
		public Storm AddBehavior(float startTime, List<EmitParams> emitParams, IParticle particle, float gap = 0)
		{
			if (emitParams == null)
				throw new ArgumentNullException(nameof(emitParams));
			StormBehavior behavior = new StormBehavior(emitParams, particle, startTime);
			if (gap > 0)
			{
				behavior.EmitGap = gap;
			}
			behaviors.Add(behavior);
			Sorted = false;
			return this;
		}

		/// <summary>
		/// Sort behaviors in the storm.
		/// Behaviors will also be sorted automaticly by the first time storm generates..
		/// </summary>
		public void Sort()
		{
			if (!Sorted)
			{
				behaviors.Sort();
				Sorted = true;
			}
		}

		internal IEnumerator Generate(Transform transform, CoroutineStarter coroutineStarter)
		{
			Sort();
			float startTime = Time.time;
			int i = 0;
			// Generate.
			while (i < behaviors.Count)
			{
				if (Time.time - startTime >= behaviors[i].StartTime)
				{
					coroutineStarter(behaviors[i].Execute(transform, startTime));
					i++;
				}
				else
					yield return null;
			}
		}
		
		private readonly List<IStormBehavior> behaviors;
	}
}
