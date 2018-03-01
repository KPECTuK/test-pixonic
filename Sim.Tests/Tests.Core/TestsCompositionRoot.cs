using Sim.Module.Core;
using Sim.Module.Generic;
using Sim.Module.Logger;

namespace Sim.Tests.Core
{
	public class TestsCompositionRoot : ServiceLocator
	{
		public TestsCompositionRoot() : base(new CompositionRoot())
		{
			RegisterInstance(new DefaultLoggerFactory<TestsLoggerImpl>());
		}
	}
}