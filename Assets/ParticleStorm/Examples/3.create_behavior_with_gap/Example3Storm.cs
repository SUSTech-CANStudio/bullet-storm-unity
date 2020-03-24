using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;
using ParticleStorm.Factories;
using ParticleStorm.Util;

[RequireComponent(typeof(StormGenerator))]
public class Example3Storm : MonoBehaviour
{
    private Storm storm;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        storm = new Storm("gap storm");
        var p = ParticleFactory.NewParticle(particle) as MyParticle;
        // 这个行为设置了0.5秒的间隙，每次发射将会间隔0.5秒
        storm.AddBehavior(0, Filters.Cone(100, 0, 30, 1), p, 0.2f);
        Invoke("Begin", 1);
    }

    void Begin()
    {
        GetComponent<StormGenerator>().Generate(storm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
