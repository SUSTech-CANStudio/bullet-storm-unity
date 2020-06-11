using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace ParticleStorm.Util
{
	public class TimeSequence
	{
		/// <summary>
		/// Total time of this sequence.
		/// </summary>
		public float Length { get; private set; }

		/// <summary>
		/// Mark count in this sequence.
		/// </summary>
		public int Count => marks.Count;

		private readonly List<float> marks;

		/// <summary>
		/// Create a time sequence by marks.
		/// </summary>
		/// <param name="marks">Marks on the time sequence.</param>
		public TimeSequence([NotNull]List<float> marks)
		{
			this.marks = marks;
			this.marks.Sort();
			if (marks.Count > 0) { Length = marks[marks.Count - 1]; }
			else { Length = 0; }
		}

		/// <summary>
		/// Create a time sequence with equal space between every two marks.
		/// </summary>
		/// <param name="markCount">Number of marks</param>
		/// <param name="space">Time between two marks</param>
		/// <returns></returns>
		static public TimeSequence EqualSpace(int markCount, float space)
		{
			var result = new List<float>();
			for (int i = 0; i < markCount; i++)
			{
				result.Add(i * space);
			}
			return new TimeSequence(result);
		}

		/// <summary>
		/// Get marks during time.
		/// </summary>
		/// <param name="start">Start time (include)</param>
		/// <param name="finish">Finish time (exclude)</param>
		/// <returns>If <paramref name="start"/> larger than <see cref="Length"/>, return null</returns>
		public List<float> GetMarksBetween(float start, float finish)
		{
			Assert.IsTrue(start < finish);
			int begin = marks.BinarySearch(start);
			while (begin > 0 && marks[begin - 1] == marks[begin]) { begin--; }
			if (begin < 0)
			{
				begin = ~begin;
				if (begin == marks.Count) { return null; }
			}
			int remain = marks.Count - begin;
			for (int count = 0; count < remain; count++)
			{
				if (marks[begin + count] >= finish)
				{
					return marks.GetRange(begin, count);
				}
			}
			return marks.GetRange(begin, remain);
		}

		/// <summary>
		/// Get marks in an update.
		/// </summary>
		/// <param name="current">Current time</param>
		/// <param name="deltaTime">Delta time of the update</param>
		/// <returns></returns>
		public List<float> GetMarks(float current, float deltaTime)
		{
			var result = GetMarksBetween(current - deltaTime, current);
			if (result == null) { return new List<float>(); }
			else { return result; }
		}
	}
}
