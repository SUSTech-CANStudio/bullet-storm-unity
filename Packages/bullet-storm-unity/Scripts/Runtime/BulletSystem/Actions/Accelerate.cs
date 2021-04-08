namespace CANStudio.BulletStorm.BulletSystem.Actions
{
    public class Accelerate : TimedAction
    {
        private readonly float acceleration;
        private readonly float maxSpeed;
        private readonly float minSpeed;

        public Accelerate(float acceleration, float minSpeed, float maxSpeed)
        {
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;
            this.minSpeed = minSpeed;
        }

        /// <summary>
        ///     Accelerate with constant acceleration
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="minSpeed"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="time"></param>
        public Accelerate(float acceleration, float minSpeed, float maxSpeed, float time) : base(time)
        {
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;
            this.minSpeed = minSpeed;
        }

        public override bool Update(ref BulletParam bulletParam, float deltaTime)
        {
            return base.Update(ref bulletParam, deltaTime);
        }
    }
}