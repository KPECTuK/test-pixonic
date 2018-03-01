namespace Sim.Module.Data
{
	public abstract class UnitBehaviorBase<T> : IUnitBehavior, IDataContainer<T> where T : UnitBehaviorBase<T>
	{
		public abstract T Copy();
	}
}