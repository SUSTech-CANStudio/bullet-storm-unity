using System.Collections;
using BulletStorm.BulletSystem;
using UnityEngine;

namespace BulletStorm.Storm
{
    public interface IStormAction
    {
        IEnumerator Execute(IBulletController controller, Transform emitter);
    }
}