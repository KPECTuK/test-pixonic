using Newtonsoft.Json;

namespace Sim.Module.Simulator.Data
{
	public class ServerData
	{
		[JsonProperty("clients")] public ConnectionData[] Clients { get; set; }
	}
}