using Sim.Module.Data.Config;
using Sim.Module.Data.State;
using Sim.Module.Generic;
using Sim.Module.Logger;

namespace Sim.Module.Client
{
	public class RealmControllerClient : RealmControllerBase
	{
		public RealmControllerClient(IContext context) : base(context) { }

		public override void SetupPlayer(PlayerState state)
		{
			if(Context.Resolve<SimService>().LocalId.Equals(state.Id))
			{
				SetupLocalPlayer(state);
				Logger.Log(SelfType, Level.Debug, $"setup LOCAL player: id:{state.Id} hero:{state.HeroName} team:{state.TeamId?.ToString() ?? "[undefined]"}", null);
			}
			else
			{
				Logger.Log(SelfType, Level.Debug, $"setup REMOTE player: id:{state.Id} hero:{state.HeroName} team:{state.TeamId?.ToString() ?? "[undefined]"}", null);
			}
		}

		private void SetupLocalPlayer(PlayerState state)
		{
			state.HeroName = Context.Resolve<IRandomService>().GetHero().Name;
			state.ActiveBehavior = new UnitBehaviorHidden();
		}
	}
}