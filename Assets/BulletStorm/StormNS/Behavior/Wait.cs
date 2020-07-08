using BulletStorm.BulletNS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletStorm.StormNS.Behavior
{
	/// <summary>
	/// Get particles wait.
	/// </summary>
	public class Wait : StormBehavior
	{
		public override float StartTime => startTime;
		public override float FinishTime => finishTime;
		public override Bullet Referenced => referenced;

		private readonly float startTime;
		private readonly float finishTime;
		private readonly Bullet referenced;

		public Wait(float start, float finish, Bullet particle)
		{
			startTime = start;
			finishTime = finish;
			referenced = particle;
		}

		public override IEnumerator Execute(BulletSystemController psc, Transform transform)
		{
			psc.ParticleSystem.Pause();
			yield return new WaitForSeconds(finishTime - startTime);
			psc.ParticleSystem.Play();
		}
	}
}
