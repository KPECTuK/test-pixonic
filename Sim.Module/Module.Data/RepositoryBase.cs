using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Data.Serialization;
using Sim.Module.Data.State;
using Sim.Module.Generic;
using Sim.Module.Resources;
using UnityEngine;

namespace Sim.Module.Data
{
	public abstract class RepositoryBase : IRepository, IProvider<RealmData>
	{
		protected readonly IContext Context;
		protected readonly JsonSerializerSettings SerializerSettings;
		// config
		private RealmData _reamData;
		private HeroData[] _heroesCache;
		// state
		private TeamState[] _teams;
		private RealmState[] _states;
		private int _currentStateIndex;
		private readonly Func<PlayerState, bool> _filterDefaultPlayerState = _ => true;

		protected RealmState CurrentState => _states[_currentStateIndex];

		public TeamId[] Teams => _teams.Select(_ => _.Id).ToArray();

		public RepositoryBase(IContext context)
		{
			Context = context;
			SerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				SerializationBinder = new CustomTypeBinder(),
				Converters = new JsonConverter[] { new TimeSpanConverter(), }
			};
		}

		public IEnumerable<PlayerState> GetPlayerStates(Func<PlayerState, bool> filter)
		{
			return _states[_currentStateIndex].PlayerStates.Where(filter ?? _filterDefaultPlayerState);
		}

		public abstract void MergeStates(PlayerState[] difference);

		public void ShiftStates()
		{
			var previous = _currentStateIndex;
			_currentStateIndex = (_currentStateIndex + 1) % _states.Length;
			_states[_currentStateIndex] = _states[previous].Copy();
		}

		public void SetTeam(TeamId teamId)
		{
			var desc = _teams.FirstOrDefault(_ => ReferenceEquals(null, _.Id));
			if(!ReferenceEquals(null, teamId))
			{
				desc.Id = teamId;
			}
		}

		public Vector3[] GetSpawnPoints(TeamId teamId)
		{
			return _teams.FirstOrDefault(_ => _.Id.Equals(teamId))?.SpawnPoints;
		}

		public virtual void ReloadConfig()
		{
			var resources = Context.Resolve<IResourceFactory>();
			_reamData = JsonConvert
				.DeserializeObject<RealmData>(
					resources.GetResource<string>(new ResourceLocator { Filename = "realm.json" }),
					SerializerSettings);
			_heroesCache = JsonConvert
				.DeserializeObject<HeroData[]>(
					resources.GetResource<string>(new ResourceLocator { Filename = "heroes.json" }),
					SerializerSettings);
		}

		public virtual void ReloadState(int frames)
		{
			var realmData = Context.Resolve<RealmData>();

			_teams = new TeamState[realmData.TotalTeams];
			for(var index = 0; index < _teams.Length; index++)
			{
				_teams[index] = new TeamState { SpawnPoints = realmData.SpawnPoints[index % realmData.SpawnPoints.Length].ToArray() };
			}

			_states = new RealmState[frames];
			for(var index = 0; index < frames; index++)
			{
				_states[index] = new RealmState { PlayerStates = new PlayerState[realmData.TotalPlayers] };
			}
		}

		// IRepository
		public IEnumerable<T> GetConfig<T>(Func<T, bool> filter) where T : class, IDataContainer<T>, new()
		{
			if(typeof(T) == typeof(HeroData))
			{
				return _heroesCache.OfType<T>().Where(filter ?? (_ => true));
			}
			if(typeof(T) == typeof(RealmData))
			{
				return new[] { (_reamData as object) as T };
			}

			return new T[] { };
		}

		// IProvider<RealmData>
		RealmData IProvider<RealmData>.Get()
		{
			return _reamData;
		}
	}
}