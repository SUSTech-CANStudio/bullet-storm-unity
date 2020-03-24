using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.Core
{
	/// <summary>
	/// A period of time, can set events to happen evenly.
	/// </summary>
	public class Duration
	{
		/// <summary>
		/// Start time.
		/// </summary>
		public float start { get; private set; }

		/// <summary>
		/// Total time.
		/// </summary>
		public float total { get; set; }

		/// <summary>
		/// Events number during the duration.
		/// </summary>
		public int eventCount { get; set; }

		/// <summary>
		/// End time.
		/// </summary>
		public float end { get => start + total; set => total = value - start; }

		/// <summary>
		/// Time between two events.
		/// </summary>
		public float gap { get => total / eventCount; set => total = eventCount * value; }

		/// <summary>
		/// Already past event number after last <see cref="GetHappenedEventCount"/>.
		/// </summary>
		public int pastEventCount { get; private set; }

		/// <summary>
		/// True if the duration finished.
		/// </summary>
		public bool finished { get => pastEventCount >= eventCount; }

		/// <summary>
		/// Create a duration.
		/// </summary>
		/// <param name="start">Start time of the duration.</param>
		/// <param name="eventCount">Events number during the duration.</param>
		public Duration(float start, int eventCount)
		{
			this.start = start;
			this.total = 0;
			this.eventCount = eventCount;
			this.pastEventCount = 0;
		}

		public Duration(Duration duration)
		{
			start = duration.start;
			total = duration.total;
			eventCount = duration.eventCount;
			pastEventCount = 0;
		}

		/// <summary>
		/// Get the number of events that should have happened at current time.
		/// </summary>
		/// <param name="currentTime"></param>
		/// <returns></returns>
		public int GetHappenedEventCount(float currentTime)
		{
			int result;

			if (currentTime >= end)
			{
				result = eventCount;
			}
			else
			{
				result = Mathf.CeilToInt((currentTime - start) / gap);
				if (result > eventCount)
					result = eventCount;
			}
			pastEventCount = result;
			return result;
		}
	}
}
