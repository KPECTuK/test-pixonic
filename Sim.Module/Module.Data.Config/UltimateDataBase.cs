using Newtonsoft.Json;

namespace Sim.Module.Data.Config
{
	public abstract class UltimateDataBase<T> : IUltimateData, IDataContainer<T> where T : UltimateDataBase<T>, new()
	{
		[JsonProperty("stacking")] public bool IsStackBlocker { get; set; }
		[JsonProperty("router")] public IModRouter Router { get; set; }

		public virtual T Copy()
		{
			return new T
			{
				IsStackBlocker = IsStackBlocker,
				Router = Router.Copy(),
			};
		}

		IUltimateData IUltimateData.Copy()
		{
			return Copy();
		}
	}
}