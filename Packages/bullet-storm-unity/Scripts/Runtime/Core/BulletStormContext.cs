using System;

namespace CANStudio.BulletStorm.Core
{
    public class BulletStormContext
    {
        internal event Action<float> simulate;

        public VariableTable Variables { get; } = new VariableTable();

        public void Simulate(float deltaTime) => simulate?.Invoke(deltaTime);
    }
}