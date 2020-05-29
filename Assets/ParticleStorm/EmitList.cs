using ParticleStorm.Util;
using System;
using System.Collections.Generic;

namespace ParticleStorm
{
	/// <summary>
	/// List of <see cref="EmitParams"/>.<para/>
	/// This is the class to generate and modify particle emission parameters.
	/// </summary>
	public class EmitList
	{
		/// <summary>
		/// List of emission parameters.
		/// </summary>
		internal List<EmitParams> List { get; private set; }

		public EmitParams this[int index] { get => List[index]; set => List[index] = value; }

		public int Count => List.Count;

		private EmitList(List<EmitParams> list) => List = list;

		/// <summary>
		/// Create an empty emit list.
		/// </summary>
		/// <param name="num">Number of emissions</param>
		public EmitList(int num) => List = Filters.Empty(num);

		/// <summary>
		/// Create a cone emit list.
		/// </summary>
		/// <param name="num">Number of particles</param>
		/// <param name="radius">Particle distance from origin point</param>
		/// <param name="theta">Half vertex angle</param>
		/// <param name="speed">Particle speed</param>
		/// <returns></returns>
		public static EmitList Cone(int num, float radius, float theta, float speed)
		{
			var emitList = new EmitList(num);
			Filters.Cone(emitList.List, radius, theta, speed);
			return emitList;
		}

		/// <summary>
		/// Create a sphere emit list using Fibonacci sphere algorithm.
		/// </summary>
		/// <param name="num">Number of particles</param>
		/// <param name="radius">Radius of the sphere</param>
		/// <param name="speed">Particle speed</param>
		/// <returns></returns>
		public static EmitList Sphere(int num, float radius, float speed)
		{
			var emitList = new EmitList(num);
			Filters.FibonacciSphere(emitList.List, radius, speed);
			return emitList;
		}

		/// <summary>
		/// Create a sphere emit list using random points.
		/// </summary>
		/// <param name="num">Number of particles</param>
		/// <param name="radius">Radius of the sphere</param>
		/// <param name="speed">Particle speed</param>
		/// <returns></returns>
		public static EmitList RandomSphere(int num, float radius, float speed)
		{
			var emitList = new EmitList(num);
			Filters.RandomSphere(emitList.List, radius, speed);
			return emitList;
		}

		/// <summary>
		/// Apply cone shape filter.
		/// </summary>
		/// <param name="radius">Particle distance from origin point.</param>
		/// <param name="theta">Half vertex angle.</param>
		/// <param name="speed">Particle speed.</param>
		/// <param name="mode">Overlay mode.</param>
		/// <returns></returns>
		public EmitList ConeFilter(float radius, float theta, float speed, OverlayMode mode = OverlayMode.COVER)
		{
			Filters.Cone(List, radius, theta, speed, mode);
			return this;
		}

		/// <summary>
		/// Apply size filter.
		/// </summary>
		/// <param name="size">Size of particles.</param>
		/// <param name="mode">Overlay mode.</param>
		/// <returns></returns>
		public EmitList SizeFilter(float size, OverlayMode mode = OverlayMode.COVER)
		{
			Filters.Size(List, size, mode);
			return this;
		}

		/// <summary>
		/// Apply gradual change size filter.
		/// </summary>
		/// <param name="fromSize">Size of the first particle.</param>
		/// <param name="toSize">Size gradually change to.</param>
		/// <param name="mode">Overlay mode.</param>
		/// <returns></returns>
		public EmitList SizeFilter(float fromSize, float toSize, OverlayMode mode = OverlayMode.COVER)
		{
			Filters.Size(List, fromSize, toSize, mode);
			return this;
		}

		/// <summary>
		/// Apply random size filter.
		/// </summary>
		/// <param name="min">Min size.</param>
		/// <param name="max">Max size.</param>
		/// <param name="mode">Overlay mode.</param>
		/// <returns></returns>
		public EmitList RandomSizeFilter(float min, float max, OverlayMode mode = OverlayMode.COVER)
		{
			Filters.RandomSize(List, min, max, mode);
			return this;
		}

		public static EmitList operator +(EmitList a, EmitList b)
		{
			if (a == null)
			{
				throw new ArgumentNullException(nameof(a));
			}
			if (b == null)
			{
				throw new ArgumentNullException(nameof(b));
			}
			var result = new List<EmitParams>(a.List);
			result.AddRange(b.List);
			return new EmitList(result);
		}

		public static EmitList operator *(EmitList a, int b)
		{
			if (a == null)
			{
				throw new ArgumentNullException(nameof(a));
			}
			var result = new List<EmitParams>();
			var item = new List<EmitParams>(a.List);
			if (b < 0)
			{
				b = -b;
				item.Reverse();
			}
			for (int i = 0; i < b; i++)
			{
				result.AddRange(item);
			}
			return new EmitList(result);
		}

		public static EmitList operator *(int a, EmitList b) => b * a;

		public static EmitList Add(EmitList left, EmitList right)
		{
			return left + right;
		}

		public static EmitList Multiply(EmitList left, int right)
		{
			return left * right;
		}
	}
}
