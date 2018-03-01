using System;
using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class UltimateDataCastMod : UltimateDataBase<UltimateDataCastMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("duration")] public TimeSpan Duration { get; set; }
		[JsonProperty("cooldown")] public TimeSpan Colldown { get; set; }

		public override UltimateDataCastMod Copy()
		{
			throw new NotImplementedException();
		}
	}
}