using System;
using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm.Events
{
    /// <summary>
    ///     Wait before next event.
    /// </summary>
    [Serializable]
    public class Wait : IStormEvent
    {
        [SerializeField] private float time;

        /// <summary>
        ///     Creates a wait event.
        /// </summary>
        /// <param name="time">Time in seconds.</param>
        public Wait(float time)
        {
            this.time = time;
        }

        public void Compile(StormInfo info, Stack<int> scopes, int index)
        {
        }

        public IEnumerator Execute(StormExecutor executor, Variable variable)
        {
            yield return new WaitForSeconds(time);
        }
    }
}