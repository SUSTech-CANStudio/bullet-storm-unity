using System.Collections.Generic;
using BulletStorm.BulletSystem;
using UnityEngine;

namespace BulletStorm.Storm
{
    public class StormInfo
    {
        private readonly List<IStormEvent> stormEvents;
        private readonly List<Bullet> referencedBullets;
        private readonly Stack<int> scopeStack;

        public StormInfo()
        {
            stormEvents = new List<IStormEvent>();
            referencedBullets = new List<Bullet>();
            scopeStack = new Stack<int>();
        }
        
        public void StackPush(int eventSeq) => scopeStack.Push(eventSeq);

        public int StackPop() => scopeStack.Pop();

        public int StackTop() => scopeStack.Peek();
    }
}