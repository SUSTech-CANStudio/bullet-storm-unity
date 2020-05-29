using ParticleStorm;
using ParticleStorm.Util;
using UnityEngine;

public class MyParticle : MonoBehaviour
{
    public ParticlePrefeb prefeb;

    void Start()
    {
        Particle particle = new Particle(prefeb);
        particle.Emit(new EmitParams(), 1);
    }
}
