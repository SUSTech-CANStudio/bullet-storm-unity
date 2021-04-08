using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    [DisallowMultipleComponent]
    public class BulletComponent : MonoBehaviour
    {
        public float speed;
        public float lifetime;
        private bool _enableLifetime;
        public float StartTime { get; private set; }

        private void Start()
        {
            StartTime = Time.time;
        }

        private void LateUpdate()
        {
            var t = transform;
            t.position += speed * Time.deltaTime * t.forward;
            if (!_enableLifetime) return;
            if (lifetime <= Time.time - StartTime) Destroy(gameObject);
        }

        public void EnableLifeTime(float time)
        {
            _enableLifetime = true;
            lifetime = time;
        }

        internal void Init(Vector3 position, Vector3 velocity)
        {
            Init(position, velocity, Color.clear, Vector3.zero);
        }

        internal void Init(Vector3 position, Vector3 velocity, Color color, Vector3 size)
        {
            var t = transform;
            t.position = position;
            speed = velocity.magnitude;
            t.forward = velocity;
            _enableLifetime = false;
            if (color != Color.clear) GetComponent<Renderer>().material.color = color;
            if (size != Vector3.zero) transform.localScale = size;
        }
    }
}