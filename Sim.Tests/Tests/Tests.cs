using NUnit.Framework;
using Sim.Module.Core;
using Sim.Tests.Core;

namespace Sim.Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void Test()
		{
			var context = new TestsCompositionRoot();
			context.Resolve<SimService>().Initialize();
			context.Release();
		}
	}
}