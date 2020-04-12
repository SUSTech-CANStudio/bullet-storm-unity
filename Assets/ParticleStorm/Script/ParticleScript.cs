using System;
using System.Collections.Generic;

namespace ParticleStorm.Script
{
	public static class ParticleScript
	{
		/// <summary>
		/// Script for particles.
		/// </summary>
		/// <param name="particle">The particle status, you can get and modify it in your script.</param>
		public delegate void Script(ParticleStatus particle);

		/// <summary>
		/// Add a script as update script.
		/// </summary>
		/// <param name="script"></param>
		/// <exception cref="ArgumentException"/>
		public static void AddUpdateScript(Script script)
		{
			try { updateScripts.Add(script.Method.Name, script); }
			catch (ArgumentException)
			{
				throw new ArgumentException("Update script " + script.Method.Name + " already exists.", script.ToString());
			}
		}

		/// <summary>
		/// Add a script as trigger.
		/// </summary>
		/// <param name="script"></param>
		/// <exception cref="ArgumentException"/>
		public static void AddTrigger(Script script)
		{
			try { triggers.Add(script.Method.Name, script); }
			catch (ArgumentException)
			{
				throw new ArgumentException("Trigger " + script.Method.Name + " already exists.", script.ToString());
			}
		}

		/// <summary>
		/// Get a script by name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="isTrigger">Get an update script or a trigger.</param>
		/// <returns></returns>
		/// <exception cref="ScriptNotFoundException"/>
		internal static Script GetScript(string name, bool isTrigger = false)
		{
			try
			{
				if (isTrigger)
					return triggers[name];
				else
					return updateScripts[name];
			}
			catch (KeyNotFoundException)
			{
				throw new ScriptNotFoundException(name, isTrigger ? "trigger" : "update script");
			}
		}

		/// <summary>
		/// Try get a script by name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="script"></param>
		/// <param name="isTrigger">Get an update script or a trigger.</param>
		/// <returns>Succeeded or not.</returns>
		internal static bool TryGetScript(string name, out Script script, bool isTrigger = false)
		{
			if (isTrigger)
				return triggers.TryGetValue(name, out script);
			else
				return updateScripts.TryGetValue(name, out script);
		}
		
		private static readonly Dictionary<string, Script> updateScripts = new Dictionary<string, Script>();
		private static readonly Dictionary<string, Script> triggers = new Dictionary<string, Script>();
	}
}
