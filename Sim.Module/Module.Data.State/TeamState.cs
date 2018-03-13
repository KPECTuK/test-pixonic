using System;
using Sim.Module.Data.Ids;
using UnityEngine;

namespace Sim.Module.Data.State
{
	public class TeamState : IStateContainer<TeamState>
	{
		public TeamId Id { get; set; }
		public Vector3[] SpawnPoints { get; set; }

		public TeamState Copy()
		{
			return new TeamState
			{
				Id = Id, // immutable
			};
		}

		public TeamState GetDifference(TeamState source)
		{
			throw new NotImplementedException();
		}

		public void ApplyDifference(TeamState difference)
		{
			throw new NotImplementedException();
		}
	}
}