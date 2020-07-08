using BulletStorm.BulletNS;
using BulletStorm.StormNS;
using BulletStorm.StormNS.Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormWithBehaviors : MonoBehaviour
{
    public BulletPrefeb prefeb;
	// Start is called before the first frame update
	void Start()
    {
        Bullet particle = new Bullet(prefeb);
        Storm storm = new Storm();
        storm.AddBehavior(new Emission(EmitList.Sphere(100, 2, 1), particle, 0))
            .AddBehavior(new Wait(2, 4, particle));
        GetComponent<StormGenerator>().Generate(storm);
    }
}
