using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    [DisallowMultipleComponent]
    public class GameObjectBullet : MonoBehaviour
    {
        public float speed;
        public float lifetime;
        private bool enableLifetime;
        public float StartTime { get; private set; }

        public void EnableLifeTime(float time)
        {
            enableLifetime = true;
            lifetime = time;
        }

        internal void Init(Vector3 position, Vector3 velocity) => Init(position, velocity, Color.clear, Vector3.zero);

        internal void Init(Vector3 position, Vector3 velocity, Color color, Vector3 size)
        {
            var t = transform;
            t.position = position;
            speed = velocity.magnitude;
            t.forward = velocity;
            enableLifetime = false;
            if (color != Color.clear) GetComponent<Renderer>().material.color = color;
            if (size != Vector3.zero) transform.localScale = size;
        }

        private void Start()
        {
            StartTime = Time.time;
        }

        private void LateUpdate()
        {
            var t = transform;
            t.position += speed * Time.deltaTime * t.forward;
            if (!enableLifetime) return;
            if (lifetime <= Time.time - StartTime) Destroy(gameObject);
        }
    }
}