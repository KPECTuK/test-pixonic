using System;
using System.Collections.Generic;
using System.Linq;
using Sim.Module.Client;
using Sim.Module.Command;
using Sim.Module.Data;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using Sim.Module.Extensions;
using Sim.Module.Generic;
using Sim.Module.Logger;
using Sim.Module.Simulation.Transport;

namespace Sim.Module.Simulation
{
	public class MatchMaker
	{
		private readonly Type _selfType;
		private readonly IContext _context;
		private readonly ILogger _logger;
		private readonly List<CommandJoinResponse> _joinCommands = new List<CommandJoinResponse>();

		public MatchMaker(IContext context)
		{
			_context = context;
			_selfType = GetType();
			_logger = _context.Resolve<ILoggerFactory>().GetFor(_selfType);
		}

		public void AddRequester(CommandJoinRequest command)
		{
			var repository = _context.Resolve<IRepository>();
			_logger.Log(_selfType, Level.Debug, $"join request from: {command.PlayerState.Id}", null);

			repository.MergeStates(new[] { command.PlayerState.Copy() });
			_joinCommands.Add(_context.Resolve<ICommandBuilder>().BuildResponse<CommandJoinResponse, CommandJoinRequest>(command));

			if(_joinCommands.Count < _context.Resolve<RealmData>().TotalPlayers)
			{
				return;
			}

			DistributeTeams();
			SendJoinResponses();

			_joinCommands.Clear();

			_logger.Log(_selfType, Level.Notice, $"invites had been sent, all estableshed: {_context.Resolve<ICommandBuilder>().CurrentServerUpdateInterval.TotalMilliseconds} ms", null);
		}

		private void DistributeTeams()
		{
			var repository = _context.Resolve<IRepository>();
			var teamsTotal = _context.Resolve<RealmData>().TotalTeams;
			for (var index = 0; index < teamsTotal; index++)
			{
				repository.SetTeam(new TeamId("team_".MakeUnique()));
			}

			var states = repository.GetPlayerStates(null).ToArray();
			for (var index = 0; index < states.Length; index++)
			{
				states[index].TeamId = repository.Teams[index / teamsTotal];
				states[index].Position = _context.Resolve<IRealmController>().GetSpawnPoint(states[index].TeamId);
			}
			repository.ShiftStates();
		}

		private void SendJoinResponses()
		{
			var builder = _context.Resolve<ICommandBuilder>();
			var repository = _context.Resolve<IRepository>();
			builder.UpdateServerInterval();
			foreach(var command in _joinCommands)
			{
				var client = _context.Resolve<SimulatorServer>().GetConnection(command.PlayerId);
				var state = repository.GetPlayerStates(_ => client.PlayerId.Equals(_.Id)).First();
				command.PlayerStates = repository.GetPlayerStates(null).ToArray();
				command.ServerUpdateInterval = _context.Resolve<ICommandBuilder>().CurrentServerUpdateInterval;
				_logger.Log(_selfType, Level.Debug, $"sending join accept to: player {state.Id} with team: {state.TeamId}", null);
				client.SendToClient(command);
				client.IsEstablished = true;
			}
		}
	}
}