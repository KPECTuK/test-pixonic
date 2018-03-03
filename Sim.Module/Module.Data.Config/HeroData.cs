using System.Linq;
using Newtonsoft.Json;

namespace Sim.Module.Data.Config
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
			return new HeroData
			{
				Name = Name,
				Speed = Speed,
				AttackPower = AttackPower,
				AttackRadius = AttackRadius,
				AttackSpeed = AttackSpeed,
				Behavior = Behavior.Select(_ => _.Copy()).ToArray(),
				Ultimate = Ultimate.Select(_ => _.Copy()).ToArray()
			};
		}
	}
}