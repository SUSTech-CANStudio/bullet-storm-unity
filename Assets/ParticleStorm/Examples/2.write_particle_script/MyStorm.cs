using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;
using ParticleStorm.Util;
using ParticleStorm.Factories;

[RequireComponent(typeof(StormGenerator))]
public class MyStorm : MonoBehaviour
{
    // 一个带有粒子脚本组件的prefeb
    public GameObject particlePrefeb;

    public string stormName;

    // Start is called before the first frame update
    void Start()
    {
        // 从prefeb新建粒子（需要类型转换）
        MyParticle particle = ParticleFactory.NewParticle(particlePrefeb) as MyParticle;
        // 新建一个风暴
        var storm = new Storm(stormName);
        // 添加风暴行为（发射30°圆锥弹幕）
        storm.AddBehavior(0, Filters.Cone(100, 0, 30, 1), particle);
        // 开始生成
        Invoke("Begin", 1);
    }

    void Begin()
    {
        GetComponent<StormGenerator>().Generate(Storm.Find(stormName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
