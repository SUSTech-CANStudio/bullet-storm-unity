using ParticleStorm;
using ParticleStorm.Util;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Particle particle = new Particle();
        particle.Origin.Emit(new EmitParams());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
