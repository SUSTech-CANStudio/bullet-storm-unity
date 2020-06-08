#pragma warning disable 0649

using ParticleStorm.Core;
using ParticleStorm.Script;
using System;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	internal struct ScriptModule : IParticleModule
	{
		[Tooltip("Enable update script on the particle.")]
		public bool enabled;
		[Tooltip("Update event name.\nSee: Script.UpdateEvent")]
		public string updateEvent;

		public void ApplicateOn(ParticleSystemController psc)
		{
			if (enabled)
			{
				var onUpdate = UpdateEvent.Find(updateEvent);
				if (onUpdate == null)
				{
					Debug.LogWarning("No update event named '" + onUpdate + "'");
				}
				else { psc.UpdateEvent = onUpdate; }
			}
			else { psc.UpdateEvent = null; }
		}
	}
}
