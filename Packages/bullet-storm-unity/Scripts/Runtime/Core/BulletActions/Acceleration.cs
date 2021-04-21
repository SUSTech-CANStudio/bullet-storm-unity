namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class Acceleration : IBulletAction
    {
        private readonly float _acceleration;
        private readonly float? _minSpeed;
        private readonly float? _maxSpeed;

        public Acceleration(float acceleration, float? minSpeed, float? maxSpeed)
        {
            _acceleration = acceleration;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
        }

        public void UpdateBullet(ref BulletParams @params, float deltaTime)
        {
            @params.speed += _acceleration * deltaTime;
            if (_minSpeed != null && @params.speed < _minSpeed)
                @params.speed = (float) _minSpeed;
            if (_maxSpeed != null && @params.speed > _maxSpeed)
                @params.speed = (float) _maxSpeed;
        }
    }
}