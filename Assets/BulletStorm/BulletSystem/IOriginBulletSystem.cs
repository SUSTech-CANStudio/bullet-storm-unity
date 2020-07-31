namespace BulletStorm.BulletSystem
{
    public interface IOriginBulletSystem : IBulletSystem
    {
        /// <summary>
        /// Name of the bullet system.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get a copy of this bullet system.
        /// </summary>
        /// <returns></returns>
        ICopiedBulletSystem Copy();
    }
}