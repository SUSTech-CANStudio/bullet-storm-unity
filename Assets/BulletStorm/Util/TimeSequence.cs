using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BulletStorm.Util
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
		/// Get marks until current time.
		/// </summary>
		/// <param name="startIndex">Start index of marks.</param>
		/// <param name="currentTime">Current time of the sequence.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public List<float> GetMarks(int startIndex, float currentTime)
		{
			if (startIndex < 0 || startIndex >= marks.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}
			int count = 0;
			for (int i = startIndex; i < marks.Count; i++)
			{
				if (marks[i] > currentTime) { break; }
				else { count++; }
			}
			return marks.GetRange(startIndex, count);
		}
	}
}
