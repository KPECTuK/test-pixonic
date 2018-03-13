using Newtonsoft.Json;

namespace Sim.Module.Data.Config.Ultimate
{
	public class UltimateDataAttackPowerMod : UltimateDataBase<UltimateDataAttackPowerMod>
	{
		[JsonProperty("factor")] public float Factor { get; set; }
		[JsonProperty("distance")] public float Distance { get; set; }
		[JsonProperty("chance")] public float Chance { get; set; }

		public override UltimateDataAttackPowerMod Copy()
		{
			var result = base.Copy();
			result.Factor = Factor;
			result.Distance = Distance;
			result.Chance = Chance;
			return result;
		}
	}
}