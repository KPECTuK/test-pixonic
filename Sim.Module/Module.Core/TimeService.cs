using Sim.Module.Generic;

namespace Sim.Module.Core
{
	public class TimeService
	{
		private readonly IContext _context;

		public TimeService(IContext context)
		{
			_context = context;
		}
	}
}