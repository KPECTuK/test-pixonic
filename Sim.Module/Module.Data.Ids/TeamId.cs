using System;
using Sim.Module.Generic;

namespace Sim.Module.Data.Ids
{
	public class TeamId : IdBase<TeamId>, IEquatable<TeamId>
	{
		public TeamId(string id) : base(id) { }
	}
}