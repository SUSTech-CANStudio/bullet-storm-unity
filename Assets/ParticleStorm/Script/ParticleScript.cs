using System;
using System.Collections.Generic;

namespace ParticleStorm.Script
{
	/// <summary>
	/// Manages all scripts about particle.
	/// </summary>
	internal static class ParticleScript
	{
		/// <summary>
		/// Add a event for update.
		/// </summary>
		/// <param name="event"></param>
		/// <exception cref="ArgumentException"/>
		public static void AddUpdateScript(UpdateEvent @event)
		{
			try { UpdateScripts.Add(@event.Name, @event); }
			catch (ArgumentException)
			{
				throw new ArgumentException("Update script " + @event.Name + " already exists.", @event.ToString());
			}
		}

		/// <summary>
		/// Add a event for collision.
		/// </summary>
		/// <param name="event">The collision event</param>
		public static void AddCollisionScript(CollisionEvent @event)
		{
			try { CollisionScripts.Add(@event.Name, @event); }
			catch (ArgumentException)
			{
				throw new ArgumentException("Update script " + @event.Name + " already exists.", @event.ToString());
			}
		}

		/// <summary>
		/// Add a script as trigger.
		/// </summary>
		/// <param name="script"></param>
		/// <exception cref="ArgumentException"/>
		[Obsolete("TriggerModule is abandoned, use CollisionModule instead.")]
		public static void AddTrigger(ParticleUpdateScript script)
		{
			try { Triggers.Add(script.Method.Name, script); }
			catch (ArgumentException)
			{
				throw new ArgumentException("Trigger " + script.Method.Name + " already exists.", script.ToString());
			}
		}

		/// <summary>
		/// Get an update script by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="ScriptNotFoundException"/>
		internal static UpdateEvent GetUpdateScript(string name)
		{
			try
			{
				return UpdateScripts[name];
			}
			catch (KeyNotFoundException)
			{
				throw new ScriptNotFoundException(name, "update script");
			}
		}

		/// <summary>
		/// Get an collision script by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal static CollisionEvent GetCollisionScript(string name)
		{
			try
			{
				return CollisionScripts[name];
			}
			catch (KeyNotFoundException)
			{
				throw new ScriptNotFoundException(name, "collision script");
			}
		}

		/// <summary>
		/// Try get an update event by name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="event"></param>
		/// <returns>Succeeded or not.</returns>
		internal static bool TryGetUpdateScript(string name, out UpdateEvent @event)
			=> UpdateScripts.TryGetValue(name, out @event);

		/// <summary>
		/// Try get a colllision event by name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="event"></param>
		/// <returns></returns>
		internal static bool TryGetCollisionScript(string name, out CollisionEvent @event)
			=> CollisionScripts.TryGetValue(name, out @event);

		private static readonly Dictionary<string, UpdateEvent> UpdateScripts
			= new Dictionary<string, UpdateEvent>();
		private static readonly Dictionary<string, CollisionEvent> CollisionScripts
			= new Dictionary<string, CollisionEvent>();
		[Obsolete("Triggers are abandoned.")]
		private static readonly Dictionary<string, ParticleUpdateScript> Triggers
			= new Dictionary<string, ParticleUpdateScript>();
	}
}
