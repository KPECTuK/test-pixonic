using System;
using Sim.Module.Client;
using Sim.Module.Data.Ids;
using Sim.Module.Generic;

namespace Sim.Tests.Core
{
	public class NetworkConnectionStub : INetworkClientConnection
	{
		public PlayerId PlayerId { get; }
		public TimeSpan NetworkLagTarget => TimeSpan.FromMilliseconds(100);

		public void SendToServer(ICommand command) { }
	}
}