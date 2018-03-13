using Sim.Module.Data.State;
using Sim.Module.Generic;
using Sim.Module.Simulation;

namespace Sim.Module.Command
{
	public class CommandJoinRequest : CommandServerSide
	{
		public PlayerState PlayerState { get; set; }

		public override void Execute()
		{
			Context.Resolve<MatchMaker>().AddRequester(this);
		}
	}
}