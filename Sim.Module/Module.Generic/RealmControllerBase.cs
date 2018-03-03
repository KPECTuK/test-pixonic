using System;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using Sim.Module.Logger;
using UnityEngine;

namespace Sim.Module.Generic
{
	public abstract class RealmControllerBase : IRealmController
	{
		protected readonly Type SelfType;
		protected readonly Logger.ILogger Logger;
		protected readonly IContext Context;

		public RealmControllerBase(IContext context)
		{
			Context = context;
			SelfType = GetType();
			Logger = Context.Resolve<ILoggerFactory>().GetFor(SelfType);
		}

		public void ReSpawnPlayer(PlayerId id)
		{
			//throw new NotImplementedException();
			//var player = Context.Resolve<IRepository>().GetPlayerStates(-1, _ => _.Id.Equals(id));
		}

		public Vector3 GetSpawnPoint(TeamId teamId)
		{
			var set = Context.Resolve<IRepository>().GetSpawnPoints(teamId);
			// null
			var result = set[0];
			int index;
			for(index = 1; index < set.Length; index++)
			{
				set[index - 1] = set[index];
			}
			set[index - 1] = result;
			return result;
		}

		public abstract void SetupPlayer(PlayerState state);
	}
}