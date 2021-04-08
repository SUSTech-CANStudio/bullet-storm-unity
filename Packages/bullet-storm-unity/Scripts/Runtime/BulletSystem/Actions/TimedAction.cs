namespace CANStudio.BulletStorm.BulletSystem.Actions
{
    /// <summary>
    ///     Bullet action with time limitation.
    /// </summary>
    public class TimedAction : IBulletAction
    {
        private readonly float? totalTime;
        private float currentTime;

        public TimedAction()
        {
            totalTime = null;
            currentTime = 0;
        }

        public TimedAction(float totalTime)
        {
            this.totalTime = totalTime;
            currentTime = 0;
        }

        public virtual bool Update(ref BulletParam bulletParam, float deltaTime)
        {
            if (totalTime is null)
                return true;
            currentTime += deltaTime;
            return currentTime < totalTime;
        }
    }
}