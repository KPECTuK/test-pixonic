namespace Sim.Module.Data.Config
{
	public abstract class UnitBehaviorBase<T> : IUnitBehavior, IDataContainer<T> where T : UnitBehaviorBase<T>, new()
	{
		private IUnitBehavior _unitBehaviorImplementation;

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