using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;

public class MyParticle : APParticle
{
    public override void ParticleStart()
    {
        // 对整类粒子进行设定
        // 材质、触发类型、初始状态等
        // 详细可设置项参见ExampleParticle注释内容
        var main = this.main;
        main.startSize = 0.2f;
        main.startLifetime = 100;
    }

    public override void ParticleUpdate(ref ParticleSystem.Particle self)
    {
        // 让速度随着时间不断减小
        self.velocity = self.velocity.normalized * self.remainingLifetime / self.startLifetime;
    }
}
