using System.Collections.Generic;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using UnityEngine;

namespace Sim.Module.Client
{
	public interface IRealmController
	{
		void SetupPlayer(PlayerState state);
		void ReSpawnPlayer(PlayerId id);
		Vector3 GetSpawnPoint(TeamId teamId);
	}
}