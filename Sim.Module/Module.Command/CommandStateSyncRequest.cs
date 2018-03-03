using System.Linq;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Command
{
	public class CommandStateSyncRequest : CommandClientSide
	{
		public PlayerState[] Deference { get; set; }

		public override void Execute()
		{
			var repository = Context.Resolve<IRepository>();
			repository.ShiftStates();
			foreach(var state in repository.GetPlayerStates(null))
			{
				state.ApplyDifference(Deference.FirstOrDefault(_ => _.Id.Equals(state.Id)));
			}
			var command = Context.Resolve<ICommandBuilder>().BuildResponse<CommandStateSyncResponse, CommandStateSyncRequest>(this);
			command.StateUpdate = Context.Resolve<IRepository>().GetPlayerStates(_ => _.Id.Equals(Context.Resolve<SimService>().LocalId)).FirstOrDefault();
			Context.Resolve<INetworkClientConnection>().SendToServer(command);
		}
	}
}