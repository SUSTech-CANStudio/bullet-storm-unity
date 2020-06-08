using UnityEngine;

namespace ParticleStorm.ParticleNS
{
	internal class ParticleSystemControllerUpdater : MonoBehaviour
	{
		private ParticleSystemController controller;

		public void UpdateFor(ParticleSystemController controller) => this.controller = controller;

		private void Update() => controller.Update();

		private void FixedUpdate() => controller.FixedUpdate();

		private void LateUpdate() => controller.LateUpdate();

		private void OnParticleCollision(GameObject other) => controller.OnParticleCollision(other);
	}
}
