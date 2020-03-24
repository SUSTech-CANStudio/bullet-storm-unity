using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Core;
using ParticleStorm.Util;

namespace ParticleStorm.Factories
{
	public class ParticleFactory
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static ParticleFactory instance => lazy.Value;

		/// <summary>
		/// Tag of particle storm game objects.
		/// </summary>
		public string particleTag { get => Settings.particleTag; }

		private readonly Dictionary<string, IParticle> particles = new Dictionary<string, IParticle>();

		/// <summary>
		/// Get particle by name.
		/// </summary>
		/// <param name="name">Particle name.</param>
		/// <returns>If no particle found, return null.</returns>
		public static IParticle GetParticle(string name)
		{
			if (instance.particles.TryGetValue(name, out IParticle particle))
				return particle;
			else
				Debug.LogError("No particle named " + name);
			return default;
		}

		/// <summary>
		/// Create particle by name and type.
		/// </summary>
		/// <typeparam name="T">Particle type.</typeparam>
		/// <param name="name">Particle name.</param>
		/// <returns></returns>
		public static T NewParticle<T>(string name) where T : MonoBehaviour, IParticle
		{
			if (instance.particles.ContainsKey(name))
			{
				Debug.LogError("Particle '" + name + "' already exists.");
				return default;
			}
			else
			{
				var go = new GameObject(name, typeof(T));
				var p = go.GetComponent<T>();
				go.tag = instance.particleTag;
				instance.particles.Add(name, p);
				return p;
			}
		}

		/// <summary>
		/// Create particle from prefeb.
		/// </summary>
		/// <param name="prefeb"></param>
		/// <returns></returns>
		public static IParticle NewParticle(GameObject prefeb)
		{
			var go = GameObject.Instantiate(prefeb) as GameObject;
			if (go.TryGetComponent(out IParticle particle))
			{
				if (instance.particles.ContainsKey(go.name))
				{
					Debug.LogError("Particle '" + go.name + "' already exists.");
					return default;
				}
				instance.particles.Add(go.name, particle);
				go.tag = instance.particleTag;
				return particle;
			}
			else
			{
				Debug.LogError("Can't find particle on prefeb");
				return default;
			}
		}

		private ParticleFactory() { }
		private static readonly Lazy<ParticleFactory>
			lazy = new Lazy<ParticleFactory>
			(() => new ParticleFactory());
	}
}
