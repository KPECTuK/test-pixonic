using System;
using System.Collections.Generic;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using UnityEngine;

namespace Sim.Module.Data
{
	public interface IRepository
	{
		TeamId[] Teams { get; }
		void ReloadConfig();
		void ReloadState(int frames);
		void ShiftStates();
		void SetTeam(TeamId teamId);
		Vector3[] GetSpawnPoints(TeamId teamId);
		IEnumerable<T> GetConfig<T>(Func<T, bool> filter) where T : class, IDataContainer<T>, new();
		void MergeStates(PlayerState[] difference);
		IEnumerable<PlayerState> GetPlayerStates(Func<PlayerState, bool> filter);
	}
}