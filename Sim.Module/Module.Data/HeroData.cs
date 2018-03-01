using System;
using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class HeroData : IDataContainer<HeroData>
	{
		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("speed")] public float Speed { get; set; }
		[JsonProperty("hit-points")] public int HitPoints { get; set; }
		[JsonProperty("attack-power")] public int AttackPower { get; set; }
		[JsonProperty("attack-radius")] public float AttackRadius { get; set; }
		[JsonProperty("attack-speed")] public float AttackSpeed { get; set; }
		[JsonProperty("behavior")] public IUnitBehavior[] Behavior { get; set; }
		[JsonProperty("ultimate")] public IUltimateData[] Ultimate { get; set; }

		public HeroData Copy()
		{
			throw new NotImplementedException();
		}
	}
}