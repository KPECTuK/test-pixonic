using Sim.Module.Generic;
using Sim.Module.Logger;

namespace Sim.Tests.Core
{
	public class TestsCompositionRoot : ServiceLocator, IRootContext
	{
		public TestsCompositionRoot(IContext context) : base(context)
		{
			RegisterInstance<IRootContext>(this);
			RegisterInstance<ILoggerFactory>(new DefaultLoggerFactory<TestsLoggerImpl>());
		}
	}
}