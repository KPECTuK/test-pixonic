using Sim.Module.Generic;

namespace Sim.Module.Simulation
{
	public class SimulatorCommandQueue : CommandQueueDelayed
	{
		public SimulatorCommandQueue(IContext context) : base(context) { }
	}
}