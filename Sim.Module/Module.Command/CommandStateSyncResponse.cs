using Sim.Module.Data;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Command
{
	public class CommandStateSyncResponse : CommandServerSide
	{
		public PlayerState StateUpdate { get; set; }

		public override void Execute()
		{
			Context.Resolve<IRepository>().MergeStates(new[] { StateUpdate });
		}
	}
}