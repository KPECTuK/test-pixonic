using Sim.Module.Client;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Data.Config.Behavior
{
	public class UnitBehaviorDefault : UnitBehaviorBase<UnitBehaviorDefault>
	{
		public override void ModifyState(IContext context, PlayerState state)
		{
			var timeService = context.Resolve<ITimeService>();

		}

		public override UnitBehaviorDefault Copy()
		{
			var result = base.Copy();
			return result;
		}
	}
}