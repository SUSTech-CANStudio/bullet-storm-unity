using ParticleStorm.ParticleNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.StormNS.Behavior
{
	/// <summary>
	/// Get particles wait.
	/// </summary>
	public class Wait : StormBehavior
	{
		public override float StartTime => startTime;
		public override float FinishTime => finishTime;
		public override Particle Referenced => referenced;

		private readonly float startTime;
		private readonly float finishTime;
		private readonly Particle referenced;

		public Wait(float start, float finish, Particle particle)
		{
			startTime = start;
			finishTime = finish;
			referenced = particle;
		}

		public override IEnumerator Execute(ParticleSystemController psc, Transform transform)
		{
			psc.ParticleSystem.Pause();
			yield return new WaitForSeconds(finishTime - startTime);
			psc.ParticleSystem.Play();
		}
	}
}
