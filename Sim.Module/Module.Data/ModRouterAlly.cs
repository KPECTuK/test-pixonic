using Newtonsoft.Json;

namespace Sim.Module.Data
{
	public class ModRouterAlly : ModRouterBase<ModRouterAlly>
	{
		[JsonProperty("self-apply")] public bool IsApplySelf { get; set; }
	}
}