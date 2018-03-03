using System;
using System.Linq;

namespace Sim.Module.Data.State
{
	public class RealmState : IStateContainer<RealmState>
	{
		public PlayerState[] PlayerStates { get; set; }

		public RealmState Copy()
		{
			return new RealmState { PlayerStates = PlayerStates.Select(_ => _?.Copy()).ToArray() };
		}

		public RealmState GetDifference(RealmState source)
		{
			throw new NotImplementedException();
		}

		public void ApplyDifference(RealmState difference)
		{
			throw new NotImplementedException();
		}
	}
}