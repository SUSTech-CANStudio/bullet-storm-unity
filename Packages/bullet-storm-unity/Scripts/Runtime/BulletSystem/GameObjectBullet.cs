using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CANStudio.BulletStorm.BulletSystem
{
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    [DisallowMultipleComponent]
    public class GameObjectBullet : MonoBehaviour
    {
        public Vector3 velocity;
        public float lifetime;
        private bool enableLifetime;

        public void EnableLifeTime(float time)
        {
            enableLifetime = true;
            lifetime = time;
        }

        internal void Init(Vector3 position, Vector3 velocity)
        {
            Init(position, velocity, Color.clear, Vector3.zero);
        }

        internal void Init(Vector3 position, Vector3 velocity, Color color, Vector3 size)
        {
            transform.position = position;
            this.velocity = velocity;
            enableLifetime = false;
            if (color != Color.clear) GetComponent<Renderer>().material.color = color;
            if (size != Vector3.zero) transform.localScale = size;
        }
        
        private void LateUpdate()
        {
            transform.position += velocity * Time.deltaTime;
            if (!enableLifetime) return;
            lifetime -= Time.deltaTime;
            if (lifetime <= 0) Destroy(this);
        }
    }
}