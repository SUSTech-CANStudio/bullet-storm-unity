using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		/// <returns></returns>
		public static List<EmitParams> Cone(List<EmitParams> @params, float radius, float theta, float speed, OverlayMode mode = OverlayMode.COVER)
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
			return @params;
		}

		/// <summary>
		/// The cone generator.
		/// </summary>
		/// <param name="num">Number of particles.</param>
		/// <param name="radius">Particle distance from origin point.</param>
		/// <param name="theta">Half vertex angle.</param>
		/// <param name="speed">Particle speed.</param>
		/// <returns></returns>
		public static List<EmitParams> Cone(int num, float radius, float theta, float speed)
		{
			var @params = Empty(num);
			Cone(@params, radius, theta, speed);
			return @params;
		}

		/// <summary>
		/// The size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="size">Particle start size.</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static List<EmitParams> Size(List<EmitParams> @params, float size, OverlayMode mode = OverlayMode.COVER)
		{
			Parallel.For(0, @params.Count, i =>
			{
				@params[i].StartSize = Operate(@params[i].StartSize, size, mode);
			});
			return @params;
		}

		/// <summary>
		/// The gradual change size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="fromSize">Size of the first particle.</param>
		/// <param name="toSize">Size gradually change to.</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static List<EmitParams> Size(List<EmitParams> @params, float fromSize, float toSize, OverlayMode mode = OverlayMode.COVER)
		{
			float dsize = (toSize - fromSize) / @params.Count;
			Parallel.For(0, @params.Count, i =>
			{
				@params[i].StartSize = Operate(@params[i].StartSize, fromSize + dsize * i, mode);
			});
			return @params;
		}

		/// <summary>
		/// The random size filter.
		/// </summary>
		/// <param name="params"></param>
		/// <param name="min">Random min.</param>
		/// <param name="max">Random max.</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static List<EmitParams> RandomSize(List<EmitParams> @params, float min, float max, OverlayMode mode = OverlayMode.COVER)
		{
			for (int i = 0; i < @params.Count; i++)
				@params[i].StartSize = Operate(@params[i].StartSize, Random.Range(min, max), mode);
			return @params;
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
