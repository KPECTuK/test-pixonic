using System;
using Sim.Module.Generic;

namespace Sim.Module.Data.Ids
{
	public class PlayerId : IdBase<PlayerId>, IEquatable<PlayerId>
	{
		public PlayerId(string id) : base(id) { }
	}
}