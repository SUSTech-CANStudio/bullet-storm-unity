using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace CANStudio.BulletStorm.Util
{
    public class TimeSequence
    {
        private readonly List<float> marks;

        /// <summary>
        ///     Create a time sequence by marks.
        /// </summary>
        /// <param name="marks">Marks on the time sequence.</param>
        public TimeSequence([NotNull] List<float> marks)
        {
            this.marks = marks;
            this.marks.Sort();
            if (marks.Count > 0)
                Length = marks[marks.Count - 1];
            else
                Length = 0;
        }

        /// <summary>
        ///     Total time of this sequence.
        /// </summary>
        public float Length { get; }

        /// <summary>
        ///     Mark count in this sequence.
        /// </summary>
        public int Count => marks.Count;

        /// <summary>
        ///     Create a time sequence with equal time interval between every two marks.
        /// </summary>
        /// <param name="markCount">Number of marks</param>
        /// <param name="space">Time between two marks</param>
        /// <returns></returns>
        public static TimeSequence EqualLength(int markCount, float space)
        {
            var result = new List<float>();
            for (var i = 0; i < markCount; i++) result.Add(i * space);
            return new TimeSequence(result);
        }

        /// <summary>
        ///     Get marks until current time.
        /// </summary>
        /// <param name="startIndex">Start index of marks.</param>
        /// <param name="currentTime">Current time of the sequence.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public List<float> GetMarks(int startIndex, float currentTime)
        {
            Assert.IsTrue(startIndex >= 0 && startIndex < Count);
            var count = 0;
            for (var i = startIndex; i < Count; i++)
            {
                if (marks[i] > currentTime) break;
                count++;
            }

            return marks.GetRange(startIndex, count);
        }
    }
}