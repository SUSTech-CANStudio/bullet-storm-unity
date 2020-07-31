using UnityEngine;

namespace BulletStorm.BulletSystem
{
    public interface ICopiedBulletSystem : IBulletSystem
    {
        void SetParent(Transform parent);
    }
}