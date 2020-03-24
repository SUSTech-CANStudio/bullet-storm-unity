using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ParticleStorm.Util
{
	/// <summary>
	/// Represent a <see cref="Vector3"/> velocity or coordinate using spherical coordinates.
	/// </summary>
	public class Sphere
	{
		/// <summary>
		/// Radius
		/// </summary>
		public float r { get; set; }
		/// <summary>
		/// θ in degree.
		/// </summary>
		public float theta { get => Mathf.Rad2Deg * radTheta; set => radTheta = Mathf.Deg2Rad * value; }
		/// <summary>
		/// φ in degree.
		/// </summary>
		public float phi { get => Mathf.Rad2Deg * radPhi; set => radPhi = Mathf.Deg2Rad * value; }

		public Vector3 vector3 { get => ToVecor3(); }

		private float radTheta;
		private float radPhi;

		public Sphere normalized { get => new Sphere(1, theta, phi); }

		/// <summary>
		/// Create sphere using r, θ, φ.
		/// </summary>
		/// <param name="r">r</param>
		/// <param name="theta">θ</param>
		/// <param name="phi">φ</param>
		public Sphere(float r, float theta, float phi)
		{
			this.r = r;
			this.theta = float.IsNaN(theta) ? 0 : theta;
			this.phi = float.IsNaN(phi) ? 0 : phi;
		}

		/// <summary>
		/// Create sphere from vector3.
		/// </summary>
		/// <param name="vector3"></param>
		public Sphere(Vector3 vector3)
		{
			var y = Settings.useRightHandedCoordinateSystem ? vector3.z : vector3.y;
			var z = Settings.useRightHandedCoordinateSystem ? vector3.y : vector3.z;
			r = vector3.magnitude;

			radTheta = Mathf.Acos(z/ r);
			radPhi = Mathf.Atan(y / vector3.x);

			if (float.IsNaN(radTheta))
				radTheta = 0;
			if (float.IsNaN(radPhi))
				radPhi = 0;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="sphere"></param>
		public Sphere(Sphere sphere)
		{
			r = sphere.r;
			radTheta = sphere.radTheta;
			radPhi = sphere.radPhi;
		}

		public Vector3 ToVecor3()
		{
			if (r == 0) return Vector3.zero;
			float rstheta = r * Mathf.Sin(radTheta);

			var x = rstheta * Mathf.Cos(radPhi);
			var y = rstheta * Mathf.Sin(radPhi);
			var z = r * Mathf.Cos(radTheta);

			if (Settings.useRightHandedCoordinateSystem)
				return new Vector3(x, z, y);
			else
				return new Vector3(x, y, z);
		}

		public static Sphere operator +(Sphere a, Sphere b) => new Sphere(a.vector3 + b.vector3);
		public static Sphere operator +(Sphere a, Vector3 b) => new Sphere(a.vector3 + b);
		public static Vector3 operator +(Vector3 a, Sphere b) => a + b.vector3;
		public static Sphere operator -(Sphere a, Sphere b) => new Sphere(a.vector3 - b.vector3);
		public static Sphere operator -(Sphere a, Vector3 b) => new Sphere(a.vector3 - b);
		public static Vector3 operator -(Vector3 a, Sphere b) => a - b.vector3;
		public static Sphere operator *(Sphere a, float b) => new Sphere(a.r * b, a.theta, a.phi);
		public static Sphere operator /(Sphere a, float b) => new Sphere(a.r / b, a.theta, a.phi);

		public override string ToString()
		{
			return "Sphere(" + r + ", " + theta + ", " + phi + ")";
		}
	}
}
