using System;
using Newtonsoft.Json;

namespace Sim.Module.Data.Config
{
	public class UltimateDataCastMod : UltimateDataBase<UltimateDataCastMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("duration")] public TimeSpan Duration { get; set; }
		[JsonProperty("cooldown")] public TimeSpan Cooldown { get; set; }

		public override UltimateDataCastMod Copy()
		{
			var result = base.Copy();
			result.Factor = Factor;
			result.Duration = Duration;
			result.Cooldown = Cooldown;
			return result;
		}
	}
}