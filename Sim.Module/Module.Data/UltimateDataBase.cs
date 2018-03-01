using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public abstract class UltimateDataBase<T> : IUltimateData, IDataContainer<T> where T : UltimateDataBase<T>
	{
		[JsonProperty("stacking")] public bool IsStackBlocker { get; set; }
		[JsonProperty("router")] public IModRouter Router  { get; set; }

		public abstract T Copy();
	}
}