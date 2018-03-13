namespace Sim.Module.Data.Config
{
	public abstract class ModRouterBase<T> : IModRouter, IDataContainer<T> where T : ModRouterBase<T>, new()
	{
		public virtual T Copy()
		{
			return new T();
		}

		IModRouter IModRouter.Copy()
		{
			return Copy();
		}
	}
}