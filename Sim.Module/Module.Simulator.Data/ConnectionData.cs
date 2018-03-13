using System;
using Newtonsoft.Json;

namespace Sim.Module.Simulator.Data
{
	public class ConnectionData
	{
		[JsonProperty("latency")] public TimeSpan AverageLatency { get; set; }
		[JsonProperty("variance")] public TimeSpan AverageLatencyVariance { get; set; }
		[JsonProperty("lost")] public float PacketsLost { get; set; }
	}
}