using System.Reflection;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Generic;
using Sim.Module.Resources;
using Sim.Module.Simulation.Transport;
using Sim.Module.Simulator.Data;

namespace Sim.Module.Simulation
{
	public class ServerCompositionRoot : ServiceLocator
	{
		public ServerCompositionRoot(IContext context) : base(context)
		{
			RegisterComponents();
		}

		public ServerCompositionRoot()
		{
			RegisterComponents();
		}

		private void RegisterComponents()
		{
			RegisterInstance(ResourceFactory.GetFactory(Assembly.GetExecutingAssembly(), "Sim.Resources"));
			RegisterInstance(new SimulatorServer(this));
			var repository = new RepositoryServer(this);
			RegisterInstance<IRepository>(repository);
			RegisterProvider<ServerData>(repository);
			RegisterProvider<RealmData>(repository);
			RegisterInstance(new MatchMaker(this));
			//
			RegisterInstance<IRealmController>(new RealmControllerClient(this));
			RegisterInstance<ICommandBuilder>(new ServerCommandBuilder(this));
			RegisterInstance<IRandomService>(new UtilityService(this));
		}
	}
}