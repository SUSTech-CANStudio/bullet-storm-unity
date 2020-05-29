using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.Core
{
	interface IStormBehavior : IComparable<IStormBehavior>
	{
		float StartTime { get; }
		IEnumerator Execute(Transform transform, float stormStartTime);
	}
}
