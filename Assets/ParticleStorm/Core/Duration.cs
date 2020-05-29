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
		public float Start { get; private set; }

		/// <summary>
		/// Total time.
		/// </summary>
		public float Total { get; set; }

		/// <summary>
		/// Events number during the duration.
		/// </summary>
		public int EventCount { get; set; }

		/// <summary>
		/// End time.
		/// </summary>
		public float End { get => Start + Total; set => Total = value - Start; }

		/// <summary>
		/// Time between two events.
		/// </summary>
		public float Gap { get => Total / EventCount; set => Total = EventCount * value; }

		/// <summary>
		/// Already past event number after last <see cref="GetHappenedEventCount"/>.
		/// </summary>
		public int PastEventCount { get; private set; }

		/// <summary>
		/// True if the duration finished.
		/// </summary>
		public bool Finished { get => PastEventCount >= EventCount; }

		/// <summary>
		/// Create a duration.
		/// </summary>
		/// <param name="start">Start time of the duration.</param>
		/// <param name="eventCount">Events number during the duration.</param>
		public Duration(float start, int eventCount)
		{
			this.Start = start;
			this.Total = 0;
			this.EventCount = eventCount;
			this.PastEventCount = 0;
		}

		public Duration(Duration duration)
		{
			if (duration == null)
				throw new ArgumentNullException(nameof(duration));
			Start = duration.Start;
			Total = duration.Total;
			EventCount = duration.EventCount;
			PastEventCount = 0;
		}

		/// <summary>
		/// Get the number of events that should have happened at current time.
		/// </summary>
		/// <param name="currentTime"></param>
		/// <returns></returns>
		public int GetHappenedEventCount(float currentTime)
		{
			int result;

			if (currentTime >= End)
			{
				result = EventCount;
			}
			else
			{
				result = Mathf.CeilToInt((currentTime - Start) / Gap);
				if (result > EventCount)
					result = EventCount;
			}
			PastEventCount = result;
			return result;
		}
	}
}
