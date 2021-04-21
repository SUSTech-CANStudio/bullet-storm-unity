namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class Acceleration : IBulletAction
    {
        private readonly float _acceleration;
        private readonly float? _maxSpeed;
        private readonly float? _minSpeed;

        public Acceleration(float acceleration, float? minSpeed, float? maxSpeed)
        {
            _acceleration = acceleration;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
        }

        void IBulletAction.SetContext(BulletStormContext context)
        {
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