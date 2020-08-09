namespace BulletStorm.BulletSystem
{
    /// <summary>
    /// Classes implement this interface are bullet systems.
    /// </summary>
    public interface IBulletSystem
    {
        /// <summary>
        /// Name of the bullet system.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get a controller of this bullet system.
        /// </summary>
        /// <returns></returns>
        IBulletController GetController();
    }
}