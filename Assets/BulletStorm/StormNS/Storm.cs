using BulletStorm.BulletNS;
using BulletStorm.StormNS.Behavior;
using BulletStorm.Util;
using System.Collections.Generic;

namespace BulletStorm.StormNS
{
	/// <summary>
	/// Stores a series of behaviors.
	/// </summary>
	public class Storm : Named<Storm>
	{
		/// <summary>
		/// Create a storm without name.
		/// </summary>
		public Storm() { }

		/// <summary>
		/// Create a storm.
		/// </summary>
		/// <param name="name">Name of the storm</param>
		public Storm(string name) => Name = name;

		/// <summary>
		/// Add a behavior to this storm.
		/// </summary>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public Storm AddBehavior(StormBehavior behavior)
		{
			if (behavior.Referenced != null && !translator.ContainsKey(behavior.Referenced))
			{
				translator.Add(behavior.Referenced, null);
			}
			behaviors.Add(behavior, behavior.Referenced);
			return this;
		}

		/// <summary>
		/// Get an excuter as children of given transform.
		/// </summary>
		/// <param name="generator">The storm generator</param>
		/// <param name="useLocalExecuter">
		/// The executer uses copy <see cref="BulletSystemController"/> if true,
		/// else use origin.
		/// </param>
		/// <returns>A new executer</returns>
		internal StormExecuter GetExecuter(StormGenerator generator, bool useLocalExecuter)
		{
			var casted = new SortedList<StormBehavior, BulletSystemController>(behaviors.Count);
			if (useLocalExecuter)
			{
				foreach (var particle in new List<BulletNS.Bullet>(translator.Keys))
				{
					translator[particle] = particle.GetCopy(generator.transform);
				}

				for (int i = 0; i < behaviors.Count; i++)
				{
					casted.Add(behaviors.Keys[i], translator[behaviors.Values[i]]);
				}
			}
			else
			{
				for (int i = 0; i < behaviors.Count; i++)
				{
					casted.Add(behaviors.Keys[i], behaviors.Values[i].Origin);
				}

			}
			return new StormExecuter(casted, generator);
		}

		private readonly SortedList<StormBehavior, BulletNS.Bullet> behaviors = new SortedList<StormBehavior, BulletNS.Bullet>();
		private readonly Dictionary<BulletNS.Bullet, BulletSystemController> translator = new Dictionary<BulletNS.Bullet, BulletSystemController>();
	}
}
