namespace CANStudio.BulletStorm.BulletSystem
{
    public interface IBulletAction
    {
        /// <summary>
        ///     Updates all bullets' parameters by this function.
        /// </summary>
        /// <param name="bulletParam">The bullet's parameters to be modified.</param>
        /// <param name="deltaTime">Time since last called.</param>
        /// <returns>False if this action finishes.</returns>
        bool Update(ref BulletParam bulletParam, float deltaTime);
    }
}