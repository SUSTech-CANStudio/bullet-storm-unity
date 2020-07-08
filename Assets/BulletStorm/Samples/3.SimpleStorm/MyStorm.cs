using BulletStorm.BulletNS;
using BulletStorm.StormNS;
using BulletStorm.StormNS.Behavior;
using UnityEngine;

public class MyStorm : MonoBehaviour
{
	public BulletPrefeb particlePrefeb;
	public float gap;

	private void Start()
	{
		// Get generator component
		var generator = GetComponent<StormGenerator>();
		// Create an empty storm
		var storm = new Storm();
		// Create a particle from prefeb
		var particle = new Bullet(particlePrefeb);
		// Create an emit list.
		var emitList = EmitList.Cone(100, 2, 90, 1);
		// Add an emission behavior to storm.
		storm.AddBehavior(new Emission(emitList, particle, 0, gap));
		// Generate storm.
		generator.Generate(storm);
	}
}
