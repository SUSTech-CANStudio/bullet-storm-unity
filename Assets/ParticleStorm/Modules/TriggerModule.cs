#pragma warning disable 0649

using ParticleStorm.Core;
using ParticleStorm.Script;
using System;
using UnityEngine;

namespace ParticleStorm.Modules
{
	[Serializable]
	[Obsolete("Use collision module instead")]
	internal struct TriggerModule : IParticleModule
	{
		[Tooltip("Enable trigger on particle.")]
		public bool enabled;
		[Tooltip("Choose what action to perform when particles enter the trigger volume.")]
		public ParticleSystemOverlapAction enter;
		[Tooltip("Script name for particle enter the trigger volume.")]
		public string enterTrigger;
		[Tooltip("Choose what action to perform when particles exit the trigger volume.")]
		public ParticleSystemOverlapAction exit;
		[Tooltip("Script name for particle exit the trigger volume.")]
		public string exitTrigger;
		[Tooltip("Choose what action to perform when particles inside the trigger volume.")]
		public ParticleSystemOverlapAction inside;
		[Tooltip("Script name for particle inside the trigger volume.")]
		public string insideTrigger;
		[Tooltip("Choose what action to perform when particles outside the trigger volume.")]
		public ParticleSystemOverlapAction outside;
		[Tooltip("Script name for particle outside the trigger volume.")]
		public string outsideTrigger;

		internal ParticleUpdateScript enterTriggerScript;
		internal ParticleUpdateScript exitTriggerScript;
		internal ParticleUpdateScript insideTriggerScript;
		internal ParticleUpdateScript outsideTriggerScript;

		public void ApplicateOn(PSParticleSystem ps)
		{
			var module = ps.GetComponent<ParticleSystem>().trigger;
			module.enabled = enabled;
			if (enabled)
			{
				module.enter = enter;
				module.exit = exit;
				module.inside = inside;
				module.outside = outside;
				//if (enter == ParticleSystemOverlapAction.Callback)
				//	enterTriggerScript = ParticleScript.GetUpdateScript(enterTrigger, true);
				//if (exit == ParticleSystemOverlapAction.Callback)
				//	exitTriggerScript = ParticleScript.GetUpdateScript(exitTrigger, true);
				//if (inside == ParticleSystemOverlapAction.Callback)
				//	insideTriggerScript = ParticleScript.GetUpdateScript(insideTrigger, true);
				//if (outside == ParticleSystemOverlapAction.Callback)
				//	outsideTriggerScript = ParticleScript.GetUpdateScript(outsideTrigger, true);
			}
			//ps.triggerModule = this;
		}

		public void ApplicateOn(GOParticleSystem go)
		{
			throw new NotImplementedException();
		}
	}
}
