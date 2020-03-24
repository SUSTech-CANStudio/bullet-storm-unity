using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;
using ParticleStorm.Factories;
using ParticleStorm.Util;

[RequireComponent(typeof(StormGenerator))]
public class MultiBehaviorStorm : MonoBehaviour
{
    private Storm storm;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        storm = new Storm("gap storm");
        var p = ParticleFactory.NewParticle(particle) as MyParticle;
        storm.AddBehavior(0, Filters.Cone(100, 2, 90, 0.5f), p); 
        storm.AddBehavior(0, Filters.Cone(100, 0, 30, 1), p, 0.2f);
        storm.AddBehavior(8, Filters.Cone(100, 2, 90, 0.5f), p);
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
