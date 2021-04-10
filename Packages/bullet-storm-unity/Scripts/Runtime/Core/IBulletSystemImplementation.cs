using System.Collections.Generic;

namespace CANStudio.BulletStorm.Core
{
    /// <summary>
    ///     Implementation of a bullet system, should be implemented by particle system or other methods
    ///     in game to render bullets.
    /// </summary>
    public interface IBulletSystemImplementation
    {
        int BulletCount { get; }

        /// <summary>
        ///     Returns reference of a bullet.
        ///     Implementation of this function should be thread-safe.
        /// </summary>
        /// <param name="index">0 to (BulletCount - 1)</param>
        /// <returns>Value can be changed by caller.</returns>
        ref BulletParams Bullet(int index);
        
        /// <summary>
        ///     Emit with parameters in world space.
        /// </summary>
        /// <param name="emitParams"></param>
        void Emit(EmitParams emitParams);
        
        void Emit(IEnumerable<EmitParams> emitParams);
        
        /// <summary>
        ///     This is called when the instance no longer needed.
        /// </summary>
        void Abandon();
    }
}