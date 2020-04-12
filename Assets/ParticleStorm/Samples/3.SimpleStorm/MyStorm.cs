using UnityEngine;
using ParticleStorm;

public class MyStorm : MonoBehaviour
{
    public ParticlePrefeb particle;
    public float gap;
    // Start is called before the first frame update
    void Start()
    {
        var generator = GetComponent<StormGenerator>();
        var storm = new Storm();
        var emitList = EmitList.Cone(100, 2, 90, 1);
        storm.AddBehavior(0, emitList, new Particle(particle), gap);
        generator.Generate(storm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
