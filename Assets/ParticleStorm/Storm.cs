using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Factories;
using ParticleStorm.Core;

namespace ParticleStorm
{
	public class Storm
	{
		private readonly List<IStormBehavior> behaviors;
		private bool sorted;
		public readonly string name;

		/// <summary>
		/// Create a storm.
		/// </summary>
		/// <param name="name"></param>
		public Storm(string name)
		{
			behaviors = new List<IStormBehavior>();
			sorted = false;
			this.name = name;
			StormFactory.instance.Register(this);
		}

		/// <summary>
		/// Find a storm by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Storm Find(string name) => StormFactory.instance.GetStorm(name);

		/// <summary>
		/// Add a new behavior for the storm.
		/// </summary>
		/// <param name="startTime">Behavior start time.</param>
		/// <param name="emitParams">Emissions' parameters during the behavior.</param>
		/// <param name="particle">The partical to emit.</param>
		/// <param name="gap">Time between two emissions.</param>
		public void AddBehavior(float startTime, List<EmitParams> emitParams, IParticle particle, float gap = 0)
		{
			StormBehavior behavior = new StormBehavior(emitParams, particle, startTime);
			if (gap > 0)
			{
				behavior.emitGap = gap;
			}
			behaviors.Add(behavior);
			sorted = false;
		}

		public IEnumerator Generate(Transform transform, CoroutineStarter coroutineStarter)
		{
			Debug.Log("Storm '" + name + "' begin");
			float startTime = Time.time;
			int i = 0;
			// Sort behaviors if not sorted.
			if (!sorted)
			{
				behaviors.Sort();
				sorted = true;
			}
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

		public void Export(string file_name, Uri file_path = null)
		{
			throw new NotImplementedException();
		}

		public void Export(string file_name, string file_path = null)
		{
			throw new NotImplementedException();
		}
	}
}
