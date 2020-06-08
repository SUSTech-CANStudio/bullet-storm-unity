using ParticleStorm.ParticleNS;
using UnityEngine;

public class MyParticle : MonoBehaviour
{
	public ParticlePrefeb prefeb;

	private void Start()
	{
		Particle particle = new Particle(prefeb);
		particle.Origin.Emit(new EmitParams());
	}
}
