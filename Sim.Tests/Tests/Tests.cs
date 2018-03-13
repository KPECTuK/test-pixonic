using System;
using NUnit.Framework;
using Sim.Module.Client;
using Sim.Module.Data;
using Sim.Module.Data.Ids;
using Sim.Module.Extensions;
using Sim.Module.Simulation;
using Sim.Tests.Core;

namespace Sim.Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void InstantiateServiceTest()
		{
			var context = new ClientCompositionRoot(new TestsCompositionRoot(null));
			context.RegisterInstance<INetworkClientConnection>(new NetworkConnectionStub());
			context.Resolve<SimulationService>().Initialize();
			context.Release();
		}

		[Test]
		public void InstantiateSimulatorTest()
		{
			var context = new ServerCompositionRoot(new TestsCompositionRoot(null));
			var server = context.Resolve<SimulatorServer>();
			server.Initialize();
			//var startTime = DateTime.UtcNow;
			//var period = TimeSpan.FromSeconds(10);
			//for(; DateTime.UtcNow - startTime < period;)
			//{
			//	server.Update();
			//}
			context.Release();
		}

		[Test]
		public void RepositoryInitializeTest()
		{
			var context = new ClientCompositionRoot(new TestsCompositionRoot(null));
			var repository = context.Resolve<IRepository>();
			context.Resolve<SimulationService>().LocalId = new PlayerId("player_".MakeUnique());
			repository.ReloadConfig();
			repository.ReloadState(2);
			repository.SetTeam(new TeamId("team_".MakeUnique()));
			repository.SetTeam(new TeamId("team_".MakeUnique()));
			repository.ShiftStates();
			repository.ShiftStates();
			repository.ShiftStates();
			repository.ShiftStates();
			context.Release();
		}
	}
}