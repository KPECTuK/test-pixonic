using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Data
{
	public interface IUnitBehavior
	{
		void ModifyState(IContext context, PlayerState state);
		IUnitBehavior Copy();
	}
}