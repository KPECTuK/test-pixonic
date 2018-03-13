using System;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Command
{
	public class CommandJoinResponse : CommandClientSide
	{
		public PlayerState[] PlayerStates { get; set; }

		public override void Execute()
		{
			Array.ForEach(PlayerStates, _ => Context.Resolve<IRepository>().MergeStates(PlayerStates));
			Context.Resolve<ITimeService>().StartSession();
			Array.ForEach(PlayerStates, _ => Context.Resolve<IRealmController>().ReSpawnPlayer(_.Id));
		}
	}
}