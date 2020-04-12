using ParticleStorm.Util;
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
		public List<EmitParams> list { get; private set; }

		public EmitParams this[int index] { get => list[index]; set => list[index] = value; }

		public int Count => list.Count;

		private EmitList(List<EmitParams> list) => this.list = list;

		/// <summary>
		/// Create an empty emit list.
		/// </summary>
		/// <param name="num">Number of emissions.</param>
		public EmitList(int num)
		{
			list = Filters.Empty(num);
		}

		/// <summary>
		/// Create a cone emit list.
		/// </summary>
		/// <param name="num">Number of particles.</param>
		/// <param name="radius">Particle distance from origin point.</param>
		/// <param name="theta">Half vertex angle.</param>
		/// <param name="speed">Particle speed.</param>
		/// <returns></returns>
		public static EmitList Cone(int num, float radius, float theta, float speed)
		{
			return new EmitList(Filters.Cone(num, radius, theta, speed));
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
			Filters.Cone(list, radius, theta, speed, mode);
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
			Filters.Size(list, size, mode);
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
			Filters.Size(list, fromSize, toSize, mode);
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
			Filters.RandomSize(list, min, max, mode);
			return this;
		}

		public static EmitList operator +(EmitList a, EmitList b)
		{
			var result = new List<EmitParams>(a.list);
			result.AddRange(b.list);
			return new EmitList(result);
		}

		public static EmitList operator *(EmitList a, int b)
		{
			var result = new List<EmitParams>();
			var item = new List<EmitParams>(a.list);
			if (b < 0)
			{
				b = -b;
				item.Reverse();
			}
			for (int i = 0; i < b; i++)
				result.AddRange(item);
			return new EmitList(result);
		}

		public static EmitList operator *(int a, EmitList b) => b * a;
	}
}
