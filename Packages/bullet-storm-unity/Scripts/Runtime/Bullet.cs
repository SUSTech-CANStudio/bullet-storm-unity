using CANStudio.BulletStorm.BulletSystem;
using CANStudio.BulletStorm.Core;
using UnityEngine;

namespace CANStudio.BulletStorm
{
    [CreateAssetMenu(fileName = "BulletStorm/Bullet")]
    public class Bullet : ScriptableObject
    {
        public BulletSystemType implementation;
        public IBulletAction[] persistenceActions;
    }
}