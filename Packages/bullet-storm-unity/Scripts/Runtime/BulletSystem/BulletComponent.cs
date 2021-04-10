using CANStudio.BulletStorm.Core;
using CANStudio.BulletStorm.Util;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    /// <summary>
    ///     This component manages the game object's position, rotation and lifetime.
    ///     Once this component attached, modifying transform will be useless.
    /// </summary>
    [DisallowMultipleComponent]
    public class BulletComponent : MonoBehaviour
    {
        internal BulletParams bulletParams;

        public Vector3 Position
        {
            get => bulletParams.position.ToUnity();
            set => bulletParams.position = value.ToSystem();
        }

        public Quaternion Rotation
        {
            get => bulletParams.rotation.ToUnity();
            set => bulletParams.rotation = value.ToSystem();
        }

        public float Speed
        {
            get => bulletParams.speed;
            set => bulletParams.speed = value;
        }

        /// <summary>
        ///     How long has this bullet existed.
        /// </summary>
        public float Lifetime => bulletParams.lifetime;
        
        private float _startLifetime;

        internal static BulletComponent Create(GameObject prototype, EmitParams emitParams, float startLifetime)
        {
            var go = Instantiate(prototype, null);
            go.hideFlags = HideFlags.HideAndDontSave;
            if (!go.TryGetComponent(out BulletComponent self)) self = go.AddComponent<BulletComponent>();
            self.bulletParams = new BulletParams(emitParams.position,
                Quaternion.LookRotation(emitParams.velocity.ToUnity()).ToSystem(),
                emitParams.velocity.Length(),
                0);
            self._startLifetime = startLifetime;
            self.GetComponent<Renderer>().material.color = new Color(emitParams.color.X, emitParams.color.Y,
                emitParams.color.Z, emitParams.color.W);
            return self;
        }

        /// <summary>
        ///     Set transform of game object with <see cref="Position"/> and <see cref="Rotation"/>.
        ///     This will be invoked automatically in every <see cref="Update"/>, you can also manually call
        ///     it to sync transform if need.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public void SyncTransform()
        {
            var t = transform;
            t.position = Position;
            t.rotation = Rotation;
        }
        
        private void Update()
        {
            bulletParams = new BulletParams(bulletParams.position, bulletParams.rotation, bulletParams.speed,
                bulletParams.lifetime + Time.deltaTime);
            if (Lifetime >= _startLifetime) Destroy(gameObject);
            Position += Rotation * Vector3.forward * (Speed * Time.deltaTime);
            SyncTransform();
        }
    }
}