using System;
using Sim.Module.Generic;

namespace Sim.Module.Data.Ids
{
	public class RequestId : IdBase<RequestId>, IEquatable<RequestId>
	{
		public RequestId(string id) : base(id) { }
	}
}