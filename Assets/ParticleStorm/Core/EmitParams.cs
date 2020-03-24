using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ParticleStorm.Util;

namespace ParticleStorm.Core
{
	public class EmitParams
	{
		/// <summary>
		/// Start position, (0, 0, 0) is the position of <see cref="StormGenerator"/>.
		/// </summary>
		public Vector3 position;
		public Vector3 velocity;
		public Vector3 rotation3D;
		public Color32 startColor { get => _startColor; set 
			{
				_startColor = value;
				colorChanged = true;
			}
		}
		public float startSize;
		public float startLifetime;
		public int meshIndex = -1;

		public ParticleSystem.EmitParams full
		{
			get
			{
				var result = new ParticleSystem.EmitParams();
				result.position = position;
				result.velocity = velocity;
				result.rotation3D = rotation3D;
				if (colorChanged)
					result.startColor = startColor;
				if (startSize > 0)
					result.startSize = startSize;
				if (startLifetime > 0)
					result.startLifetime = startLifetime;
				if (meshIndex >= 0)
					result.meshIndex = meshIndex;
				return result;
			}
		}

		public EmitParams() { }

		public EmitParams(EmitParams emitParams)
		{
			bool flag = emitParams.colorChanged;
			position = emitParams.position;
			velocity = emitParams.velocity;
			rotation3D = emitParams.rotation3D;
			startColor = emitParams.startColor;
			startSize = emitParams.startSize;
			startLifetime = emitParams.startLifetime;
			meshIndex = emitParams.meshIndex;
			colorChanged = flag;
		}

		public EmitParams RelativeParams(Transform transform)
		{
			var rel = new EmitParams(this);
			rel.velocity = transform.rotation * velocity;
			rel.rotation3D = (transform.rotation * Quaternion.Euler(rotation3D)).eulerAngles;
			rel.position = transform.rotation * position + transform.position;
			return rel;
		}

		private Color32 _startColor;
		private bool colorChanged = false;
	}
}
