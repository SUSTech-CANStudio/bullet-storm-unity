using UnityEngine;
using ParticleStorm;

public class MyStorm : MonoBehaviour
{
    public ParticlePrefeb particlePrefeb;
    public float gap;

    void Start()
    {
        // Get generator component
        var generator = GetComponent<StormGenerator>();
        // Create an empty storm
        var storm = new Storm();
        // Create a particle from prefeb
        var particle = new Particle(particlePrefeb);
        // Create an emit list.
        var emitList = EmitList.Cone(100, 2, 90, 1);
        // Add an emission behavior to storm.
        storm.AddBehavior(0, emitList, particle, gap);
        // Generate storm.
        generator.Generate(storm);
    }
}
