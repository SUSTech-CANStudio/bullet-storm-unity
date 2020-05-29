using UnityEngine;

namespace ParticleStorm.Util
{
	/// <summary>
	/// Represent a <see cref="UnityEngine.Vector3"/> velocity or coordinate using spherical coordinates.
	/// </summary>
	public class Sphere
	{
		/// <summary>
		/// Radius
		/// </summary>
		public float R { get; set; }
		/// <summary>
		/// θ in degree.
		/// </summary>
		public float Theta { get => Mathf.Rad2Deg * radTheta; set => radTheta = Mathf.Deg2Rad * value; }
		/// <summary>
		/// φ in degree.
		/// </summary>
		public float Phi { get => Mathf.Rad2Deg * radPhi; set => radPhi = Mathf.Deg2Rad * value; }

		public Vector3 Vector3 { get => ToVecor3(); }

		private float radTheta;
		private float radPhi;

		public Sphere Normalized { get => new Sphere(1, Theta, Phi); }

		/// <summary>
		/// Create sphere using r, θ, φ.
		/// </summary>
		/// <param name="r">r</param>
		/// <param name="theta">θ</param>
		/// <param name="phi">φ</param>
		public Sphere(float r, float theta, float phi)
		{
			this.R = r;
			this.Theta = float.IsNaN(theta) ? 0 : theta;
			this.Phi = float.IsNaN(phi) ? 0 : phi;
		}

		/// <summary>
		/// Create sphere from vector3.
		/// </summary>
		/// <param name="vector3"></param>
		public Sphere(Vector3 vector3)
		{
			var y = Settings.UseRightHandedCoordinateSystem ? vector3.z : vector3.y;
			var z = Settings.UseRightHandedCoordinateSystem ? vector3.y : vector3.z;
			R = vector3.magnitude;

			radTheta = Mathf.Acos(z/ R);
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
			R = sphere.R;
			radTheta = sphere.radTheta;
			radPhi = sphere.radPhi;
		}

		public Vector3 ToVecor3()
		{
			if (R == 0) return Vector3.zero;
			float rstheta = R * Mathf.Sin(radTheta);

			var x = rstheta * Mathf.Cos(radPhi);
			var y = rstheta * Mathf.Sin(radPhi);
			var z = R * Mathf.Cos(radTheta);

			if (Settings.UseRightHandedCoordinateSystem)
				return new Vector3(x, z, y);
			else
				return new Vector3(x, y, z);
		}

		public static Sphere operator +(Sphere a, Sphere b) => new Sphere(a.Vector3 + b.Vector3);
		public static Sphere operator +(Sphere a, Vector3 b) => new Sphere(a.Vector3 + b);
		public static Vector3 operator +(Vector3 a, Sphere b) => a + b.Vector3;
		public static Sphere operator -(Sphere a, Sphere b) => new Sphere(a.Vector3 - b.Vector3);
		public static Sphere operator -(Sphere a, Vector3 b) => new Sphere(a.Vector3 - b);
		public static Vector3 operator -(Vector3 a, Sphere b) => a - b.Vector3;
		public static Sphere operator *(Sphere a, float b) => new Sphere(a.R * b, a.Theta, a.Phi);
		public static Sphere operator /(Sphere a, float b) => new Sphere(a.R / b, a.Theta, a.Phi);

		public override string ToString()
		{
			return "Sphere(" + R + ", " + Theta + ", " + Phi + ")";
		}
	}
}
