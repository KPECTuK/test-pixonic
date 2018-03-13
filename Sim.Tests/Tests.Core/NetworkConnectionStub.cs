using Sim.Module.Client;
using Sim.Module.Data.Ids;
using Sim.Module.Generic;

namespace Sim.Tests.Core
{
	public class NetworkConnectionStub : INetworkClientConnection
	{
		public PlayerId PlayerId { get; }
		public bool IsEstablished { get; }

		public void SendToServer(ICommand command) { }

		public void GenerateNetworkLagTargetInterval() { }
	}
}