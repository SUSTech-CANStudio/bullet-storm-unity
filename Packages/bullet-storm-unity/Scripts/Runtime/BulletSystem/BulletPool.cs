using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     A useful tool to manage bullets.
    /// </summary>
    /// Put this
    /// <see cref="ScriptableObject" />
    /// into the folder you store your bullet system prefabs,
    /// and it can auto register all bullet systems in the folder and sub folders.
    /// Then you can use
    /// <see cref="Find" />
    /// to get a bullet.
    [CreateAssetMenu(menuName = "BulletStorm/BulletPool")]
    public sealed class BulletPool : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Bullet pool can inherit bullets from other pool.")]
        public BulletPool parentPool;

        [SerializeField] [HideInInspector] private List<string> keys;
        [SerializeField] [HideInInspector] private List<Object> values;

        [NonSerialized] public readonly Dictionary<string, IBullet> bullets = new Dictionary<string, IBullet>();

        public void OnBeforeSerialize()
        {
            if (keys is null) keys = new List<string>();
            else keys.Clear();
            if (values is null) values = new List<Object>();
            else values.Clear();
            foreach (var bullet in bullets)
            {
                keys.Add(bullet.Key);
                values.Add(bullet.Value as Object);
            }
        }

        public void OnAfterDeserialize()
        {
            bullets.Clear();
            for (var i = 0; i < keys.Count && i < values.Count; i++) bullets.Add(keys[i], values[i] as IBullet);
        }

        /// <summary>
        ///     Finds a bullet system in this pool, will also find in parent pools.
        /// </summary>
        /// <param name="bulletName">Name of the bullet (bullet system prefab name)</param>
        /// <returns>Null if not found</returns>
        public IBullet Find(string bulletName)
        {
            if (bullets.TryGetValue(bulletName, out var bullet)) return bullet;
            if (parentPool && parentPool != this) return parentPool.Find(bulletName);
            return null;
        }
    }
}