using Sim.Module.Data;
using Sim.Module.Generic;

namespace Sim.Module.Core
{
	public class SimService
	{
		private readonly IContext _context;

		public SimService(IContext context)
		{
			_context = context;
		}

		public void Initialize()
		{
			_context.Resolve<Repository>().Load();
		}
	}
}