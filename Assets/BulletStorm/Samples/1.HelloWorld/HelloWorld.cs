using BulletStorm.BulletNS;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
	// Start is called before the first frame update
	private void Start()
	{
		Bullet particle = new Bullet();
		particle.Origin.Emit(new EmitParams());
	}
}
