using System;
using System.Linq;
using Sim.Module.Client;
using Sim.Module.Command;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Generic;
using Sim.Module.Logger;
using Sim.Module.Simulation.Transport;
using Sim.Module.Simulator.Data;

namespace Sim.Module.Simulation
{
	public class SimulatorServer : IDisposable
	{
		private readonly Type _selfType;
		private readonly ILogger _logger;
		private readonly IContext _context;
		private IContext[] _clientContexts;
		private SimulatorConnection[] _connections;
		private DateTime _lastUpdate;
		private bool _disposed;

		public SimulatorServer(IContext context)
		{
			_context = context;
			_selfType = GetType();
			_logger = _context.Resolve<ILoggerFactory>().GetFor(_selfType);
		}

		private IContext BuildClient(IContext context, ConnectionData config)
		{
			var root = context.Resolve<IRootContext>();
			var moduleContext = new ClientCompositionRoot(root);
			var connection = new SimulatorConnection(context) { ConnectionData = config };
			_connections = (_connections ?? new SimulatorConnection[] { }).Concat(new[] { connection }).ToArray();
			moduleContext.RegisterInstance<INetworkClientConnection>(connection);
			return moduleContext;
		}

		public void Initialize()
		{
			_context.Resolve<IRepository>().ReloadConfig();
			_context.Resolve<IRepository>().ReloadState(_context.Resolve<RealmData>().TotalTeams);
			_clientContexts = _context
				.Resolve<ServerData>()
				.Clients
				.Select(_ => BuildClient(_context, _))
				.ToArray();
			_lastUpdate = DateTime.UtcNow;
			Array.ForEach(_connections, _ => _.Initialize());
			Array.ForEach(_clientContexts, _ =>
			{
				var simService = _.Resolve<SimService>();
				var repository = _.Resolve<IRepository>();
				simService.Initialize();
				var command = _.Resolve<ICommandBuilder>().BuildRequest<CommandJoinRequest>(simService.LocalId);
				command.PlayerState = repository.GetPlayerStates(_1 => _1?.Id.Equals(simService.LocalId) ?? false).First();
				simService.Send(command);
			});
		}

		public IContext GetConnectionContext(PlayerId id)
		{
			return _clientContexts.FirstOrDefault(_ => _.Resolve<INetworkClientConnection>().PlayerId.Equals(id));
		}

		public SimulatorConnection GetConnection(PlayerId id)
		{
			return _connections.FirstOrDefault(_ => _.PlayerId.Equals(id));
		}

		public void Update()
		{
			Array.ForEach(_connections, _ => _.UpdateIncoming());
			Array.ForEach(_connections, _ => _.UpdateOutgoing());
			Array.ForEach(_clientContexts, _ => _.Resolve<SimService>().UpdateSimulation());

			if(!Array.TrueForAll(_connections, _ => _.IsEstablished))
			{
				return;
			}

			var builder = _context.Resolve<ICommandBuilder>();
			if(builder.CurrentServerUpdateInterval > DateTime.UtcNow - _lastUpdate)
			{
				return;
			}

			_lastUpdate = DateTime.UtcNow;
			Array.ForEach(_connections, SendSync);
			_context.Resolve<IRepository>().ShiftStates();

			_logger.Log(_selfType, Level.Info, $"update interval: {builder.CurrentServerUpdateInterval.TotalMilliseconds} ms", null);
		}

		private void SendSync(SimulatorConnection connection)
		{
			var builder = _context.Resolve<ICommandBuilder>();
			if(builder.IsInProgress<CommandStateSyncRequest>(connection.PlayerId))
			{
				return;
			}

			var command = builder.BuildRequest<CommandStateSyncRequest>(connection.PlayerId);
			command.Deference = _context.Resolve<IRepository>().GetPlayerStates(null).ToArray();
			connection.SendToClient(command);
		}

		public void Release()
		{
			Array.ForEach(_clientContexts, _ => _.Release());
		}

		public void Dispose()
		{
			if(_disposed)
			{
				return;
			}

			_disposed = true;
			Release();
		}
	}
}