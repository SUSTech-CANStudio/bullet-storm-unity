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
	public class Storm
	{
		/// <summary>
		/// Name of the storm.
		/// </summary>
		public string name { get => m_Name; set => Register(value); }
		/// <summary>
		/// True if the behaviors of the storm already sorted.
		/// </summary>
		public bool sorted { get; private set; }

		/// <summary>
		/// Create a storm without name.
		/// </summary>
		public Storm()
		{
			behaviors = new List<IStormBehavior>();
			sorted = false;
		}

		/// <summary>
		/// Create a storm.
		/// </summary>
		/// <param name="name">Set a name, and you can always find it by <see cref="Find(string)"/></param>
		public Storm(string name)
		{
			behaviors = new List<IStormBehavior>();
			sorted = false;
			Register(name);
		}

		~Storm() { if (name != null) dict.Remove(name); }

		/// <summary>
		/// Find a storm by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"/>
		public static Storm Find(string name)
		{
			if (dict.TryGetValue(name, out Storm storm))
				return storm;
			else
				throw new KeyNotFoundException("No storm named " + name);
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
			StormBehavior behavior = new StormBehavior(emitList.list, particle, startTime);
			if (gap > 0)
			{
				behavior.emitGap = gap;
			}
			behaviors.Add(behavior);
			sorted = false;
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
		[Obsolete]
		public Storm AddBehavior(float startTime, List<EmitParams> emitParams, IParticle particle, float gap = 0)
		{
			StormBehavior behavior = new StormBehavior(emitParams, particle, startTime);
			if (gap > 0)
			{
				behavior.emitGap = gap;
			}
			behaviors.Add(behavior);
			sorted = false;
			return this;
		}

		/// <summary>
		/// Sort behaviors in the storm.
		/// Behaviors will also be sorted automaticly by the first time storm generates..
		/// </summary>
		public void Sort()
		{
			if (!sorted)
			{
				behaviors.Sort();
				sorted = true;
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
				if (Time.time - startTime >= behaviors[i].GetStartTime())
				{
					coroutineStarter(behaviors[i].Execute(transform, startTime));
					i++;
				}
				else
					yield return null;
			}
		}

		/// <summary>
		/// Set and register name.
		/// </summary>
		/// <param name="name"></param>
		private void Register(string name)
		{
			if (name == null)
				Debug.LogError("Storm name can't be null");
			else if (dict.ContainsKey(name))
				Debug.LogError("Storm " + name + " already exists.");
			else
			{
				if (m_Name != null)
					dict.Remove(m_Name);
				dict.Add(name, this);
				m_Name = name;
			}
		}
		
		private readonly List<IStormBehavior> behaviors;
		private string m_Name;
		private static readonly Dictionary<string, Storm> dict = new Dictionary<string, Storm>();
	}
}
