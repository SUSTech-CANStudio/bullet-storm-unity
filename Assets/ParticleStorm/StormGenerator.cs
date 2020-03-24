using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Core;

namespace ParticleStorm
{
	public delegate Coroutine CoroutineStarter(IEnumerator coroutine);

	public class StormGenerator : MonoBehaviour
	{
		public void Generate(Storm storm)
		{
			StartCoroutine(storm.Generate(transform, StartCoroutine));
		}
	}
}
