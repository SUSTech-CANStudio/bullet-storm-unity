using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Util;
using JetBrains.Annotations;

namespace CANStudio.BulletStorm.Storm
{
    public interface IStormEvent
    {
        /// <summary>
        ///     Compiles the event in a <see cref="StormInfo" />.
        /// </summary>
        /// <param name="info">The storm this event belongs to.</param>
        /// <param name="scopes">A stack to read and write scopes.</param>
        /// <param name="index">Index of this event.</param>
        /// <exception cref="StormCompileException"></exception>
        void Compile([NotNull] StormInfo info, [NotNull] Stack<int> scopes, int index);

        /// <summary>
        ///     Executes the event in a <see cref="StormExecutor" />.
        /// </summary>
        /// <param name="executor">The storm executor to execute this event.</param>
        /// <param name="variable">This event's own variable.</param>
        /// <returns></returns>
        IEnumerator Execute([NotNull] StormExecutor executor, [NotNull] Variable variable);
    }
}