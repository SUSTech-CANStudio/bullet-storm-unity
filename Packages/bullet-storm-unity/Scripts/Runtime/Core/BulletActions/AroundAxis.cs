using System;
using System.Numerics;

namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class AroundAxis : IBulletAction
    {
        private readonly float _anglePerSecond;
        private readonly Vector3 _axis;

        public AroundAxis(Vector3 axis, float anglePerSecond)
        {
            if (axis == Vector3.Zero) throw new ArgumentException("Axis can't be zero", nameof(axis));
            _axis = axis;
            _anglePerSecond = anglePerSecond;
        }

        void IBulletAction.SetContext(BulletStormContext context)
        {
        }

        public void UpdateBullet(ref BulletParams @params, float deltaTime) => @params.rotation =
            Quaternion.CreateFromAxisAngle(_axis, _anglePerSecond * deltaTime) * @params.rotation;
    }
}