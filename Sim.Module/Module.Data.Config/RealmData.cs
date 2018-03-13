using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Sim.Module.Data.Config
{
	public class RealmData : IDataContainer<RealmData>
	{
		[JsonProperty("sync-interval")] public TimeSpan TargetSyncInterval { get; set; }
		[JsonProperty("sync-deviation")] public TimeSpan TargetSyncDeviation { get; set; }
		[JsonProperty("max-players")] public int TotalPlayers { get; set; }
		[JsonProperty("max-teams")] public int TotalTeams { get; set; }
		[JsonProperty("spawn-points")] public Vector3[][] SpawnPoints { get; set; }

		public RealmData Copy()
		{
			var spawnPoints = new Vector3[SpawnPoints.Length][];
			for(var index = 0; index < spawnPoints.Length; index++)
			{
				var sidePoints = new Vector3[SpawnPoints[index].Length];
				Array.Copy(SpawnPoints[index], sidePoints, SpawnPoints[index].Length);
				spawnPoints[index] = sidePoints;
			}

			return new RealmData
			{
				TotalTeams = TotalTeams,
				TargetSyncInterval = TargetSyncInterval,
				SpawnPoints = spawnPoints,
			};
		}
	}
}