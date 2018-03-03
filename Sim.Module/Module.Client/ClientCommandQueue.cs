using Sim.Module.Generic;

namespace Sim.Module.Client
{
	public class ClientCommandQueue : CommandQueue
	{
		public ClientCommandQueue(IContext context) : base(context) { }
	}
}