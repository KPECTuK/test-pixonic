using Sim.Module.Generic;

namespace Sim.Module.Extensions {
	public static class RangeExtensions
	{
		private static Range<float> _indent = new Range<float>(0f, 1f);

		public static float Interpolate(this Range<float> source, float value)
		{
			return source.Max - source.Min > 0
				? _indent.Clamp(value / (source.Max - source.Min))
				: 0f;
		}
	}
}