namespace BulletStorm.Core
{
    /// <summary>
    ///     Implementation of a bullet system, should be implemented by particle system or other methods
    ///     in game to render bullets.
    /// </summary>
    public interface IBulletSystemImplementation
    {
        BulletParams[] Bullets { get; set; }

        /// <summary>
        ///     This is called when the instance no longer needed.
        /// </summary>
        void Abandon();
    }
}