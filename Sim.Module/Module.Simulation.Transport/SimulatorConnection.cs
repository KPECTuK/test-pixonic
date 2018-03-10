using System;
using Sim.Module.Client;
using Sim.Module.Data.Ids;
using Sim.Module.Extensions;
using Sim.Module.Generic;
using Sim.Module.Logger;
using Sim.Module.Simulator.Data;

namespace Sim.Module.Simulation.Transport
{
	public class SimulatorConnection : INetworkClientConnection
	{
		private readonly Type _selfType;
		private readonly ILogger _logger;
		private readonly IContext _context;
		private readonly CommandQueueDelayed _incoming;
		private readonly CommandQueueDelayed _outgoing;

		public ConnectionData ConnectionData { get; set; }
		public PlayerId PlayerId { get; set; }
		public TimeSpan NetworkLagTarget { get; private set; }
		public bool IsEstablished { get; set; }

		public SimulatorConnection(IContext context)
		{
			_context = context;
			_selfType = GetType();
			_logger = _context.Resolve<ILoggerFactory>().GetFor(_selfType);
			_incoming = new SimulatorCommandQueue(_context);
			_outgoing = new SimulatorCommandQueue(_context);
		}

		public void Initialize()
		{
			GenerateNetworkLagTargetInterval();
			_logger.Log(_selfType, Level.Debug, $"connection initialized: player: {PlayerId} lag: {NetworkLagTarget.TotalMilliseconds} ms", null);
		}

		public void SendToServer(ICommand command)
		{
			PlayerId = PlayerId ?? (command as CommandExternal)?.PlayerId ?? PlayerId;
			_context.TryInject(command);
			_incoming.Enqueue(command, NetworkLagTarget);
			_context.Resolve<ICommandBuilder>().CompleteRequest(command);
		}

		public void UpdateIncoming()
		{
			_incoming.Execute();
		}

		public void SendToClient(ICommand command)
		{
			_outgoing.Enqueue(command, NetworkLagTarget);
		}

		public void UpdateOutgoing()
		{
			ICommand command;
			while(_outgoing.TryConsume(out command))
			{
				_context.Resolve<SimulatorServer>().GetConnectionContext(PlayerId).Resolve<SimulationService>().OnReceived(command);
			}
		}

		public void GenerateNetworkLagTargetInterval()
		{
			var variance =
				2.0 *
				ConnectionData.AverageLatencyVariance.TotalMilliseconds *
				_context.Resolve<IRandomService>().GetNormalized() -
				ConnectionData.AverageLatencyVariance.TotalMilliseconds;
			NetworkLagTarget = TimeSpan.FromMilliseconds(variance + ConnectionData.AverageLatency.TotalMilliseconds);
		}
	}
}