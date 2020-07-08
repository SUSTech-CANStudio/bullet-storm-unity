using BulletStorm.BulletNS;
using BulletStorm.BulletNS.Script;
using BulletStorm.StormNS;
using BulletStorm.StormNS.Behavior;
using UnityEngine;

public class ParticleScriptDemo : MonoBehaviour
{
	public BulletPrefeb particlePrefeb;
	public float gap;
	public float wait;
	public float speed;

	private void MyScript(BulletStatus particle)
	{
		if (particle.StartLifetime - particle.RemainingLifetime < wait)
			particle.Velocity = particle.Velocity.normalized * 0.01f;
		else
			particle.Velocity = particle.Velocity.normalized * speed;
	}

	private void Start()
	{
		// Regester function 'MyScript' as a script
		// To use the script, fill the function name in particle prefeb script module
		new UpdateEvent("MyScript", MyScript) { ParallelOnUpdate = true };
		// Create particle and generate storm
		var generator = GetComponent<StormGenerator>();
		var storm = new Storm();
		var particle = new Bullet(particlePrefeb);
		storm.AddBehavior(new Emission(EmitList.Sphere(100, 3, speed), particle, 0.5f, gap));
		generator.Generate(storm);
	}
}
