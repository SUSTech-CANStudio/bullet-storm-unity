using ParticleStorm.ParticleNS;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
	// Start is called before the first frame update
	private void Start()
	{
		Particle particle = new Particle();
		particle.Origin.Emit(new EmitParams());
	}
}
