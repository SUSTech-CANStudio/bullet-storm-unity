using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ParticleStorm.Util
{
	internal static class Filters
	{
		/// <summary>
		/// The empty generator.<para/>
		/// Create an empty emission parameters list.
		/// </summary>
		/// <param name="num">Number of emissions.</param>
		/// <returns></returns>
		public static List<EmitParams> Empty(int num)
		{
			var @params = new List<EmitParams>(num);
			for (int i = 0; i < num; i++)
			{
				@params.Add(new EmitParams());
			}
			return @params;
		}

		/// <summary>
		/// The cone filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="radius">Particle distance from origin point.</param>
		/// <param name="theta">Half vertex angle.</param>
		/// <param name="speed">Particle speed.</param>
		/// <param name="mode"></param>
		public static void Cone(List<EmitParams> @params, float radius, float theta, float speed, OverlayMode mode = OverlayMode.COVER)
		{
			float dphi = 360.0f / @params.Count;
			Parallel.For(0, @params.Count, i =>
			{
				float phi = i * dphi;

				@params[i].Position = Operate(@params[i].Position, new Sphere(radius, theta, phi).Vector3, mode);
				@params[i].Velocity = Operate(@params[i].Velocity, new Sphere(speed, theta, phi).Vector3, mode);
				@params[i].Rotation3D = (Quaternion.LookRotation(@params[i].Velocity)
						  * Quaternion.Euler(@params[i].Rotation3D)).eulerAngles;
			});
		}

		/// <summary>
		/// Use Fibonacci sphere algorithm to generate a simillar equal-distance point sphere.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="radius">Radius of the sphere</param>
		/// <param name="speed">Particle start speed</param>
		/// <param name="mode"></param>
		public static void FibonacciSphere(List<EmitParams> @params, float radius, float speed, OverlayMode mode = OverlayMode.COVER)
		{
			const float ga = 2.39996322972865332f;  // golden angle = 2.39996322972865332
			Vector3 vector;

			for (int i = 0; i < @params.Count; i++)
			{
				float lat = Mathf.Asin(-1.0f + 2.0f * i / (@params.Count + 1));
				float lon = ga * i;

				vector.x = Mathf.Cos(lon) * Mathf.Cos(lat);
				vector.y = Mathf.Sin(lon) * Mathf.Cos(lat);
				vector.z = Mathf.Sin(lat);

				@params[i].Position = Operate(@params[i].Position, vector * radius, mode);
				@params[i].Velocity = Operate(@params[i].Velocity, vector * speed, mode);
				@params[i].Rotation3D = (Quaternion.LookRotation(@params[i].Velocity)
						  * Quaternion.Euler(@params[i].Rotation3D)).eulerAngles;
			}
		}

		/// <summary>
		/// Random points on a sphere
		/// </summary>
		/// <param name="params"></param>
		/// <param name="radius">Radius of the sphere</param>
		/// <param name="speed">Particle start speed</param>
		/// <param name="mode"></param>
		public static void RandomSphere(List<EmitParams> @params, float radius, float speed, OverlayMode mode = OverlayMode.COVER)
		{
			Vector3 vector;
			foreach (var param in @params)
			{
				vector = Random.onUnitSphere;
				param.Position = Operate(param.Position, vector * radius, mode);
				param.Velocity = Operate(param.Velocity, vector * speed, mode);
				param.Rotation3D = (Quaternion.LookRotation(param.Velocity) * Quaternion.Euler(param.Rotation3D)).eulerAngles;
			}
		}

		/// <summary>
		/// The size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="size">Particle start size.</param>
		/// <param name="mode"></param>
		public static void Size(List<EmitParams> @params, float size, OverlayMode mode = OverlayMode.COVER)
		{
			Parallel.For(0, @params.Count, i =>
			{
				@params[i].StartSize = Operate(@params[i].StartSize, size, mode);
			});
		}

		/// <summary>
		/// The gradual change size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="fromSize">Size of the first particle.</param>
		/// <param name="toSize">Size gradually change to.</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static void Size(List<EmitParams> @params, float fromSize, float toSize, OverlayMode mode = OverlayMode.COVER)
		{
			float dsize = (toSize - fromSize) / @params.Count;
			Parallel.For(0, @params.Count, i =>
			{
				@params[i].StartSize = Operate(@params[i].StartSize, fromSize + dsize * i, mode);
			});
		}

		/// <summary>
		/// The random size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="min">Random min.</param>
		/// <param name="max">Random max.</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static void RandomSize(List<EmitParams> @params, float min, float max, OverlayMode mode = OverlayMode.COVER)
		{
			for (int i = 0; i < @params.Count; i++)
				@params[i].StartSize = Operate(@params[i].StartSize, Random.Range(min, max), mode);
		}

		private static float Operate(float a, float b, OverlayMode mode)
		{
			switch (mode)
			{
				case OverlayMode.COVER:
					return b;
				case OverlayMode.ADD:
					return a + b;
				case OverlayMode.MINUS:
					return a - b;
				case OverlayMode.MULTIPLY:
					return a * b;
				case OverlayMode.DIVIDE:
					if (b == 0)
						throw new DivideByZeroException("Can't use divide mode for this float");
					return a / b;
				case OverlayMode.AVERAGE:
					return (a + b) / 2;
				default:
					throw new InvalidOperationException("No such an OverlayMode: " + mode.ToString());
			}
		}

		private static Vector3 Operate(Vector3 a, Vector3 b, OverlayMode mode)
		{
			switch (mode)
			{
				case OverlayMode.COVER:
					return b;
				case OverlayMode.ADD:
					return a + b;
				case OverlayMode.MINUS:
					return a - b;
				case OverlayMode.MULTIPLY:
					return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
				case OverlayMode.DIVIDE:
					if (b.x == 0 || b.y == 0 || b.z == 0)
						throw new DivideByZeroException("Can't use divide mode for this vector");
					return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
				case OverlayMode.AVERAGE:
					return (a + b) / 2;
				default:
					throw new InvalidOperationException("No such an OverlayMode: " + mode.ToString());
			}
		}

		private static Color32 Operate(Color32 a, Color32 b, OverlayMode mode)
		{
			byte trans(int i)
			{
				if (i < 0)
					return 0;
				if (i > 255)
					return 255;
				return (byte)i;
			}
			switch (mode)
			{
				case OverlayMode.COVER:
					return b;
				case OverlayMode.ADD:
					return new Color32(
						trans(a.r * a.a / 255 + b.r * b.a / 255),
						trans(a.g * a.a / 255 + b.g * b.a / 255),
						trans(a.b * a.a / 255 + b.b * b.a / 255),
						trans(a.a + b.a));
				case OverlayMode.MINUS:
					return new Color32(
						trans(a.r * a.a / 255 - b.r * b.a / 255),
						trans(a.g * a.a / 255 - b.g * b.a / 255),
						trans(a.b * a.a / 255 - b.b * b.a / 255),
						trans(a.a + b.a));
				case OverlayMode.MULTIPLY:
					return new Color32(
						trans((a.r * a.a / 255) * (b.r * b.a / 255)),
						trans((a.g * a.a / 255) * (b.g * b.a / 255)),
						trans((a.b * a.a / 255) * (b.b * b.a / 255)),
						trans(a.a + b.a));
				case OverlayMode.AVERAGE:
					return new Color32(
						trans((a.r * a.a / 255 + b.r * b.a / 255) / 2),
						trans((a.g * a.a / 255 + b.g * b.a / 255) / 2),
						trans((a.b * a.a / 255 + b.b * b.a / 255) / 2),
						trans((a.a + b.a) / 2));
				default:
					throw new InvalidOperationException("No such an OverlayMode: " + mode.ToString());
			}
		}
	}
}
