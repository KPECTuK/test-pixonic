using System;
using Sim.Module.Data.Ids;

namespace Sim.Module.Generic
{
	public interface ICommandBuilder : IDisposable
	{
		TimeSpan CurrentServerUpdateInterval { get; }

		TResponse BuildResponse<TResponse, TRequest>(TRequest request) where TResponse : CommandExternal where TRequest : CommandExternal;
		TRequest BuildRequest<TRequest>(PlayerId id) where TRequest : CommandExternal;
		void UpdateServerInterval();
		bool IsInProgress(RequestId requestId);
		bool IsInProgress(PlayerId playerId);
		bool IsInProgress<TRequest>(PlayerId playerId) where TRequest : CommandExternal;
		void CompleteRequest(ICommand response);
	}
}