//#define TRACE_COMMANDS

using System;
using System.Collections.Generic;
using Sim.Module.Extensions;
using Sim.Module.Logger;

namespace Sim.Module.Generic
{
	public abstract class CommandQueue : ICommand
	{
		private readonly Type _selfType;

		protected readonly ILogger Logger;

		private readonly Queue<ICommand>[] _queue =
		{
			new Queue<ICommand>(), // working
			new Queue<ICommand>(), // staging
		};
		protected readonly object SyncStaging = new object();
		protected readonly object SyncWorking = new object();
		protected Queue<ICommand> WorkingQueue;
		protected Queue<ICommand> StagingQueue;
		private int _workingIndex;
		private int _stagingIndex = 1;

		protected CommandQueue(IContext context)
		{
			_selfType = GetType();
			Logger = context.Resolve<ILoggerFactory>().GetFor(_selfType);
			WorkingQueue = _queue[0];
			StagingQueue = _queue[1];
		}

		protected interface ICommandTypeExtractor
		{
			Type CommandType { get; }
		}

		private class ParameterWrapper<TPayload> : ICommand, ICommandTypeExtractor
		{
			private readonly ICommand<TPayload> _command;
			private readonly TPayload _payload;

			public Type CommandType => _command?.GetType();

			public ParameterWrapper(ICommand<TPayload> command, TPayload payload)
			{
				_command = command;
				_payload = payload;
			}

			public void Execute()
			{
				_command.Execute(_payload);
			}
		}

		public virtual void Enqueue(ICommand command)
		{
			lock(SyncStaging)
			{
				StagingQueue.Enqueue(command);
			}
		}

		public virtual void Enqueue<TPayload>(ICommand<TPayload> command, TPayload payload)
		{
			lock(SyncStaging)
			{
				StagingQueue.Enqueue(new ParameterWrapper<TPayload>(command, payload));
			}
		}

		protected void SwapQueues()
		{
			lock(SyncStaging)
			{
				_workingIndex = ++_workingIndex % _queue.Length;
				_stagingIndex = ++_stagingIndex % _queue.Length;
				WorkingQueue = _queue[_workingIndex];
				StagingQueue = _queue[_stagingIndex];
			}
		}

		public virtual void Execute()
		{
			var execution = new Queue<ICommand>();
			lock(SyncWorking)
			{
				SwapQueues();
				while(WorkingQueue.Count > 0)
				{
					execution.Enqueue(WorkingQueue.Dequeue());
				}
			}

			while(execution.Count > 0)
			{
				var executive = execution.Dequeue();
				try
				{
					#if TRACE_COMMANDS
					var type = executive is ICommandTypeExtractor
						? (executive as ICommandTypeExtractor).CommandType
						: executive.GetType();
					Logger?.Log(_selfType, Level.Debug, $"executing command: {type.NameNice()}", null);
					executive.Execute();
					#else
					executive.Execute();
					#endif
				}
				catch(Exception exception)
				{
					Logger?.Log(
						executive is ICommandTypeExtractor
							? (executive as ICommandTypeExtractor).CommandType
							: executive.GetType(),
						Level.Error,
						$"exception during execution from: {_selfType.NameNice()}\n{exception.ToText()}",
						exception);
				}
			}
		}

		public void Clear()
		{
			lock(SyncStaging)
			{
				StagingQueue.Clear();
			}
		}
	}
}