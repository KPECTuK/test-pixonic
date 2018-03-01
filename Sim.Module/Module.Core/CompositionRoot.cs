using System.Reflection;
using Sim.Module.Data;
using Sim.Module.Generic;
using Sim.Module.Logger;
using Sim.Module.Resources;

namespace Sim.Module.Core
{
	public class CompositionRoot : ServiceLocator
	{
		public CompositionRoot()
		{
			RegisterInstance<ILoggerFactory>(new DefaultLoggerFactory<DefaultLoggerImpl>());
			// ReSharper disable once RedundantTypeArgumentsOfMethod
			RegisterInstance<Repository>(new Repository(this));
			RegisterInstance(ResourceFactory.GetFactory(Assembly.GetExecutingAssembly(), "Sim.Resources"));
			//
			RegisterInstance(new TimeService(this));
			RegisterInstance(new SimService(this));
		}
	}
}