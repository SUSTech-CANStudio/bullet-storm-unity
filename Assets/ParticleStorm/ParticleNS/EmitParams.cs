using UnityEngine;

namespace ParticleStorm.ParticleNS
{
	/// <summary>
	/// Simplified emit parameters.
	/// </summary>
	public class EmitParams
	{
		/// <summary>
		/// Start position, (0, 0, 0) is the position of <see cref="StormGenerator"/>.
		/// </summary>
		public Vector3 Position { get; set; }
		public Vector3 Velocity { get; set; }
		public Vector3 Rotation3D { get; set; }
		public float StartSize { get; set; }
		public float StartLifetime { get; set; }
		public int MeshIndex { get; set; } = -1;
		public Color32 StartColor
		{
			get => startColor; set
			{
				startColor = value;
				colorChanged = true;
			}
		}

		public ParticleSystem.EmitParams Full
		{
			get
			{
				var result = new ParticleSystem.EmitParams();
				result.position = Position;
				result.velocity = Velocity;
				result.rotation3D = Rotation3D;
				if (colorChanged) { result.startColor = StartColor; }
				if (StartSize > 0) { result.startSize = StartSize; }
				if (StartLifetime > 0) { result.startLifetime = StartLifetime; }
				if (MeshIndex >= 0) { result.meshIndex = MeshIndex; }
				return result;
			}
		}


		public EmitParams() { }

		public EmitParams(EmitParams emitParams)
		{
			bool flag = emitParams.colorChanged;
			Position = emitParams.Position;
			Velocity = emitParams.Velocity;
			Rotation3D = emitParams.Rotation3D;
			StartColor = emitParams.StartColor;
			StartSize = emitParams.StartSize;
			StartLifetime = emitParams.StartLifetime;
			MeshIndex = emitParams.MeshIndex;
			colorChanged = flag;
		}

		/// <summary>
		/// Get relative parameters of a transform.
		/// </summary>
		/// <param name="transform">The parent transform.</param>
		/// <returns></returns>
		public EmitParams RelativeParams(Transform transform)
		{
			var rel = new EmitParams(this);
			rel.Velocity = transform.rotation * Velocity;
			rel.Rotation3D = (transform.rotation * Quaternion.Euler(Rotation3D)).eulerAngles;
			rel.Position = transform.rotation * Position + transform.position;
			return rel;
		}

		/// <summary>
		/// Adjust parameters for lag.
		/// </summary>
		/// <param name="time">Lagged time.</param>
		/// <returns></returns>
		public EmitParams Lag(float time)
		{
			var res = new EmitParams(this);
			res.Position += Velocity * time;
			res.StartLifetime -= time;
			return res;
		}

		private Color32 startColor;
		private bool colorChanged = false;
	}
}
