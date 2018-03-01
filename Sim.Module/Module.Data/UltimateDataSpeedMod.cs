using System;
using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class UltimateDataSpeedMod : UltimateDataBase<UltimateDataSpeedMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("duration")] public TimeSpan Duration { get; set; }

		public override UltimateDataSpeedMod Copy()
		{
			throw new NotImplementedException();
		}
	}
}