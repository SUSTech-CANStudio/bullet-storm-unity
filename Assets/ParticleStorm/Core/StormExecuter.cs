using JetBrains.Annotations;
using ParticleStorm.Modules;
using ParticleStorm.Script;
using ParticleStorm.Util;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using ParticleStorm.StormBehaviors;

namespace ParticleStorm.Core
{
	/// <summary>
	/// Executes storm behaviors.
	/// </summary>
	class StormExecuter
	{
		private readonly SortedList<IStormBehavior, ParticleSystemController> behaviors;
		private readonly StormGenerator generator;

		public StormExecuter(
			[NotNull]SortedList<IStormBehavior, ParticleSystemController> behaviors,
			[NotNull]StormGenerator generator)
		{
			this.behaviors = behaviors;
			this.generator = generator;
		}

		public IEnumerator Start()
		{
			float startTime = Time.time;
			int i = 0;
			// Generate.
			while (i < behaviors.Count)
			{
				if (Time.time - startTime >= behaviors.Keys[i].StartTime)
				{
					generator.StartCoroutine(
						behaviors.Keys[i].Execute(behaviors.Values[i], generator.transform, startTime));
					i++;
				}
				else { yield return null; }
			}
		}
	}
}
