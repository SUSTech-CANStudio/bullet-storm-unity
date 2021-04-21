using System;
using System.Numerics;

namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class AroundAxis : IBulletAction
    {
        private readonly Vector3 _axis;
        private readonly float _anglePerSecond;

        public AroundAxis(Vector3 axis, float anglePerSecond)
        {
            if (axis == Vector3.Zero) throw new ArgumentException("Axis can't be zero", nameof(axis));
            _axis = axis;
            _anglePerSecond = anglePerSecond;
        }

        public void UpdateBullet(ref BulletParams @params, float deltaTime) => @params.rotation =
            Quaternion.CreateFromAxisAngle(_axis, _anglePerSecond * deltaTime) * @params.rotation;
    }
}