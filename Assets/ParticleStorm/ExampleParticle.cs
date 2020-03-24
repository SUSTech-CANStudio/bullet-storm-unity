using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParticleStorm.Core;
using UnityEngine;

namespace ParticleStorm
{
	public class ExampleParticle : APParticle
	{
		public override void ParticleStart()
		{
			ParticleSystem.MainModule main = this.main;
			ParticleSystem.TriggerModule trigger = this.trigger;

			//// Do your config here.
			//main.maxParticles = 1000;
			//main.startLifetime = 10;
			//trigger.enabled = true;
			//trigger.enter = ParticleSystemOverlapAction.Callback;
			//trigger.inside = ParticleSystemOverlapAction.Kill;
			//trigger.outside = ParticleSystemOverlapAction.Ignore;
			//trigger.exit = ParticleSystemOverlapAction.Ignore;
			//updateMode = Util.UpdateMode.LATEUPDATE;
			//material = ...;

		}

		public override void ParticleUpdate(ref ParticleSystem.Particle self)
		{
			// Write sctipt here.
		}
	}
}
