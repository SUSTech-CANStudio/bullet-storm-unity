using System.Collections;
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
		/// <summary>
		/// Begin to generate a storm.
		/// </summary>
		/// <param name="storm"></param>
		public void Generate(Storm storm)
		{
			StartCoroutine(storm.Generate(transform, StartCoroutine));
		}
	}
}
