using System.Collections;
using CANStudio.BulletStorm.BulletSystem;
using UnityEngine;

namespace CANStudio.BulletStorm.Storm
{
    public interface IStormAction
    {
        IEnumerator Execute(IBulletController controller, Transform emitter);
    }
}