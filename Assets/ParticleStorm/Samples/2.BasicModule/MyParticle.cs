using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;
using ParticleStorm.Util;

public class MyParticle : MonoBehaviour
{
    public ParticlePrefeb prefeb;
    // Start is called before the first frame update
    void Start()
    {
        Particle particle = new Particle(prefeb);
        particle.Emit(new EmitParams(), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
