using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Data.Config.Behavior
{
	public class UnitBehaviorHidden : UnitBehaviorBase<UnitBehaviorHidden>
	{
		public override void ModifyState(IContext context, PlayerState state) { }

		public override UnitBehaviorHidden Copy()
		{
			var result = base.Copy();
			return result;
		}
	}
}