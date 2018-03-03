using System;
using Newtonsoft.Json;

namespace Sim.Module.Data.Config
{
	public class UltimateDataSpeedMod : UltimateDataBase<UltimateDataSpeedMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("duration")] public TimeSpan Duration { get; set; }

		public override UltimateDataSpeedMod Copy()
		{
			var result = base.Copy();
			result.Factor = Factor;
			result.Duration = Duration;
			return result;
		}
	}
}