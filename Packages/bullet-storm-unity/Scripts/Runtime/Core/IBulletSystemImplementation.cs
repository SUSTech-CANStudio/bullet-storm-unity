using System.Collections.Generic;

namespace BulletStorm.Core
{
    /// <summary>
    ///     Implementation of a bullet system, should be implemented by particle system or other methods
    ///     in game to render bullets.
    /// </summary>
    public interface IBulletSystemImplementation
    {
        int BulletCount { get; }

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