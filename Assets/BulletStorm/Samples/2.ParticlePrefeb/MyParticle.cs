using BulletStorm.BulletNS;
using UnityEngine;

public class MyParticle : MonoBehaviour
{
	public BulletPrefeb prefeb;

	private void Start()
	{
		Bullet particle = new Bullet(prefeb);
		particle.Origin.Emit(new EmitParams());
	}
}
