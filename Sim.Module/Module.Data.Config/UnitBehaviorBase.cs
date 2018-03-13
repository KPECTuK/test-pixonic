using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Data.Config
{
	public abstract class UnitBehaviorBase<T> : IUnitBehavior, IDataContainer<T> where T : UnitBehaviorBase<T>, new()
	{
		public abstract void ModifyState(IContext context, PlayerState state);

		public virtual T Copy()
		{
			return new T();
		}

		IUnitBehavior IUnitBehavior.Copy()
		{
			return Copy();
		}
	}
}