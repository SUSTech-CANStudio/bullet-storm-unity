using System;

namespace BulletStorm.Core
{
    public class BulletStormContext
    {
        internal event Action<float> simulate;

        public void Simulate(float deltaTime)
        {
            simulate?.Invoke(deltaTime);
        }
    }
}