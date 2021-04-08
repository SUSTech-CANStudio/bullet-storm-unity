using System;
using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm.Actions
{
    /// <summary>
    ///     Emits bullets with given parameters.
    /// </summary>
    [Serializable]
    public class Emit : IStormAction
    {
        private IEnumerable<BulletEmitParam> emitParams;
        private TimeSequence timeSequence;

        /// <summary>
        ///     Emit a bullet.
        /// </summary>
        /// <param name="emitParam">Parameter of the bullet.</param>
        public Emit(BulletEmitParam emitParam)
        {
            emitParams = new[] {emitParam};
        }

        /// <summary>
        ///     Emit bullets.
        /// </summary>
        /// <param name="shape">
        ///     Bullet parameters, you can get them from <see cref="Shape" />.
        /// </param>
        public Emit(IEnumerable<BulletEmitParam> shape)
        {
            emitParams = shape;
        }

        /// <summary>
        ///     Emit bullets with a const time interval.
        /// </summary>
        /// <param name="shape">Bullet parameters, you can get them from <see cref="Shape" />.</param>
        /// <param name="interval">Time interval between two bullets.</param>
        public Emit(IReadOnlyCollection<BulletEmitParam> shape, float interval) : this(shape)
        {
            timeSequence = TimeSequence.EqualLength(shape.Count, interval);
        }

        public IEnumerator Execute(IBulletController controller, Transform emitter)
        {
            if (!(timeSequence is null))
            {
                var startTime = Time.time;
                var it = emitParams.GetEnumerator();
                for (var i = 0; i < timeSequence.Count;)
                {
                    var list = timeSequence.GetMarks(i, Time.time - startTime);
                    foreach (var _ in list)
                    {
                        controller.Emit(it.Current, emitter);
                        it.MoveNext();
                    }

                    i += list.Count;
                    yield return null;
                }

                it.Dispose();
            }
            else
            {
                foreach (var emitParam in emitParams)
                    controller.Emit(emitParam, emitter);
            }
        }
    }
}