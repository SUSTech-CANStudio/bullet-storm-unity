using System;
using System.Collections;
using System.Collections.Generic;
using BulletStorm.BulletSystem;
using BulletStorm.Util;
using UnityEngine;

namespace BulletStorm.Storm
{
    /// <summary>
    /// Executes a storm once.
    /// </summary>
    public class StormExecutor
    {
        private readonly StormInfo info;
        private readonly Transform emitter;
        private readonly IReadOnlyList<IBulletController> controllers;
        private readonly IReadOnlyList<IBulletController> defaultControllers;
        private readonly List<Variable> variables;
        private int nextEvent;

        internal StormExecutor(StormInfo info, Transform emitter, IReadOnlyList<IBulletController> controllers,
            IReadOnlyList<IBulletController> defaultControllers)
        {
            this.info = info;
            this.controllers = controllers;
            this.defaultControllers = defaultControllers;
            variables = new List<Variable>();
            for (var i = 0; i < info.Events.Count; i++) variables.Add(new Variable());
            nextEvent = 0;
        }

        ~StormExecutor()
        {
            foreach (var controller in controllers)
            {
                controller.Destroy();
            }

            foreach (var defaultController in defaultControllers)
            {
                defaultController.Destroy();
            }
        }

        internal IEnumerator Execute()
        {
            var stormEvents = info.Events;
            while (nextEvent < stormEvents.Count)
            {
                yield return stormEvents[nextEvent].Execute(this, variables[nextEvent]);
            }
        }

        /// <summary>
        /// Get a bullet controller.
        /// </summary>
        /// <param name="index">Index of bullet in the storm.</param>
        /// <returns></returns>
        internal IBulletController GetController(int index) => index >= 0 ? controllers[index] : defaultControllers[~index];

        /// <summary>
        /// Emitter of this executor.
        /// </summary>
        /// <returns></returns>
        internal Transform GetEmitter() => emitter;
        
        /// <summary>
        /// To next event.
        /// </summary>
        internal void Next() => nextEvent++;
        
        /// <summary>
        /// Jump to a event.
        /// </summary>
        /// <param name="stormEventIndex">Index of target event</param>
        internal void JumpTo(int stormEventIndex) => nextEvent = stormEventIndex;
    }
}