using System.Collections;
using System.Collections.Generic;
using BulletStorm.Util;

namespace BulletStorm.Storm.Events
{
    public class Action : IStormEvent
    {
        private readonly string bullet;
        private readonly IStormAction action;

        private int bulletIndex;
        
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