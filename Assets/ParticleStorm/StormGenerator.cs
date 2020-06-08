using ParticleStorm.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleStorm
{
	public delegate Coroutine CoroutineStarter(IEnumerator coroutine);

	/// <summary>
	/// Storm generator is used to generate storms at its location.<para/>
	/// See also:
	/// <seealso cref="Generate(Storm)"/>
	/// </summary>
	[AddComponentMenu("StormGenerator")]
	public class StormGenerator : MonoBehaviour
	{
		public bool UseLocalExecuter { get => useLocalExecuter; private set => useLocalExecuter = value; }

		/// <summary>
		/// Begin to generate a storm.
		/// </summary>
		/// <param name="storm"></param>
		public void Generate(Storm storm)
		{
			if (!executers.TryGetValue(storm, out StormExecuter executer))
			{
				executer = storm.GetExecuter(this, UseLocalExecuter);
				executers.Add(storm, executer);
			}
			StartCoroutine(executer.Start());
		}

		[SerializeField]
		[Tooltip("Copy the particle systems to the generator if true")]
		private bool useLocalExecuter;

		private readonly Dictionary<Storm, StormExecuter> executers = new Dictionary<Storm, StormExecuter>();
	}
}
