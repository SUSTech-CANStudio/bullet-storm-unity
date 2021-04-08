using System.Numerics;

namespace BulletStorm.Core
{
    public struct EmitParams
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 size;
        public Vector4 color;

        public EmitParams(Vector3 position)
        {
            this.position = position;
            velocity = Vector3.Zero;
            size = Vector3.One;
            color = Vector4.Zero;
        }
    }
}