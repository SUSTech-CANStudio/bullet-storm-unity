using UnityEngine;

namespace BulletStorm.BulletNS
{
	internal class BulletSystemControllerUpdater : MonoBehaviour
	{
		private BulletSystemController controller;

		public void UpdateFor(BulletSystemController controller) => this.controller = controller;

		private void Update() => controller.Update();

		private void FixedUpdate() => controller.FixedUpdate();

		private void LateUpdate() => controller.LateUpdate();

		private void OnParticleCollision(GameObject other) => controller.OnParticleCollision(other);
	}
}
