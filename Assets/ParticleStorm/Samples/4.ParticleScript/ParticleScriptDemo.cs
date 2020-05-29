using ParticleStorm;
using ParticleStorm.Script;
using UnityEngine;

public class ParticleScriptDemo : MonoBehaviour
{
    public ParticlePrefeb particlePrefeb;
    public float gap;
    public float wait;
    public float speed;

    void MyScript(ParticleStatus particle)
    {
        Debug.Log(particle.Position);
        if (particle.StartLifetime - particle.RemainingLifetime < wait)
            particle.Velocity = particle.Velocity.normalized * 0.01f;
        else
            particle.Velocity = particle.Velocity.normalized * speed;
    }

    void Start()
    {
        // Regester function 'MyScript' as a script
        // To use the script, fill the function name in particle prefeb script module
        UpdateEvent myScript = new UpdateEvent("MyScript", MyScript);
        ParticleScript.AddUpdateScript(myScript);
        // Create particle and generate storm
        var generator = GetComponent<StormGenerator>();
        var storm = new Storm();
        var particle = new Particle(particlePrefeb);
        storm.AddBehavior(0.5f, EmitList.Sphere(100, 3, speed), particle, gap);
        generator.Generate(storm);
    }
}
