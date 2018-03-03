using Newtonsoft.Json;

namespace Sim.Module.Data.Config
{
	public class ModRouterAlly : ModRouterBase<ModRouterAlly>
	{
		[JsonProperty("self-apply")] public bool IsApplySelf { get; set; }

		public override ModRouterAlly Copy()
		{
			var result = base.Copy();
			result.IsApplySelf = IsApplySelf;
			return result;
		}
	}
}