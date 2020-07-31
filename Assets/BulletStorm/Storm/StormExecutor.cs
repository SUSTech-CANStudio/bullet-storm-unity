using System.Collections.Generic;
using BulletStorm.BulletSystem;

namespace BulletStorm.Storm
{
    public class StormExecutor
    {
        private StormInfo info;
        private List<ICopiedBulletSystem> referencedBulletSystems;
        private int nextEvent;

        public void JumpTo(int stormEventSeq) => nextEvent = stormEventSeq;
    }
}