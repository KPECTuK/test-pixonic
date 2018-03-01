using System;
using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class RealmData : IDataContainer<RealmData>
	{
		[JsonProperty("tick-time")] public float TicksPerSecond { get; set; }

		public RealmData Copy()
		{
			throw new NotImplementedException();
		}
	}
}