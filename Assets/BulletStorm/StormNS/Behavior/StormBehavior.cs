using BulletStorm.BulletNS;
using System;
using System.Collections;
using UnityEngine;

namespace BulletStorm.StormNS.Behavior
{
	public abstract class StormBehavior : IComparable<StormBehavior>
	{
		/// <summary>
		/// Start time of the behavior (related to storm start time).
		/// </summary>
		public abstract float StartTime { get; }

		/// <summary>
		/// Finish time of the behavior (related to storm start time).
		/// </summary>
		public abstract float FinishTime { get; }

		/// <summary>
		/// Referenced particle of this behavior.
		/// </summary>
		public abstract Bullet Referenced { get; }

		/// <summary>
		/// Execute the behavior.
		/// </summary>
		/// <param name="psc"></param>
		/// <param name="transform"></param>
		/// <param name="stormStartTime"></param>
		/// <returns></returns>
		public abstract IEnumerator Execute(BulletSystemController psc, Transform transform);

		public int CompareTo(StormBehavior other)
		{
			if (StartTime < other.StartTime) { return -1; }
			else { return 1; }
		}
	}
}
