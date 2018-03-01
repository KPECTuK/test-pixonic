using System;

namespace Sim.Module.Data
{
	public abstract class ModRouterBase<T> : IModRouter, IDataContainer<T> where T : ModRouterBase<T>
	{
		public T Copy()
		{
			throw new NotImplementedException();
		}
	}
}