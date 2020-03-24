using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Core;

namespace ParticleStorm.Factories
{
	class StormFactory
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static StormFactory instance => lazy.Value;

		private readonly Dictionary<string, Storm> storms = new Dictionary<string, Storm>();

		/// <summary>
		/// Get a storm from factory.
		/// </summary>
		/// <param name="name">Storm name.</param>
		/// <returns>Returns null if no such storm found.</returns>
		public Storm GetStorm(string name)
		{
			if (storms.TryGetValue(name, out Storm storm))
				return storm;
			else
				Debug.LogError("No storm named " + name);
			return default;
		}

		public void Register(Storm storm)
		{
			if (storms.ContainsKey(storm.name))
				Debug.LogError("Storm '" + storm.name + "' already exists");
			else
				storms.Add(storm.name, storm);
		}

		private StormFactory() { }
		private static readonly Lazy<StormFactory>
			lazy = new Lazy<StormFactory>
			(() => new StormFactory());
	}
}
