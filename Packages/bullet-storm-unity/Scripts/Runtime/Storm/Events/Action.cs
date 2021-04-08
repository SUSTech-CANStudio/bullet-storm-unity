using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Util;

namespace CANStudio.BulletStorm.Storm.Events
{
    /// <summary>
    ///     Event contains an action.
    /// </summary>
    /// <seealso cref="BulletStorm.Storm.Actions" />
    public class Action : IStormEvent
    {
        private readonly IStormAction action;
        private readonly string bullet;

        private int bulletIndex;

        /// <summary>
        ///     Creates an action event.
        /// </summary>
        /// <param name="action">The action inside.</param>
        /// <param name="bullet">Bullet name the action ues.</param>
        public Action(IStormAction action, string bullet)
        {
            this.bullet = bullet;
            this.action = action;
        }

        public void Compile(StormInfo info, Stack<int> scopes, int index)
        {
            bulletIndex = info.GetBulletIndex(bullet);
        }

        public IEnumerator Execute(StormExecutor executor, Variable variable)
        {
            yield return action.Execute(executor.GetController(bulletIndex), executor.GetEmitter());
            executor.Next();
        }
    }
}