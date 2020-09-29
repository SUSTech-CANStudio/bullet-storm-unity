using System;
using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Util;

namespace CANStudio.BulletStorm.Storm.Events
{
    [Serializable]
    public class EndLoop : IStormEvent
    {
        #region Compile time

        [NonSerialized] private int beginLoopIndex;

        #endregion
        
        /// <summary>
        /// Creates an end loop event.
        /// </summary>
        public EndLoop() {}
        
        public void Compile(StormInfo info, Stack<int> scopes, int index)
        {
            try
            {
                var lastScopeBegin = scopes.Pop();
                if (info.Events[lastScopeBegin] is BeginLoop beginLoopEvent)
                {
                    beginLoopEvent.SetOutScopeIndex(index + 1);
                    beginLoopIndex = lastScopeBegin;
                }
                else throw new StormCompileException(index, GetType(),
                    "Event at index " + lastScopeBegin + " is not a begin loop event.");
            }
            catch (InvalidOperationException)
            {
                throw new StormCompileException(index, GetType(), "Can't find begin loop event.");
            }
        }

        public IEnumerator Execute(StormExecutor executor, Variable variable)
        {
            executor.JumpTo(beginLoopIndex);
            yield break;
        }
    }
}