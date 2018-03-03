using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sim.Module.Data.Config;
using Sim.Module.Data.Ids;
using Sim.Module.Extensions;
using Sim.Module.Logger;

namespace Sim.Module.Generic
{
	public abstract class CommandBuilderBase : ICommandBuilder
	{
		// assume transport is reelable and not sequential

		protected class LagDesc
		{
			public PlayerId PlayerId { get; set; }
			public RequestId RequestId { get; set; }
			public DateTime BuildTimeStamp { get; set; }
			public Type RequestType { get; set; }
		}

		protected readonly Type SelfType;
		protected readonly ILogger Logger;
		protected readonly IContext Context;

		private readonly List<LagDesc> _inProgress = new List<LagDesc>();
		private readonly Dictionary<PlayerId, TimeSpan> _lag = new Dictionary<PlayerId, TimeSpan>();

		protected abstract Dictionary<Type, ConstructorInfo> Correspondences { get; } // conformity

		public TimeSpan CurrentServerUpdateInterval =>
			_lag.Values.Count > 0
				? _lag.Values.Max()
				: Context.Resolve<RealmData>().TargetSyncInterval;

		public CommandBuilderBase(IContext context)
		{
			Context = context;
			SelfType = GetType();
			Logger = Context.Resolve<ILoggerFactory>().GetFor(SelfType);
		}

		public bool IsInProgress(RequestId requestId)
		{
			return _inProgress.Exists(_ => _.RequestId.Equals(requestId));
		}

		public bool IsInProgress(PlayerId playerId)
		{
			return _inProgress.Exists(_ => _.PlayerId.Equals(playerId));
		}

		public bool IsInProgress<TRequest>(PlayerId playerId) where TRequest : CommandExternal
		{
			return _inProgress.Exists(_ => ReferenceEquals(typeof(TRequest), _.RequestType) && playerId.Equals(_.PlayerId));
		}

		public TRequest BuildRequest<TRequest>(PlayerId id) where TRequest : CommandExternal
		{
			ConstructorInfo ctor;
			if(!Correspondences.TryGetValue(typeof(TRequest), out ctor))
			{
				throw new NotImplementedException("request is not recognized");
			}

			var result = ctor.Invoke(null) as TRequest;
			if(!ReferenceEquals(null, result))
			{
				var lag = new LagDesc
				{
					PlayerId = id,
					RequestId = GetNextRequestId(),
					BuildTimeStamp = DateTime.UtcNow,
				};

				result.PlayerId = lag.PlayerId;
				result.RequestId = lag.RequestId;
				result.BuildTimeStamp = lag.BuildTimeStamp;

				_inProgress.Add(lag);
			}

			return result;
		}

		public TResponse BuildResponse<TResponse, TRequest>(TRequest request)
			where TResponse : CommandExternal
			where TRequest : CommandExternal
		{
			ConstructorInfo ctor;
			if(!Correspondences.TryGetValue(typeof(TRequest), out ctor))
			{
				throw new NotImplementedException("request is not recognized");
			}

			var result = ctor.Invoke(null) as TResponse;
			result.PlayerId = request.PlayerId;
			result.RequestId = request.RequestId;
			result.BuildTimeStamp = DateTime.UtcNow;
			// update interval

			return result;
		}

		public void CompleteRequest(ICommand response)
		{
			var external = response as CommandExternal;
			if(ReferenceEquals(null, external))
			{
				return;
			}

			var request = _inProgress.FindAll(_ => _.PlayerId.Equals(external.PlayerId) && _.RequestId.Equals(external.RequestId));
			var lag = DateTime.UtcNow - (request.FirstOrDefault()?.BuildTimeStamp ?? DateTime.UtcNow);
			_lag[external.PlayerId] = lag;
			_inProgress.RemoveAll(_ => _.PlayerId.Equals(external.PlayerId) && _.RequestId.Equals(external.RequestId));
		}

		public void UpdateServerInterval()
		{
			//! maintain outdated

			//CurrentServerUpdateInterval = _inProgress.Values.Count == 0
			//	? TimeSpan.Zero
			//	: _inProgress.Values.Max(_ => _.Lag);
		}

		private RequestId GetNextRequestId()
		{
			return new RequestId("request_".MakeUnique());
		}

		public void Dispose()
		{
			_inProgress.ToList().ForEach(_ => Logger.Log(SelfType, Level.Debug, $"player: {_.PlayerId} request: {_.RequestId} timestamp: {_.BuildTimeStamp}", null));
		}
	}
}