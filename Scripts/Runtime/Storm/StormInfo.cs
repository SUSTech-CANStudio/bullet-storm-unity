using System.Collections;
using System.Collections.Generic;
using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm
{
    public class StormInfo
    {
        public bool Compiled { get; private set; }

        internal IReadOnlyList<IStormEvent> Events => events;

        private readonly IReadOnlyDictionary<string, IBulletSystem> bulletSystemDictionary;
        private readonly IBulletSystem defaultBulletSystem;
        private readonly List<IStormEvent> events = new List<IStormEvent>();
        private readonly List<IBulletSystem> bulletSystemList = new List<IBulletSystem>();
        private readonly List<string> defaultBulletSystemList = new List<string>();

        /// <summary>
        /// Creates a storm info.
        /// </summary>
        /// <param name="bulletSystems">This tells the storm how to find bullets by name.</param>
        /// <param name="defaultBulletSystem">Use default bullet system if bullet name not found.</param>
        public StormInfo([NotNull] IReadOnlyDictionary<string, IBulletSystem> bulletSystems,
            [NotNull] IBulletSystem defaultBulletSystem)
        {
            bulletSystemDictionary = bulletSystems;
            this.defaultBulletSystem = defaultBulletSystem;
            Compiled = false;
        }

        /// <summary>
        /// Add a event to the storm.
        /// </summary>
        /// <param name="stormEvent"></param>
        public void AddEvent(IStormEvent stormEvent)
        {
            events.Add(stormEvent);
            Compiled = false;
        }

        /// <summary>
        /// Set events in the storm.
        /// </summary>
        /// <param name="stormEvents"></param>
        public void SetEvents(IEnumerable<IStormEvent> stormEvents)
        {
            events.Clear();
            events.AddRange(stormEvents);
            Compiled = false;
        }

        /// <summary>
        /// Compile the storm.
        /// </summary>
        /// <returns>True if successes.</returns>
        public bool Compile()
        {
            var scopes = new Stack<int>();
            try
            {
                var index = 0;
                foreach (var stormEvent in events) stormEvent.Compile(this, scopes, index++);

                if (scopes.Count == 0) Compiled = true;
                else
                {
                    BulletStormLogger.LogError(""); // TODO: Write error info.
                    Compiled = false;
                }
            }
            catch (StormCompileException e)
            {
                BulletStormLogger.LogException(e);
                Compiled = false;
            }

            return Compiled;
        }

        /// <summary>
        /// Executes the storm.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Execute(Transform emitter)
        {
            if (!Compiled && !Compile()) return null;
            
            var defaultControllers = new List<IBulletController>(defaultBulletSystemList.Count);
            for (var i = 0; i < defaultBulletSystemList.Count; i++)
            {
                defaultControllers.Add(defaultBulletSystem.GetController());
            }

            var executor = new StormExecutor(
                this, emitter,
                bulletSystemList.ConvertAll(bulletSystem => bulletSystem.GetController()),
                defaultControllers);

            return executor.Execute();
        }

        /// <summary>
        /// Get index of a bullet in this storm.
        /// </summary>
        /// <param name="bulletName">Name of the bullet.</param>
        /// <returns>Index if bullet exists, 0's complement of a unique number if doesn't exists.</returns>
        internal int GetBulletIndex(string bulletName)
        {
            if (bulletSystemDictionary.TryGetValue(bulletName, out var bulletSystem))
            {
                var index = bulletSystemList.IndexOf(bulletSystem);
                if (index > 0) return index;
                bulletSystemList.Add(bulletSystem);
                return bulletSystemList.Count - 1;
            }
            else
            {
                var index = defaultBulletSystemList.IndexOf(bulletName);
                if (index > 0) return ~index;
                defaultBulletSystemList.Add(bulletName);
                return ~(defaultBulletSystemList.Count - 1);
            }
        }
    }
}