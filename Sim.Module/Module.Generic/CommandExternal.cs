using System;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;

namespace Sim.Module.Generic
{
	public abstract class CommandExternal : ICommand, IContextInjector, IEquatable<PlayerId>
	{
		public IContext Context { protected get; set; }
		public TimeSpan ServerUpdateInterval { get; set; }
		public DateTime BuildTimeStamp { get; set; }
		public PlayerId PlayerId { get; set; }
		public RequestId RequestId { get; set; }

		public abstract void Execute();

		// IEquatable<PlayerId>
		public bool Equals(PlayerId other)
		{
			return !ReferenceEquals(null, other) && PlayerId.Equals(other);
		}
	}
}