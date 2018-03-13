using System.Reflection;
using log4net.Config;
using Sim.Module.Generic;
using Sim.Module.Logger;
using Sim.Module.Resources;

namespace Sim.Launcher
{
	public class LauncherCompositionRoot : ServiceLocator, IRootContext
	{
		public LauncherCompositionRoot(IContext context) : base(context)
		{
			RegisterInstance<IRootContext>(this);
			RegisterInstance<ILoggerFactory>(new DefaultLoggerFactory<LauncherLoggerImpl>());
			RegisterInstance(ResourceFactory.GetFactory(Assembly.GetExecutingAssembly(), "Resources"));
			//
			XmlConfigurator.Configure(Resolve<IResourceFactory>().GetResourceStream(new ResourceLocator { Filename = "log4net.xml" }));
		}
	}
}