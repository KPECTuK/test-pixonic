using Newtonsoft.Json;
using Sim.Module.Data.Ids;
using UnityEngine;

namespace Sim.Module.Data.State
{
	public class PlayerState : IStateContainer<PlayerState>
	{
		[JsonProperty("pid")] public PlayerId Id { get; set; }
		[JsonProperty("pos")] public Vector3 Position { get; set; }
		[JsonProperty("spd")] public Vector3 Speed { get; set; }
		[JsonProperty("cbh")] public IUnitBehavior ActiveBehavior { get; set; }
		[JsonProperty("tem")] public TeamId TeamId { get; set; }
		[JsonProperty("hro")] public string HeroName { get; set; }

		public PlayerState Copy()
		{
			return new PlayerState
			{
				Id = Id, // immutable
				Position = Position,
				Speed = Speed,
				ActiveBehavior = ActiveBehavior.Copy(),
				TeamId = TeamId, // immutable
				HeroName = HeroName,
			};
		}

		public PlayerState GetDifference(PlayerState source)
		{
			return new PlayerState
			{
				Id = Id,
				Position = source.Position - Position,
				Speed = source.Speed - Speed,
				ActiveBehavior = source.ActiveBehavior.Equals(ActiveBehavior)
					? null
					: source.ActiveBehavior.Copy(),
				TeamId = TeamId,
			};
		}

		public void ApplyDifference(PlayerState difference)
		{
			Position += difference.Position;
			Speed += difference.Speed;
			ActiveBehavior = difference.ActiveBehavior ?? ActiveBehavior;
		}
	}
}