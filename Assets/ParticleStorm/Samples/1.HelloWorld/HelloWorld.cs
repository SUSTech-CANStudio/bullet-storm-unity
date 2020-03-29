using ParticleStorm;
using ParticleStorm.Util;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Particle particle = new Particle();
        particle.Emit(new EmitParams(), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
