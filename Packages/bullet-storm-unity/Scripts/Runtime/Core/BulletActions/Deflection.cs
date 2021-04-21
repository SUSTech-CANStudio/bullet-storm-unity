using System.Numerics;

namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class Deflection : IBulletAction
    {
        private readonly float _pitch;
        private readonly float _roll;
        private readonly float _yaw;

        public Deflection(float yaw, float pitch, float roll)
        {
            _yaw = yaw;
            _pitch = pitch;
            _roll = roll;
        }

        void IBulletAction.SetContext(BulletStormContext context)
        {
        }

        public void UpdateBullet(ref BulletParams @params, float deltaTime)
        {
            var rotation = Quaternion.CreateFromYawPitchRoll(_yaw * deltaTime, _pitch * deltaTime, _roll * deltaTime);
            @params.rotation = rotation * @params.rotation;
        }
    }
}