using System;
using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class UltimateDataAttackPowerMod : UltimateDataBase<UltimateDataAttackPowerMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("distance")] public float Distance { get; set; }
		[JsonProperty("chance")] public float Chance { get; set; }

		public override UltimateDataAttackPowerMod Copy()
		{
			throw new NotImplementedException();
		}
	}
}