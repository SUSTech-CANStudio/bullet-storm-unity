using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 使用命名空间
using ParticleStorm; // Storm，StormGenerator，和一个空的Particle脚本在这个目录下
using ParticleStorm.Util; // 设置和一些小工具在里面
using ParticleStorm.Factories; // 用于获取Particle和Storm对象

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 新建一种粒子（使用空粒子）
        var particle = ParticleFactory.NewParticle<ExampleParticle>("my particle"); // 新建粒子必须用工厂
        // 新建一个风暴（使用普通Storm）
        var storm = new Storm("my storm"); // 风暴可以从工厂新建，也可以这样直接新建

        // 使用圆锥工具创建一个发射参数列表，圆锥θ值为90°就是一个圆环
        // 创建发射参数的工具都在Filters类里
        var emitParams = Filters.Cone(100, 2, 90, 1);
        Filters.Size(emitParams, 1);

        // 为风暴添加一个行为
        storm.AddBehavior(
            5 /* 这个行为在风暴形成5秒后开始执行 */,
            emitParams /* 之前创建好的发射参数 */,
            particle /* 要发射的粒子，也可以从工厂获取:
            ParticleFactory.instance.GetParticle("my particle") */
        );

        // 发射！（需要ParticleStorm目录下的StormGenerator组件）
        GetComponent<StormGenerator>().Generate(storm);
    }
}
