//#define TRACE_COMMANDS

using System;
using System.Collections.Generic;
using Sim.Module.Extensions;
using Sim.Module.Logger;

namespace Sim.Module.Generic
{
	public abstract class CommandQueueDelayed : CommandQueue
	{
		private readonly Type _selfType;

		private class CommandWrapper : ICommand
		{
			public readonly ICommand Source;
			public readonly TimeSpan Delay;

			private readonly DateTime _engaged;

			public CommandWrapper(ICommand command, TimeSpan delay)
			{
				_engaged = DateTime.UtcNow;
				Delay = delay;
				Source = command;
			}

			public bool IsExecutable => DateTime.UtcNow - _engaged > Delay;

			public void Execute()
			{
				Source.Execute();
			}
		}

		protected CommandQueueDelayed(IContext context) : base(context)
		{
			_selfType = GetType();
		}

		public override void Enqueue<TPayload>(ICommand<TPayload> command, TPayload payload)
		{
			throw new NotSupportedException("delayed queue with parameter closure");
		}

		public override void Enqueue(ICommand command)
		{
			var wrapper = command as CommandWrapper ?? new CommandWrapper(command, TimeSpan.Zero);
			base.Enqueue(wrapper);
		}

		public virtual void Enqueue(ICommand command, TimeSpan delay)
		{
			base.Enqueue(new CommandWrapper(command, delay));
		}

		public bool TryConsume(out ICommand result)
		{
			//! slow 
			var execution  = new Queue<ICommand>();
			result = null;
			lock(SyncWorking)
			{
				SwapQueues();
				while(WorkingQueue.Count > 0)
				{
					var command = WorkingQueue.Dequeue();
					var wrapper = command as CommandWrapper;
					if(!wrapper?.IsExecutable ?? false)
					{
						Enqueue(command);
					}
					else
					{
						if(ReferenceEquals(null, result))
						{
							result = wrapper?.Source;
						}
						else
						{
							execution.Enqueue(command);
						}
					}
				}

				while(execution.Count > 0)
				{
					Enqueue(execution.Dequeue());
				}
			}

			return !ReferenceEquals(null, result);
		}

		public override void Execute()
		{
			var execution = new Queue<ICommand>();
			lock(SyncWorking)
			{
				SwapQueues();
				while(WorkingQueue.Count > 0)
				{					var command = WorkingQueue.Dequeue();
					var wrapper = command as CommandWrapper;
					if(!wrapper?.IsExecutable ?? false)
					{
						Enqueue(command);
					}
					else
					{
						execution.Enqueue(command);
					}

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
	}
}