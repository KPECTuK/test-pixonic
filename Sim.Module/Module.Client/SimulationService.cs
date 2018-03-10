using System;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Extensions;
using Sim.Module.Generic;
using Sim.Module.Logger;

namespace Sim.Module.Client
{
	public class SimulationLink
	{

	}

	public class SimulationService
	{
		private readonly IContext _context;
		private readonly Type _selfType;
		private readonly ILogger _logger;

		public PlayerId LocalId { get; set; }

		public SimulationService(IContext context)
		{
			_context = context;
			_selfType = GetType();
			_logger = _context.Resolve<ILoggerFactory>().GetFor(_selfType);
		}

		public void Initialize()
		{
			LocalId = new PlayerId("p_".MakeUnique());
			var repository = _context.Resolve<IRepository>();
			repository.ReloadConfig();
			repository.ReloadState(_context.Resolve<RealmData>().TotalTeams);
			BuildBehaviorPipeline();
		}

		public void Send(ICommand command)
		{
			var external = command as CommandServerSide;
			if(ReferenceEquals(null, external))
			{
				_context.TryInject(command);
				_context.Resolve<ClientCommandQueue>().Enqueue(command);
			}
			else
			{
				_context.Resolve<INetworkClientConnection>().SendToServer(command);
			}
		}

		public void OnReceived(ICommand command)
		{
			_context.Resolve<ICommandBuilder>().CompleteRequest(command);
			_context.TryInject(command);
			_context.Resolve<ClientCommandQueue>().Enqueue(command);
		}

		public void BuildBehaviorPipeline() { }

		public void UpdateSimulation()
		{
			_context.Resolve<ClientCommandQueue>().Execute();
			_context.Resolve<INetworkClientConnection>().GenerateNetworkLagTargetInterval();
		}
	}
}