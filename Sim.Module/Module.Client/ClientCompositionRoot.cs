using System.Reflection;
using Sim.Module.Data;
using Sim.Module.Generic;
using Sim.Module.Resources;
using Sim.Module.Simulation.Transport;

namespace Sim.Module.Client
{
	public class ClientCompositionRoot : ServiceLocator
	{
		public ClientCompositionRoot(IContext context) : base(context)
		{
			RegisterComponents();
		}

		public ClientCompositionRoot()
		{
			RegisterComponents();
		}

		private void RegisterComponents()
		{
			var repository = new RepositoryClient(this);
			RegisterInstance<IRepository>(repository);
			RegisterProvider(repository);
			RegisterInstance(ResourceFactory.GetFactory(Assembly.GetExecutingAssembly(), "Sim.Resources"));
			RegisterInstance<IRealmController>(new RealmControllerClient(this));
			var utility = new UtilityService(this);
			RegisterInstance<IRandomService>(utility);
			RegisterInstance<ITimeService>(utility);
			RegisterInstance<ICommandBuilder>(new ClientCommandBuilder(this));
			RegisterInstance(new SimService(this));
			RegisterInstance(new ClientCommandQueue(this));
		}
	}
}