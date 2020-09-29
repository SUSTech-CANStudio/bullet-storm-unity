using System;
using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm.Events
{
    [Serializable]
    public class BeginLoop : IStormEvent
    {
        [SerializeField] private LoopType type;
        [SerializeField] private int loopCount;

        #region Compile time
        
        [NonSerialized] private int outScopeIndex;
        
        #endregion
        
        /// <summary>
        /// Create a finite loop.
        /// </summary>
        /// <param name="loopCount">How many times should loop.</param>
        public BeginLoop(int loopCount)
        {
            type = LoopType.Finite;
            this.loopCount = loopCount;
        }

        public void Compile(StormInfo info, Stack<int> scopes, int index)
        {
            scopes.Push(index);
        }

        public IEnumerator Execute(StormExecutor executor, Variable variable)
        {
            switch (type)
            {
                case LoopType.Finite:
                    if (!variable.TryGetValue(out int count)) count = 0;
                    if (count < loopCount)
                    {
                        variable.SetValue(count + 1);
                        executor.Next();
                    }
                    else
                    {
                        variable.Reset();
                        executor.JumpTo(outScopeIndex);
                    }
                    break;
                
                case LoopType.Conditional:
                    // TODO: Condition expression
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield break;
        }

        /// <summary>
        /// Set index for this to jump out of scope.
        /// </summary>
        /// This function should only be called in <see cref="EndLoop"/>.
        /// <param name="index">The next event index of <see cref="EndLoop"/> event.</param>
        internal void SetOutScopeIndex(int index) => outScopeIndex = index;

        [Serializable]
        private enum LoopType
        {
            [Tooltip("Finite loop with a given loop count.")]
            Finite,
            [Tooltip("Loop when condition expresion returns true.")]
            Conditional,
        }
    }
}